using System;
using System.IO;
using System.Text;

using FCS.Contracts;

namespace FCS.Core
{
    public class FileCache : ICacheService
    {
        private const string FileExtension = ".json";

        private string _defaultFilePath = Directory.GetCurrentDirectory();
        private string _filePath;

        private static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCache"/> class.
        /// </summary>
        public FileCache()
        {
            this._filePath = _defaultFilePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCache"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public FileCache(string filePath)
        {
            this._filePath = string.IsNullOrWhiteSpace(filePath) ? _defaultFilePath : filePath;
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
            string fullPath = string.Concat(this._defaultFilePath, "\\", itemName, FileExtension);

            if (File.Exists(fullPath))
            {

            }

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

        private void WriteToFile(StringBuilder text, string fileName)
        {
            lock (locker)
            {
                using (FileStream file = new FileStream(string.Concat(this._filePath, fileName, FileExtension), FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.Write(text.ToString());
                }
            }
        }

        private string ReadFromFile(string fileName)
        {
            string data;

            lock (locker)
            {
                using (FileStream file = new FileStream(string.Concat(this._filePath, fileName, FileExtension), FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamReader reader = new StreamReader(file, Encoding.Unicode))
                {
                    data = reader.ReadToEnd();
                }
            }

            return data;
        }
    }
}
