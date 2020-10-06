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
            else
                return;

            writer.Put(t.key); //TODO: optimize the size needed to transmit the key
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
                default:
                    return null;
            }

            var key = reader.GetString(); //TODO: optimize the size needed to transmit the key

            return (key, data);
        }
    }
}
