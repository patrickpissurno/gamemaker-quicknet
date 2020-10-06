using System;
using System.Runtime.InteropServices;

namespace QuickNet
{
    public static class Main
    {
        [DllExport("server_start", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerStart(string ip, string port, double maxConnections)
        {
            try
            {
                Server.GetInstance().Start(ip, port, (int)maxConnections);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_start' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }
        
        [DllExport("server_stop", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerStop()
        {
            try
            {
                Server.GetInstance().Stop();
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_stop' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_get_connections_count", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerGetConnectionsCount()
        {
            try
            {
                return Server.GetInstance().GetConnectionsCount();
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_get_connections_count' has thrown an exception. Stack:");
                Utils.Log(ex);
                return -1;
            }
        }

        [DllExport("client_connect", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientConnect(string ip, string port)
        {
            try
            {
                Client.GetInstance().Connect(ip, port);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'client_connect' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_disconnect", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientDisconnect()
        {
            try
            {
                Client.GetInstance().Disconnect();
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'client_disconnect' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }
    }
}
