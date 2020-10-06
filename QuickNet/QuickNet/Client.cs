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
                string key;
                DataType type;
                string data;

                while (!reader.EndOfData)
                {
                    type = (DataType)reader.GetByte();

                    switch (type)
                    {
                        case DataType.STRING:
                            data = reader.GetString();
                            break;
                        case DataType.INT:
                            data = reader.GetInt().ToString();
                            break;
                        case DataType.DOUBLE:
                            data = reader.GetDouble().ToString();
                            break;
                        default:
                            continue;
                    }

                    key = reader.GetString(); //TODO: optimize the size needed to transmit the key

                    inboundQueue.Enqueue((key, data));
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
