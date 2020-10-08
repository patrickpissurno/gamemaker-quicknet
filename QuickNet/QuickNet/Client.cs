using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace QuickNet
{
    internal class Client
    {
        #region Singleton
        public static Client instance;

        public static Client GetInstance()
        {
            if (instance == null)
                instance = new Client();
            return instance;
        }
        #endregion

        private EventBasedNetListener listener;
        private NetManager client;
        private Thread mainThread;
        private NetPeer host;
        private bool started = false;
        private bool stop = false;
        private int id = -1;

        private ConcurrentQueue<(string key, string data)> inboundQueue;
        private ConcurrentQueue<(string key, object data)> reliableOutboundQueue;

        private Dictionary<string, object> outboundCache;

        public void Connect(string ip, string port)
        {
            if (started)
                return;

            id = -1;

            inboundQueue = new ConcurrentQueue<(string key, string data)>();
            reliableOutboundQueue = new ConcurrentQueue<(string key, object data)>();
            outboundCache = new Dictionary<string, object>();

            listener = new EventBasedNetListener();
            client = new NetManager(listener);
            client.Start();
            host = client.Connect(ip, int.Parse(port), "");

            started = true;

            listener.NetworkReceiveEvent += NetworkReceived;
            listener.NetworkErrorEvent += (s, e) => Disconnect();

            mainThread = new Thread(MainThread) { IsBackground = true };
            mainThread.Start();
        }

        public void Disconnect()
        {
            if (!started || stop)
                return;

            stop = true;
            client.DisconnectAll();
        }

        private void CleanupAndReset()
        {
            client.Stop();
            client = null;
            listener = null;
            mainThread = null;

            stop = false;
            started = false;
        }

        public string PollQueue()
        {
            (string key, string data) t = (null, null);

            if (inboundQueue?.TryDequeue(out t) == true)
            {
                return $"{t.key}={t.data}";
            }
            return null;
        }

        public int GetId()
        {
            return id;
        }

        public void ReliablePut(string key, object value)
        {
            if (!outboundCache.ContainsKey(key) || !Utils.CacheEntryEquals(outboundCache[key], value))
            {
                outboundCache[key] = value;
                reliableOutboundQueue.Enqueue((key, value));
            }
        }

        private void NetworkReceived(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            try
            {
                while (!reader.EndOfData)
                {

                    var data = Serializer.DeserializeData(reader);
                    if (data != null)
                    {
                        if (id == -1 && data.Value.key == "connected_id")
                            id = int.Parse(data.Value.data);

                        inboundQueue.Enqueue(((string key, string data))data);
                    }
                }
            }
            catch(Exception ex)
            {
                Utils.Log("[NetworkReceived] thrown an exception. Stack:");
                Utils.Log(ex);
            }
            finally
            {
                reader.Recycle();
            }
        }

        private void MainThread()
        {
            while (!stop)
            {
                client.PollEvents();

                bool empty = true;

                var writer = new NetDataWriter();
                while (reliableOutboundQueue.TryDequeue(out var t))
                {
                    Serializer.SerializeData(writer, t);
                    empty = false;
                }

                if(!empty)
                    host.Send(writer, DeliveryMethod.ReliableOrdered);

                Thread.Sleep(15);
            }

            // this method needs to be called in order to be able to connect to another server later
            CleanupAndReset();
        }
    }
}
