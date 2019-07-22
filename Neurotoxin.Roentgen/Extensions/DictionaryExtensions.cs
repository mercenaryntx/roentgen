using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurotoxin.Roentgen.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key)) return false;
            dict.Add(key, value);
            return true;
        }

        public static void TryAdd<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dict, TKey key, TValue value)
        {
            if (dict.TryAdd(key, new List<TValue> { value })) return;
            dict[key].Add(value);
        }
    }
}