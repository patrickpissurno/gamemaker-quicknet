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

        [DllExport("server_send_reliable", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliable(string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, value);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_reliable_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableFloat(string key, double value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, value);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_reliable_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableInt(string key, double value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, (int)value);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }
        
        [DllExport("server_send_reliable_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableBool(string key, double value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, ((int)value) == 1);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_reliable_array", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableArray(string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, Utils.DecodeArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_array' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_reliable_array_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableArrayFloat(string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, Utils.DecodeDoubleArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_array_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }
        
        [DllExport("server_send_reliable_array_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableArrayInt(string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, Utils.DecodeIntArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_array_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_reliable_array_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableArrayBool(string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePut(key, Utils.DecodeBoolArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_array_bool' has thrown an exception. Stack:");
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

        [DllExport("client_get_id", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientGetId()
        {
            try
            {
                return Client.GetInstance().GetId();
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_get_id' has thrown an exception. Stack:");
                Utils.Log(ex);
                return -1;
            }
        }

        [DllExport("client_send_reliable_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendReliableBool(string key, double value)
        {
            try
            {
                Client.GetInstance().ReliablePut(key, ((int)value) == 1);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_reliable_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }
    }
}
