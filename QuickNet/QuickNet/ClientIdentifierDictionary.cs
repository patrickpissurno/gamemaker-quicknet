using LiteNetLib.Utils;
using System.Collections.Generic;

namespace QuickNet
{
    internal class ClientIdentifierDictionary
    {
        private Dict dict = new Dict();

        public AppendOnlyDictionary<uint> GetDictionary() => dict;

        public List<string> GetKeys() => dict.KeysList;

        public void DeserializeKeys(NetDataReader reader)
        {
            var offset = reader.GetInt();
            var length = reader.GetInt();

            for (var i = 0; i < length; i++)
            {
                //var key = reader.GetString();
                var keyAsBytes = new byte[reader.GetByte() + 1];
                reader.GetBytes(keyAsBytes, keyAsBytes.Length);
                var key = SubASCIIStringEncoder.GetString(keyAsBytes);

                var index = offset + i;
                while (index >= dict.KeysList.Count)
                    dict.KeysList.Add(null);

                if (dict.KeysList[index] == null)
                {
                    dict.KeysList[index] = key;
                    dict[key] = (uint)index;
                }
            }
        }

        private class Dict : AppendOnlyDictionary<uint>
        {
            public List<string> KeysList { get; private set; } = new List<string>();

            public override uint this[string key]
            {
                get => Values[Keys[key]];
                set
                {
                    if (!Keys.ContainsKey(key))
                    {
                        Values.Add(value);
                        Keys[key] = Count++;
                    }
                }
            }
        }
    }
}
