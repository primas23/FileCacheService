using System;

using FCS.Contracts;
using FCS.Common;
using System.Text.RegularExpressions;

namespace FCS.Core
{
    public class FileCache : ICacheService
    {
        private const string ScopePattern = @"([A-Z])\w+";

        private string _pathToDirectory;

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
            this._pathToDirectory = this.directoryOperationsProvider.GetCurrentDirectory();
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
            var nameOfDirectoryScope = string.Empty;

            if (this.IsDirectoryProvided(itemName))
            {
                nameOfDirectoryScope = this.GetScopeName(itemName);

                var directoryPath = string.Concat(this.GetDirectoryLocation(), nameOfDirectoryScope);

                if (!this.directoryOperationsProvider.Exists(directoryPath))
                {
                    this.directoryOperationsProvider.CreateDirectory(directoryPath);
                }
            }

            var fullPath = this.GetFullPath(itemName);

            var fileContent = string.Empty;
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

        private string GetScopeName(string itemName)
        {
            string nameOfDirectoryScope;

            Regex regex = new Regex(ScopePattern);
            Match match = regex.Match(itemName);
            nameOfDirectoryScope = match.Value;

            return nameOfDirectoryScope;
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

        private bool IsDirectoryProvided(string itemName)
        {
            return itemName.Contains("\\");
        }

        private void WriteToFile(string fullPath, string text)
        {
            lock (locker)
            {
                this.fileWrite.WriteToFile(fullPath, text);
            }
        }

        private string ReadFromFile(string fullPath)
        {
            string data;

            lock (locker)
            {
                data = this.fileRead.ReadFromFile(fullPath);
            }

            return data;
        }
        
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

        private void DeleteFile(string fullPath)
        {
            this.fileOperationsProvider.DeleteFile(fullPath);
        }

        private T SaveDataToFile<T>(Func<T> getDataFunc, string fullPath)
        {
            T data = getDataFunc();
            string fileContent = this.josnService.SerializeObject(data);
            this.WriteToFile(fullPath, fileContent);

            return data;
        }

        private DateTime GetDateTimeNow()
        {
            return this.dateTimeProvider.GetDateTimeNow();
        }

        private DateTime GetCreatedDate(string fullPath)
        {
            return this.fileOperationsProvider.GetCreationTime(fullPath);
        }

        private bool CheckIfFileExists(string fullPath)
        {
            bool isExisting = false;

            isExisting = this.fileOperationsProvider.Exists(fullPath);

            return isExisting;
        }

        private string GetFullPath(string itemName)
        {
            return string.Concat(this.GetDirectoryLocation(), itemName, GlobalConstants.JsonFileExtension);
        }

        private string GetDirectoryLocation()
        {
            return string.Concat(this._pathToDirectory, "\\", GlobalConstants.FileCacheDirectoryName);
        }
    }
}
