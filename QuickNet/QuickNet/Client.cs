using LiteNetLib;
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

        public void Connect(string ip, string port)
        {
            if (started)
                return;

            listener = new EventBasedNetListener();
            client = new NetManager(listener);
            client.Start();
            client.Connect(ip, int.Parse(port), "");

            started = true;

            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                //Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
                dataReader.Recycle();
            };

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
