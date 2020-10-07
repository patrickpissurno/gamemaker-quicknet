using LiteNetLib.Utils;

namespace QuickNet
{
    internal static class Serializer
    {
        public static void SerializeData(NetDataWriter writer, (string key, object data) t)
        {
            byte type;
            if (t.data is string _string)
            {
                type = (byte)DataType.STRING;
                writer.Put(type);
                writer.Put(_string);
            }
            else if (t.data is int _int)
            {
                type = (byte)DataType.INT;
                writer.Put(type);
                writer.Put(_int);
            }
            else if (t.data is double _double)
            {
                type = (byte)DataType.DOUBLE;
                writer.Put(type);
                writer.Put(_double);
            }
            else if (t.data is bool _bool)
            {
                type = (byte)DataType.BOOL;
                writer.Put(type);
                writer.Put(_bool);
            }
            else if (t.data is string[] _arr_string)
            {
                type = (byte)DataType.ARRAY_STRING;
                writer.Put(type);
                writer.PutArray(_arr_string);
            }
            else if (t.data is int[] _arr_int)
            {
                type = (byte)DataType.ARRAY_INT;
                writer.Put(type);
                writer.PutArray(_arr_int);
            }
            else if (t.data is double[] _arr_double)
            {
                type = (byte)DataType.ARRAY_DOUBLE;
                writer.Put(type);
                writer.PutArray(_arr_double);
            }
            else if (t.data is bool[] _arr_bool)
            {
                type = (byte)DataType.ARRAY_BOOL;
                writer.Put(type);
                writer.PutArray(_arr_bool);
            }
            else
                return;

            //TODO: optimize the size needed to transmit the key
            var key = SubASCIIStringEncoder.GetBytes(t.key);
            writer.Put((byte)(key.Length - 1));
            writer.Put(key);
        }

        public static (string key, string data)? DeserializeData(NetDataReader reader)
        {
            var type = (DataType)reader.GetByte();
            string data;

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
                case DataType.BOOL:
                    data = (reader.GetBool() ? 1 : 0).ToString();
                    break;
                case DataType.ARRAY_STRING:
                    data = Utils.EncodeArray(reader.GetStringArray());
                    break;
                case DataType.ARRAY_INT:
                    data = Utils.EncodeArray(reader.GetIntArray());
                    break;
                case DataType.ARRAY_DOUBLE:
                    data = Utils.EncodeArray(reader.GetDoubleArray());
                    break;
                case DataType.ARRAY_BOOL:
                    data = Utils.EncodeArray(reader.GetBoolArray());
                    break;
                default:
                    return null;
            }

            //TODO: optimize the size needed to transmit the key
            var keyAsBytes = new byte[reader.GetByte() + 1];
            reader.GetBytes(keyAsBytes, keyAsBytes.Length);
            var key = SubASCIIStringEncoder.GetString(keyAsBytes);

            return (key, data);
        }

        private enum DataType : byte
        {
            STRING = 0,
            INT = 1,
            DOUBLE = 2,
            BOOL = 3,
            ARRAY_STRING = 4,
            ARRAY_INT = 5,
            ARRAY_DOUBLE = 6,
            ARRAY_BOOL = 7,
        }
    }
}
