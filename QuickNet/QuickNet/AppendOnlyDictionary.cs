using System.Collections.Generic;

namespace QuickNet
{
    internal class AppendOnlyDictionary<T>
    {
        public Dictionary<string, int> Keys { get; protected set; } = new Dictionary<string, int>();
        public List<T> Values { get; protected set; } = new List<T>();
        public int Count { get; protected set; } = 0;

        public virtual T this[string key]
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
