using System;

using FCS.Contracts;

namespace FCS.Core
{
    public class FileCache : ICacheService
    {
        public T Get<T>(string itemName, Func<T> getDataFunc, int durationInSeconds)
        {
            throw new NotImplementedException();
        }

        public void Remove(string itemName)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
