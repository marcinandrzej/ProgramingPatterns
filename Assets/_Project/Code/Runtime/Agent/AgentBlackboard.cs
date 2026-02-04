using System.Collections.Generic;

namespace Runtime.Agent
{
    public class AgentBlackboard
    {
        private Dictionary<string, object> data = new Dictionary<string, object>(50);

        public void Set<T>(string key, T value)
        {
            if (data.ContainsKey(key))
                data[key] = value;
            else
                data.Add(key, value);
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (data.TryGetValue(key, out object storedValue) && storedValue is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;

            return false;
        }

        public T Get<T>(string key) => data.TryGetValue(key, out object value) ? (T)value : default;

        public bool Has(string key) => data.ContainsKey(key);

        public void Remove(string key) => data.Remove(key);

        public void Clear() => data.Clear();
    }
}