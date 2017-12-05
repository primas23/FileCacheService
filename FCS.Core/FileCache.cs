using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using FCS.Contracts;

namespace FCS.Core
{
    public class FileCache : ICacheService
    {
        private const string FileExtension = ".json";
        private const string FileCacheDirectoryName = "FileCache";

        private string _defaultFilePath = Directory.GetCurrentDirectory();
        private string _filePath;

        private static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCache"/> class.
        /// </summary>
        public FileCache()
        {
            this._filePath = this._defaultFilePath;
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
        /// Gets or sets the date time now.
        /// </summary>
        /// <value>
        /// The date time now.
        /// </value>
        public DateTime? DateTimeNow { get; set; }

        /// <summary>
        /// Gets the value of the specified item name if exists or inserts it in the cache.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="getDataFunc">The get data function.</param>
        /// <param name="durationInSeconds">The duration in seconds.</param>
        /// <returns>
        /// The cached item value.
        /// </returns>
        public T Get<T>(string itemName, Func<T> getDataFunc, int durationInSeconds)
        {
            string fullPath = this.GetFullPath(itemName);
            string fileContent = string.Empty;
            T data;

            if (this.CheckIfFileExists(fullPath))
            {
                if (this.CheckIfExpired(fullPath, durationInSeconds))
                {
                    this.DeleteFile(fullPath);
                    data = this.SaveDataToFile(getDataFunc, fullPath);
                }
                else
                {
                    fileContent = this.ReadFromFile(fullPath);
                    data = JsonConvert.DeserializeObject<T>(fileContent);
                }
            }
            else
            {
                data = this.SaveDataToFile(getDataFunc, fullPath);
            }

            return data;
        }

        /// <summary>
        /// Creats the directory if not exists.
        /// </summary>
        private void CreatDirectoryIfNotExists()
        {
            string fileDirectoryPath = this.GetDirectoryLocation();

            if (!Directory.Exists(fileDirectoryPath))
            {
                Directory.CreateDirectory(fileDirectoryPath);
            }
        }

        /// <summary>
        /// Checks if expired.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="durationInSeconds">The duration in seconds.</param>
        /// <returns>If the file is expired.</returns>
        private bool CheckIfExpired(string fullPath, int durationInSeconds)
        {
            bool isExpired = false;
            DateTime createdOn = this.GetCreatedDate(fullPath);
            DateTime dateTimeNow = this.GetDateTimeNow();

            if (createdOn.AddSeconds(durationInSeconds) < dateTimeNow)
            {
                isExpired = true;
            }

            return isExpired;
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        private void DeleteFile(string fullPath)
        {
            File.Delete(fullPath);
        }

        /// <summary>
        /// Saves the data to file.
        /// </summary>
        /// <typeparam name="T">The template class</typeparam>
        /// <param name="getDataFunc">The get data function.</param>
        /// <param name="fullPath">The full path.</param>
        /// <returns>The parsed data.</returns>
        private T SaveDataToFile<T>(Func<T> getDataFunc, string fullPath)
        {
            this.CreatDirectoryIfNotExists();
            T data = getDataFunc();
            string fileContent = JsonConvert.SerializeObject(data);
            this.WriteToFile(fullPath, fileContent);

            return data;
        }

        /// <summary>
        /// Gets the date time now.
        /// </summary>
        /// <returns>Returns either the provide datetime property or the current datetime now.</returns>
        private DateTime GetDateTimeNow()
        {
            return this.DateTimeNow ?? DateTime.Now;
        }

        /// <summary>
        /// Gets the created date.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>The created datetime of the file.</returns>
        private DateTime GetCreatedDate(string fullPath)
        {
            return File.GetCreationTime(fullPath);
        }

        /// <summary>
        /// Checks if file exists.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>If the file exists.</returns>
        private bool CheckIfFileExists(string fullPath)
        {
            bool isExisting = false;

            if (Directory.Exists(this.GetDirectoryLocation()))
            {
                isExisting = File.Exists(fullPath);
            }

            return isExisting;
        }

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <returns>The full path.</returns>
        private string GetFullPath(string itemName)
        {
            return string.Concat(this.GetDirectoryLocation(), "\\", itemName, FileExtension);
        }

        /// <summary>
        /// Gets the directory location.
        /// </summary>
        /// <returns>Returns the path to the file directory</returns>
        private string GetDirectoryLocation()
        {
            return string.Concat(this._defaultFilePath, "\\", FileCacheDirectoryName);
        }

        /// <summary>
        /// Removes the specified item name.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        public void Remove(string itemName)
        {
            string fullPath = this.GetFullPath(itemName);
            this.DeleteFile(fullPath);
        }

        /// <summary>
        /// Clears the all the cache.
        /// </summary>
        /// 
        public void Clear()
        {
            Directory.Delete(this.GetDirectoryLocation());
        }

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="text">The text.</param>
        private void WriteToFile(string fullPath, string text)
        {
            lock (locker)
            {
                using (FileStream file = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.Write(text);
                }
            }
        }

        /// <summary>
        /// Reads from file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>The content of the file.</returns>
        private string ReadFromFile(string fullPath)
        {
            string data;

            lock (locker)
            {
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    data = reader.ReadToEnd();
                }
            }

            return data;
        }
    }
}
