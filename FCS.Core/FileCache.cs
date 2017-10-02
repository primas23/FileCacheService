using System;
using System.IO;
using System.Text;
using FCS.Contracts;

namespace FCS.Core
{
    public class FileCache : ICacheService
    {
        private const string DefaultFileName = "cacheFileName";
        private const string DefaultFilePath = "cacheFilePath";
        private const string FileExtension = ".cache";

        private string _fileName;
        private string _filePath;

        private static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCache"/> class.
        /// </summary>
        public FileCache()
        {
            this._fileName = DefaultFileName;
            this._filePath = DefaultFilePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCache"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filePath">The file path.</param>
        public FileCache(string fileName, string filePath)
        {
            this._fileName = string.IsNullOrWhiteSpace(fileName) ? DefaultFileName : fileName;
            this._filePath = string.IsNullOrWhiteSpace(filePath) ? DefaultFilePath : filePath;
        }

        /// <summary>
        /// Gets the value of the specified item name if exists or inserts it in the cache.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="getDataFunc">The get data function.</param>
        /// <param name="durationInSeconds">The duration in seconds.</param>
        /// <returns>
        /// The cached item value
        /// </returns>
        public T Get<T>(string itemName, Func<T> getDataFunc, int durationInSeconds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the specified item name.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        public void Remove(string itemName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears the all the cache.
        /// </summary>
        /// 
        public void Clear()
        {
            throw new NotImplementedException();
        }

        private void WriteToFile(StringBuilder text)
        {
            lock (locker)
            {
                using (FileStream file = new FileStream(string.Concat(this._filePath, this._fileName, FileExtension), FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.Write(text.ToString());
                }
            }
        }
    }
}
