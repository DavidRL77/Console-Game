using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    public class AssetRegistry
    {
        private Dictionary<string, object> registry = new Dictionary<string, object>();

        public void Register(string path, object obj)
        {
            AddOrUpdate(registry, path, obj);
        }

        public T Get<T>(string key)
        {
            return (T)registry[key];
        }

        private void AddOrUpdate<K, V>(Dictionary<K, V> dict, K key, V value)
        {
            if(dict.ContainsKey(key))
            {
                dict.Remove(key);
            }
            dict.Add(key, value);
        }

    }
}
