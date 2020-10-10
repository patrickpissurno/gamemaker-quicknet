using System;
using System.Runtime.InteropServices;

namespace QuickNet
{
    public static class Main
    {
        #region Server functions

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

        #region Send Reliable

        #region Primitive Types

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

        #endregion

        #region Array Types

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

        #endregion

        #endregion
        
        #region Send Reliable To

        #region Primitive Types

        [DllExport("server_send_reliable_to", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableTo(double destination_id, string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePutTo((int)destination_id, key, value);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_to' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_reliable_to_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableToFloat(double destination_id, string key, double value)
        {
            try
            {
                Server.GetInstance().ReliablePutTo((int)destination_id, key, value);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_to_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_reliable_to_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableToInt(double destination_id, string key, double value)
        {
            try
            {
                Server.GetInstance().ReliablePutTo((int)destination_id, key, (int)value);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_to_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }
        
        [DllExport("server_send_reliable_to_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableToBool(double destination_id, string key, double value)
        {
            try
            {
                Server.GetInstance().ReliablePutTo((int)destination_id, key, ((int)value) == 1);
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_to_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        #endregion

        #region Array Types

        [DllExport("server_send_reliable_to_array", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableToArray(double destination_id, string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePutTo((int)destination_id, key, Utils.DecodeArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_to_array' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_reliable_to_array_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableToArrayFloat(double destination_id, string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePutTo((int)destination_id, key, Utils.DecodeDoubleArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_to_array_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }
        
        [DllExport("server_send_reliable_to_array_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableArrayInt(double destination_id, string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePutTo((int)destination_id, key, Utils.DecodeIntArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_to_array_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_reliable_to_array_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendReliableToArrayBool(double destination_id, string key, string value)
        {
            try
            {
                Server.GetInstance().ReliablePutTo((int)destination_id, key, Utils.DecodeBoolArray(value));
                return 0;
            }
            catch(Exception ex)
            {
                Utils.Log("Function 'server_send_reliable_to_array_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        #endregion

        #endregion

        #region Send Unreliable

        #region Primitive Types

        [DllExport("server_send_unreliable", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendUnreliable(string key, string value)
        {
            try
            {
                Server.GetInstance().UnreliablePut(key, value);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'server_send_unreliable' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_unreliable_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendUnreliableFloat(string key, double value)
        {
            try
            {
                Server.GetInstance().UnreliablePut(key, value);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'server_send_unreliable_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_unreliable_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendUnreliableInt(string key, double value)
        {
            try
            {
                Server.GetInstance().UnreliablePut(key, (int)value);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'server_send_unreliable_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_unreliable_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendUnreliableBool(string key, double value)
        {
            try
            {
                Server.GetInstance().UnreliablePut(key, ((int)value) == 1);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'server_send_unreliable_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        #endregion

        #region Array Types

        [DllExport("server_send_unreliable_array", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendUnreliableArray(string key, string value)
        {
            try
            {
                Server.GetInstance().UnreliablePut(key, Utils.DecodeArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'server_send_unreliable_array' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_unreliable_array_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendUnreliableArrayFloat(string key, string value)
        {
            try
            {
                Server.GetInstance().UnreliablePut(key, Utils.DecodeDoubleArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'server_send_unreliable_array_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_unreliable_array_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendUnreliableArrayInt(string key, string value)
        {
            try
            {
                Server.GetInstance().UnreliablePut(key, Utils.DecodeIntArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'server_send_unreliable_array_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("server_send_unreliable_array_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ServerSendUnreliableArrayBool(string key, string value)
        {
            try
            {
                Server.GetInstance().UnreliablePut(key, Utils.DecodeBoolArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'server_send_unreliable_array_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        #endregion

        #endregion


        #endregion

        #region Client functions

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

        [DllExport("client_get_latency", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientGetLatency()
        {
            try
            {
                return Client.GetInstance().GetLatency();
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_get_latency' has thrown an exception. Stack:");
                Utils.Log(ex);
                return -1;
            }
        }

        #region Send Reliable

        #region Primitive Types

        [DllExport("client_send_reliable", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendReliable(string key, string value)
        {
            try
            {
                Client.GetInstance().ReliablePut(key, value);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_reliable' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_reliable_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendReliableFloat(string key, double value)
        {
            try
            {
                Client.GetInstance().ReliablePut(key, value);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_reliable_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_reliable_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendReliableInt(string key, double value)
        {
            try
            {
                Client.GetInstance().ReliablePut(key, (int)value);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_reliable_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
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

        #endregion

        #region Array Types


        [DllExport("client_send_reliable_array", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendReliableArray(string key, string value)
        {
            try
            {
                Client.GetInstance().ReliablePut(key, Utils.DecodeArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_reliable_array' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_reliable_array_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendReliableArrayFloat(string key, string value)
        {
            try
            {
                Client.GetInstance().ReliablePut(key, Utils.DecodeDoubleArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_reliable_array_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_reliable_array_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendReliableArrayInt(string key, string value)
        {
            try
            {
                Client.GetInstance().ReliablePut(key, Utils.DecodeIntArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_reliable_array_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_reliable_array_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendReliableArrayBool(string key, string value)
        {
            try
            {
                Client.GetInstance().ReliablePut(key, Utils.DecodeBoolArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_reliable_array_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        #endregion

        #endregion
        
        #region Send Unreliable

        #region Primitive Types

        [DllExport("client_send_unreliable", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendUnreliable(string key, string value)
        {
            try
            {
                Client.GetInstance().UnreliablePut(key, value);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_unreliable' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_unreliable_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendUnreliableFloat(string key, double value)
        {
            try
            {
                Client.GetInstance().UnreliablePut(key, value);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_unreliable_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_unreliable_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendUnreliableInt(string key, double value)
        {
            try
            {
                Client.GetInstance().UnreliablePut(key, (int)value);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_unreliable_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_unreliable_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendUnreliableBool(string key, double value)
        {
            try
            {
                Client.GetInstance().UnreliablePut(key, ((int)value) == 1);
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_unreliable_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        #endregion

        #region Array Types


        [DllExport("client_send_unreliable_array", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendUnreliableArray(string key, string value)
        {
            try
            {
                Client.GetInstance().UnreliablePut(key, Utils.DecodeArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_unreliable_array' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_unreliable_array_float", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendUnreliableArrayFloat(string key, string value)
        {
            try
            {
                Client.GetInstance().UnreliablePut(key, Utils.DecodeDoubleArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_unreliable_array_float' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_unreliable_array_int", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendUnreliableArrayInt(string key, string value)
        {
            try
            {
                Client.GetInstance().UnreliablePut(key, Utils.DecodeIntArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_unreliable_array_int' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        [DllExport("client_send_unreliable_array_bool", CallingConvention = CallingConvention.Cdecl)]
        public static double ClientSendUnreliableArrayBool(string key, string value)
        {
            try
            {
                Client.GetInstance().UnreliablePut(key, Utils.DecodeBoolArray(value));
                return 0;
            }
            catch (Exception ex)
            {
                Utils.Log("Function 'client_send_unreliable_array_bool' has thrown an exception. Stack:");
                Utils.Log(ex);
                return 1;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Utils functions
        [DllExport("utils_check_valid_ip_address", CallingConvention = CallingConvention.Cdecl)]
        public static double UtilsCheckValidIpAddress(string ip)
        {
            return Utils.IsIpAddressValid(ip) ? 1 : 0;
        }

        [DllExport("utils_check_valid_port_number", CallingConvention = CallingConvention.Cdecl)]
        public static double UtilsCheckValidPortNumber(string port)
        {
            return Utils.IsPortNumberValid(port) ? 1 : 0;
        }
        #endregion
    }
}
