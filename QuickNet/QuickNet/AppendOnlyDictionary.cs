using System.Collections.Generic;

namespace QuickNet
{
    internal class AppendOnlyDictionary<T>
    {
        public Dictionary<string, int> Keys { get; private set; } = new Dictionary<string, int>();
        public List<T> Values { get; private set; } = new List<T>();
        public int Count { get; private set; } = 0;

        public T this[string key]
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
