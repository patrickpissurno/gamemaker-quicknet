using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace QuickNet
{
    internal class Server
    {
        #region Singleton
        public static Server instance;

        public static Server GetInstance()
        {
            if (instance == null)
                instance = new Server();
            return instance;
        }
        #endregion

        private EventBasedNetListener listener;
        private NetManager server;
        private Thread mainThread;
        private bool started = false;
        private bool stop = false;

        private ConcurrentQueue<(string key, string data)> inboundQueue;
        private ConcurrentQueue<(string key, object data)> reliableOutboundQueue;
        private AppendOnlyDictionary<(string key, object data)> unreliableOutboundQueue;

        private Dictionary<string, object> outboundCache;

        private static readonly object clientPeerCreationLock = new object();

        public void Start(string ip, string port, int maxConnections)
        {
            if (started)
                return;

            inboundQueue = new ConcurrentQueue<(string key, string data)>();
            reliableOutboundQueue = new ConcurrentQueue<(string key, object data)>();
            unreliableOutboundQueue = new AppendOnlyDictionary<(string key, object data)>();
            outboundCache = new Dictionary<string, object>();

            listener = new EventBasedNetListener();
            server = new NetManager(listener);

            server.Start(IPAddress.Parse(ip), IPAddress.IPv6None, int.Parse(port));

            started = true;

            inboundQueue.Enqueue(("started", "true"));

            listener.ConnectionRequestEvent += request =>
            {
                if (server.ConnectedPeersCount < maxConnections)
                    request.Accept();
                else
                    request.Reject();
            };

            listener.PeerConnectedEvent += peer =>
            {
                ClientPeer client;
                lock (clientPeerCreationLock)
                {
                    if (peer.Tag == null)
                        peer.Tag = new ClientPeer();
                    client = (ClientPeer)peer.Tag;
                }

                outboundCache = new Dictionary<string, object>(); //reset global cache

                inboundQueue.Enqueue(("new_connection", (peer.Id + 1).ToString()));
                client.reliableOutboundQueue.Enqueue(("connected_id", peer.Id + 1));
            };

            listener.PeerDisconnectedEvent += (peer, e) =>
            {
                inboundQueue.Enqueue(("disconnected", (peer.Id + 1).ToString()));
            };

            listener.NetworkReceiveEvent += NetworkReceived;

            mainThread = new Thread(MainThread) { IsBackground = true };
            mainThread.Start();
        }

        private void NetworkReceived(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            try
            {
                while (!reader.EndOfData)
                {

                    var data = Serializer.DeserializeData(reader);
                    if (data != null)
                        inboundQueue.Enqueue(((string key, string data))data);
                }
            }
            catch (Exception ex)
            {
                Utils.Log("[NetworkReceived] thrown an exception. Stack:");
                Utils.Log(ex);
            }
            finally
            {
                reader.Recycle();
            }
        }

        public void Stop()
        {
            if (!started || stop)
                return;

            stop = true;
            server.DisconnectAll();
        }

        private void CleanupAndReset()
        {
            server.Stop();
            server = null;
            listener = null;
            mainThread = null;

            stop = false;
            started = false;
        }

        public int GetConnectionsCount()
        {
            return server?.ConnectedPeersCount ?? 0;
        }

        public string PollQueue()
        {
            (string key, string data) t = (null, null);

            if(inboundQueue?.TryDequeue(out t) == true)
            {
                return $"{t.key}={t.data}";
            }
            return null;
        }

        public void ReliablePut(string key, object value)
        {
            if (key[0] == '!' || !outboundCache.ContainsKey(key) || !Utils.CacheEntryEquals(outboundCache[key], value))
            {
                outboundCache[key] = value;
                reliableOutboundQueue.Enqueue((key, value));
            }
        }
        
        public void ReliablePutTo(int id, string key, object value)
        {
            var peer = server.GetPeerById(id - 1);
            if (peer == null)
                return;

            var bypassCache = key[0] == '!';

            ClientPeer client;
            if (peer.Tag is ClientPeer _client)
            {
                client = _client;

                if (!bypassCache)
                {
                    bool globalContains;
                    var localContains = false;

                    if ((globalContains = outboundCache.ContainsKey(key)) || (localContains = client.outboundCache.ContainsKey(key)))
                    {
                        if (globalContains && Utils.CacheEntryEquals(outboundCache[key], value))
                            return;

                        if (localContains && Utils.CacheEntryEquals(client.outboundCache[key], value))
                            return;
                    }
                }
            }
            else
            {
                lock (clientPeerCreationLock)
                {
                    if (peer.Tag == null)
                        peer.Tag = new ClientPeer();
                    client = (ClientPeer)peer.Tag;
                }
            }

            if(!bypassCache)
                client.outboundCache[key] = value;

            client.reliableOutboundQueue.Enqueue((key, value));
        }

        public void UnreliablePut(string key, object value)
        {
            unreliableOutboundQueue[key] = (key, value);
        }

        private void MainThread()
        {
            while (!stop)
            {
                server.PollEvents();

                if(server.ConnectedPeersCount < 1)
                {
                    while (reliableOutboundQueue.TryDequeue(out _)); // no peers connected, just empty the queue
                }
                else
                {
                    var empty = true;
                    var writer = new NetDataWriter(true);

                    // unreliable
                    if (unreliableOutboundQueue.Count > 0)
                    {
                        var unreliableQueue = unreliableOutboundQueue;
                        unreliableOutboundQueue = new AppendOnlyDictionary<(string key, object data)>();

                        var i = 0;
                        while (i < unreliableQueue.Count)
                        {
                            Serializer.SerializeData(writer, unreliableQueue.Values[i]);
                            empty = false;
                            i++;
                        }

                        if (!empty)
                            server.SendToAll(writer, DeliveryMethod.Sequenced);
                    }

                    // reliable
                    if(!empty)
                        writer.Reset();
                    empty = true;
                    
                    while (reliableOutboundQueue.TryDequeue(out var t))
                    {
                        Serializer.SerializeData(writer, t);
                        empty = false;
                    }

                    var peers = server.ConnectedPeerList;

                    var data = empty || (peers.Count < 2) ? null : writer.CopyData();
                    var initialCapacity = data?.Length;
                    var first = true;

                    foreach (var peer in peers)
                    {
                        var client = (ClientPeer)peer.Tag;

                        if (!first)
                        {
                            if (empty)
                            {
                                writer.Reset();
                            }
                            else
                            {
                                writer.Reset((int)initialCapacity);
                                writer.Put(data);
                            }
                        }

                        var _empty = empty;

                        if (client != null)
                        {
                            while (client.reliableOutboundQueue.TryDequeue(out var t))
                            {
                                Serializer.SerializeData(writer, t);
                                _empty = false;
                            }
                        }

                        if(!_empty)
                            peer.Send(writer, DeliveryMethod.ReliableOrdered);

                        first = false;
                    }
                }

                Thread.Sleep(15);
            }

            // this method needs to be called in order to be able to start another server later
            CleanupAndReset();
        }
    }
}
