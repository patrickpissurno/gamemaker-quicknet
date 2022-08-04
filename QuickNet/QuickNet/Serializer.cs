using LiteNetLib.Utils;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QuickNet
{
    internal static class Serializer
    {
        // returns true if mixed mode was used, false otherwise
        public static void SerializeData(NetDataWriter writer, AppendOnlyDictionary<uint> dict, (string key, object data) t, bool mixedMode = false)
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

            if (mixedMode)
            {
                var contains = dict.Keys.ContainsKey(t.key);

                writer.Put((byte)(contains ? 0 : 1)); // 0 => uint, 1 => string

                if (contains)
                {
                    writer.Put(dict[t.key]);
                }
                else
                {
                    var key = SubASCIIStringEncoder.GetBytes(t.key);
                    writer.Put((byte)(key.Length - 1));
                    writer.Put(key);
                }
            }
            else
            {
                writer.Put(dict[t.key]);
            }
        }

        public static (uint id, string key, string data)? DeserializeData(NetDataReader reader, List<string> keys, bool mixedMode = false)
        {
            var data = DeserializeValue(reader);

            uint id = uint.MaxValue;
            string key;

            if(mixedMode)
            {
                var mixedType = reader.GetByte();
                if(mixedType == 0)
                {
                    id = reader.GetUInt();
                    if (id >= keys.Count)
                        key = null;
                    else
                        key = keys[(int)id];
                }
                else
                {
                    var keyAsBytes = new byte[reader.GetByte() + 1];
                    reader.GetBytes(keyAsBytes, keyAsBytes.Length);
                    key = SubASCIIStringEncoder.GetString(keyAsBytes);
                }
            }
            else
            {
                id = reader.GetUInt();
                if (id >= keys.Count)
                    key = null;
                else
                    key = keys[(int)id];
            }

            if (data == null)
                return null;

            return (id, key, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string DeserializeValue(NetDataReader reader)
        {
            var type = (DataType)reader.GetByte();

            switch (type)
            {
                case DataType.STRING:
                    return reader.GetString();
                case DataType.INT:
                    return reader.GetInt().ToString();
                case DataType.DOUBLE:
                    return reader.GetDouble().ToString();
                case DataType.BOOL:
                    return (reader.GetBool() ? 1 : 0).ToString();
                case DataType.ARRAY_STRING:
                    return Utils.EncodeArray(reader.GetStringArray());
                case DataType.ARRAY_INT:
                    return Utils.EncodeArray(reader.GetIntArray());
                case DataType.ARRAY_DOUBLE:
                    return Utils.EncodeArray(reader.GetDoubleArray());
                case DataType.ARRAY_BOOL:
                    return Utils.EncodeArray(reader.GetBoolArray());
                default:
                    return null;
            }
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
