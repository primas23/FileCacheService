using System;

namespace FCS.Contracts
{
    public interface ICacheService
    {
        /// <summary>
        /// Gets the value of the specified item name if exists or inserts it in the cache.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="getDataFunc">The get data function.</param>
        /// <param name="durationInSeconds">The duration in seconds.</param>
        /// <returns>The cached item value</returns>
        T Get<T>(string itemName, Func<T> getDataFunc, int durationInSeconds);

        /// <summary>
        /// Removes the specified item name.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        void Remove(string itemName);

        /// <summary>
        /// Clears the all the cache.
        /// </summary>
        void Clear();
    }
}