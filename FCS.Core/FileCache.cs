using System;

using FCS.Contracts;
using FCS.Common;

namespace FCS.Core
{
    public class FileCache : ICacheService
    {
        private string _filePath;

        private static object locker = new object();

        private readonly IFileRead fileRead;
        private readonly IFileWrite fileWrite;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IJosnService josnService;
        private readonly IFileOperationsProvider fileOperationsProvider;
        private readonly IDirectoryOperationsProvider directoryOperationsProvider;

        public FileCache(IFileRead fileRead,
            IFileWrite fileWrite,
            IDateTimeProvider dateTimeProvider,
            IJosnService josnService,
            IFileOperationsProvider fileOperationsProvider,
            IDirectoryOperationsProvider directoryOperationsProvider)
        {
            this.fileRead = fileRead;
            this.fileWrite = fileWrite;
            this.dateTimeProvider = dateTimeProvider;
            this.josnService = josnService;
            this.fileOperationsProvider = fileOperationsProvider;
            this.directoryOperationsProvider = directoryOperationsProvider;
            this._filePath = this.directoryOperationsProvider.GetCurrentDirectory();
        }

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
                    data = this.josnService.DeserializeObject<T>(fileContent);
                }
            }
            else
            {
                data = this.SaveDataToFile(getDataFunc, fullPath);
            }

            return data;
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
        public void Clear()
        {
            directoryOperationsProvider.Delete(this.GetDirectoryLocation());
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
                this.fileWrite.WriteToFile(fullPath, text);
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
                data = this.fileRead.ReadFromFile(fullPath);
            }

            return data;
        }

        /// <summary>
        /// Creats the directory if not exists.
        /// </summary>
        private void CreatDirectoryIfNotExists()
        {
            string fileDirectoryPath = this.GetDirectoryLocation();

            if (!this.directoryOperationsProvider.Exists(fileDirectoryPath))
            {
                this.directoryOperationsProvider.CreateDirectory(fileDirectoryPath);
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
            this.fileOperationsProvider.DeleteFile(fullPath);
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
            string fileContent = this.josnService.SerializeObject(data);
            this.WriteToFile(fullPath, fileContent);

            return data;
        }

        /// <summary>
        /// Gets the date time now.
        /// </summary>
        /// <returns>Returns either the provide datetime property or the current datetime now.</returns>
        private DateTime GetDateTimeNow()
        {
            return this.dateTimeProvider.GetDateTimeNow();
        }

        /// <summary>
        /// Gets the created date.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>The created datetime of the file.</returns>
        private DateTime GetCreatedDate(string fullPath)
        {
            return this.fileOperationsProvider.GetCreationTime(fullPath);
        }

        /// <summary>
        /// Checks if file exists.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>If the file exists.</returns>
        private bool CheckIfFileExists(string fullPath)
        {
            bool isExisting = false;

            if (this.directoryOperationsProvider.Exists(this.GetDirectoryLocation()))
            {
                isExisting = this.fileOperationsProvider.Exists(fullPath);
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
            return string.Concat(this.GetDirectoryLocation(), "\\", itemName, GlobalConstants.JsonFileExtension);
        }

        /// <summary>
        /// Gets the directory location.
        /// </summary>
        /// <returns>Returns the path to the file directory</returns>
        private string GetDirectoryLocation()
        {
            return string.Concat(this._filePath, "\\", GlobalConstants.FileCacheDirectoryName);
        }
    }
}
