using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Concurrent;
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

        public void Start(string ip, string port, int maxConnections)
        {
            if (started)
                return;

            inboundQueue = new ConcurrentQueue<(string key, string data)>();
            reliableOutboundQueue = new ConcurrentQueue<(string key, object data)>();

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
                inboundQueue.Enqueue(("new_connection", peer.Id.ToString()));

                //Console.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
                //NetDataWriter writer = new NetDataWriter();                 // Create writer class
                //writer.Put("Hello client!");                                // Put some string
                //peer.Send(writer, DeliveryMethod.ReliableOrdered);             // Send with reliability
            };

            listener.PeerDisconnectedEvent += (peer, e) =>
            {
                inboundQueue.Enqueue(("disconnected", peer.Id.ToString()));
            };

            mainThread = new Thread(MainThread) { IsBackground = true };
            mainThread.Start();
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
            reliableOutboundQueue.Enqueue((key, value));
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
                    var writer = new NetDataWriter();
                    while (reliableOutboundQueue.TryDequeue(out var t))
                    {
                        Serializer.SerializeData(writer, t);
                    }

                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                }

                Thread.Sleep(15);
            }

            // this method needs to be called in order to be able to start another server later
            CleanupAndReset();
        }
    }
}
