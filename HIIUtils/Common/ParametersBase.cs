using HIIUtils.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HIIUtils.Common
{
    public abstract class ParametersBase : IParameters, IEnumerable<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> _parameters = new List<KeyValuePair<string, object>>();

        protected ParametersBase()
        {
        }

        protected ParametersBase(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                int num = query.Length;
                for (int i = ((query.Length > 0) && (query[0] == '?')) ? 1 : 0; i < num; i++)
                {
                    int startIndex = i;
                    int num4 = -1;
                    while (i < num)
                    {
                        char ch = query[i];
                        if (ch == '=')
                        {
                            if (num4 < 0)
                                num4 = i;
                        }
                        else if (ch == '&')
                        {
                            break;
                        }
                        i++;
                    }

                    string key = null;
                    string value;
                    if (num4 >= 0)
                    {
                        key = query.Substring(startIndex, num4 - startIndex);
                        value = query.Substring(num4 + 1, (i - num4) - 1);
                    }
                    else
                    {
                        value = query.Substring(startIndex, i - startIndex);
                    }

                    if (key != null)
                        Add(Uri.UnescapeDataString(key), Uri.UnescapeDataString(value));
                }
            }
        }

        public int Count => _parameters.Count;

        public IEnumerable<string> Keys => _parameters.Select(x => x.Key);

        public object this[string key]
        {
            get
            {
                foreach (var entry in _parameters)
                {
                    if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                        return entry.Value;
                }

                return null;
            }
        }

        public void Add(string key, object value) =>
            _parameters.Add(new KeyValuePair<string, object>(key, value));

        public bool ContainsKey(string key) => _parameters.ContainsKey(key);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void FromParameters(IEnumerable<KeyValuePair<string, object>> parameters) =>
            _parameters.AddRange(parameters);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _parameters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T GetValue<T>(string key) => _parameters.GetValue<T>(key);

        public IEnumerable<T> GetValues<T>(string key) => _parameters.GetValues<T>(key);

        public override string ToString()
        {
            var queryBuilder = new StringBuilder();

            if (_parameters.Count > 0)
            {
                queryBuilder.Append('?');
                var first = true;

                foreach (var kvp in _parameters)
                {
                    if (!first)
                        queryBuilder.Append('&');
                    else
                        first = false;

                    queryBuilder.Append(Uri.EscapeDataString(kvp.Key));
                    queryBuilder.Append('=');
                    queryBuilder.Append(Uri.EscapeDataString(kvp.Value != null ? kvp.Value.ToString() : ""));
                }
            }

            return queryBuilder.ToString();
        }

        public bool TryGetValue<T>(string key, out T value) => _parameters.TryGetValue(key, out value);
    }
}