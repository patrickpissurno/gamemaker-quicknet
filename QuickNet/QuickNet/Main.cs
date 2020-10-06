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

        [DllExport("server_queue_poll", CallingConvention = CallingConvention.Cdecl)]
        public static string ServerQueuePoll()
        {
            try
            {
                return Server.GetInstance().PollQueue() ?? "0";
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_queue_poll' has thrown an exception. Stack:");
                Utils.Log(ex);
                return "-1";
            }
        }

        [DllExport("server_queue_reliable_put", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerQueueReliablePut(string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, value);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_queue_reliable_put' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_queue_reliable_put_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerQueueReliablePutFloat(string key, double value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, value);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_queue_reliable_put_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_queue_reliable_put_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerQueueReliablePutInt(string key, double value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, (int)value);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_queue_reliable_put_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }
        
        [DllExport("server_queue_reliable_put_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerQueueReliablePutBool(string key, double value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, ((int)value) == 1);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_queue_reliable_put_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_queue_reliable_put_array", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerQueueReliablePutArray(string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, Utils.DecodeArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_queue_reliable_put_array' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_queue_reliable_put_array_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerQueueReliablePutArrayFloat(string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, Utils.DecodeDoubleArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_queue_reliable_put_array_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }
        
        [DllExport("server_queue_reliable_put_array_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerQueueReliablePutArrayInt(string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, Utils.DecodeIntArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_queue_reliable_put_array_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_queue_reliable_put_array_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerQueueReliablePutArrayBool(string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, Utils.DecodeBoolArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_queue_reliable_put_array_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
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

        [DllExport("client_queue_poll", CallingConvention = CallingConvention.Cdecl)]
        public static string ClientQueuePoll()
        {
            try
            {
                return Client.GetInstance().PollQueue() ?? "0";
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_queue_poll' has thrown an exception. Stack:");
                Utils.Log(ex);
                return "-1";
            }
        }
    }
}
