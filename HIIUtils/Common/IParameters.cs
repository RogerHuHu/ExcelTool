using System.Collections.Generic;

namespace HIIUtils.Common
{
    internal interface IParameters : IEnumerable<KeyValuePair<string, object>>
    {
        int Count { get; }

        IEnumerable<string> Keys { get; }

        object this[string key] { get; }

        void Add(string key, object value);

        bool ContainsKey(string key);

        T GetValue<T>(string key);

        IEnumerable<T> GetValues<T>(string key);

        bool TryGetValue<T>(string key, out T value);
    }
}