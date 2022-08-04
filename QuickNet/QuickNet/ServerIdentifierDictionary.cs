using LiteNetLib.Utils;
using System.Collections.Generic;

namespace QuickNet
{
    internal class ServerIdentifierDictionary
    {
        private Dict dict = new Dict();

        public AppendOnlyDictionary<uint> GetDictionary() => dict;

        public List<string> GetKeys() => dict.KeysList;

        public void Add(string identifier)
        {
            dict[identifier] = (uint)dict.Count;
        }

        public int SerializeKeysSince(NetDataWriter writer, int count)
        {
            var _count = Count();

            var added = _count - count;
            if (added < 1)
                return 0;

            writer.Put((byte)0); // indicates that this packet contains a dictionary
            writer.Put(count); // starting count
            writer.Put(added); // number of elements

            for(var i = count; i < _count; i++)
            {
                //writer.Put(dict.KeysList[i]);
                var key = SubASCIIStringEncoder.GetBytes(dict.KeysList[i]);
                writer.Put((byte)(key.Length - 1));
                writer.Put(key);
            }

            return added;
        }

        public int Count() => dict.Count;

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
                        KeysList.Add(key);
                        Values.Add(value);
                        Keys[key] = Count++;
                    }
                }
            }
        }
    }
}
