using LiteNetLib;
using System;
using System.Collections.Concurrent;
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
        private bool started = false;
        private bool stop = false;

        private ConcurrentQueue<(string key, string data)> inboundQueue;

        public void Connect(string ip, string port)
        {
            if (started)
                return;

            inboundQueue = new ConcurrentQueue<(string key, string data)>();

            listener = new EventBasedNetListener();
            client = new NetManager(listener);
            client.Start();
            client.Connect(ip, int.Parse(port), "");

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

        private void NetworkReceived(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            try
            {
                while (!reader.EndOfData)
                {

                    var data = Serializer.DeserializeData(reader);
                    if(data != null)
                        inboundQueue.Enqueue(((string key, string data))data);
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
                Thread.Sleep(15);
            }

            // this method needs to be called in order to be able to connect to another server later
            CleanupAndReset();
        }
    }
}
