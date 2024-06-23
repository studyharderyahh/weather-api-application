using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.FileHandlers
{
    /// <summary>
    /// Represents a generic dictionary wrapper providing additional functionalities.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public class m_APIDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> m_internalDictionary = new Dictionary<TKey, TValue>();

        /// <summary>
        /// Adds a key-value pair to the dictionary.
        /// </summary>
        /// <param name="key">The key of the pair to add.</param>
        /// <param name="value">The value of the pair to add.</param>
        public void Add(TKey key, TValue value)
        {
            m_internalDictionary.Add(key, value);
        }

        /// <summary>
        /// Tries to get the value associated with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key,
        /// if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return m_internalDictionary.TryGetValue(key, out value);
        }

        // We will implement more features if needed in the future.
    }

}
