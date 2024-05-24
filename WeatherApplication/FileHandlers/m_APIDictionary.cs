using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.FileHandlers
{
    public class m_APIDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> m_internalDictionary = new Dictionary<TKey, TValue>();

        public void Add(TKey key, TValue value)
        {
            m_internalDictionary.Add(key, value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return m_internalDictionary.TryGetValue(key, out value);
        }

        // Additional methods to manipulate the dictionary...
    }

}
