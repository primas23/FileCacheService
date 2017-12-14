using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

using FCS.Common;
using FCS.Contracts;

namespace FCS.Core
{
    public class CacheManager : IDisposable
    {
        private const byte backupDataLimit = 3;
        private const string ZipFileExtension = ".zip";

        private readonly ICacheService _cacheService;
        private readonly IDirectoryOperationsProvider _directoryOperationsProvider;
        
        private string _scopeName;

        public CacheManager(ICacheService cacheService, IDirectoryOperationsProvider directoryOperationsProvider)
        {   
            this._cacheService = cacheService;
            this._directoryOperationsProvider = directoryOperationsProvider;
        }

        /// <summary>
        /// Name of the scope in which the indexing should save the files.
        /// </summary>
        /// <value>
        /// The name of the scope.
        /// </value>
        public string ScopeName
        {
            get
            {
                return this._scopeName;
            }

            set
            {
                this._scopeName = value;
            }
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
            var dirName = this.GetNameOfDirectory();
            this.CreatDirectoryIfNotExists(dirName);

            var name = string.Concat(dirName, "\\", itemName);

            return this._cacheService.Get(name, getDataFunc, durationInSeconds);
        }

        public void Dispose()
        {
            try
            {
                Logger.Info("Start archiving...");
                string currentDirectory = this.GetDirectoryLocation();

                this.DisposeDirectory(currentDirectory);
                this.DisposeZipFiles(currentDirectory);

                Logger.Info("Archiving is completed.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private string GetNameOfDirectory()
        {            
            var sortableData = this.GetSortedNameForDirectory();            
            var nameOfDirectory = string.Concat(this._scopeName, "_", sortableData);

            return nameOfDirectory;
        }

        /// <summary>
        /// Creats the directory if not exists.
        /// </summary>
        /// <param name="uniqueNameOfDirectory">The unique name of directory.</param>
        private void CreatDirectoryIfNotExists(string uniqueNameOfDirectory)
        {
            if (!this._directoryOperationsProvider.Exists(uniqueNameOfDirectory))
            {
                this._directoryOperationsProvider.CreateDirectory(uniqueNameOfDirectory);
            }
        }

        private void DisposeZipFiles(string currentDirectory)
        {
            List<string> zipFilesEntyties = this.GetZipFileEntyties(currentDirectory);
            var countToDeleteFiles = zipFilesEntyties.Count - backupDataLimit;

            if (countToDeleteFiles > 0)
            {
                this.DeleteFiles(zipFilesEntyties, countToDeleteFiles);
            }
        }

        private void DisposeDirectory(string currentDirectory)
        {
            List<string> fileEntyties = this.GetFileEntyties(currentDirectory);
            var countToArchive = fileEntyties.Count - backupDataLimit;

            if (countToArchive > 0)
            {
                this.DeleteDirectories(fileEntyties, countToArchive);
            }
        }

        private void DeleteDirectories(List<string> fileEntyties, int countToArchive)
        {
            var allToBeArchived = fileEntyties.Take(countToArchive);

            // TODO: Refactor try
            foreach (var itemForArchive in allToBeArchived)
            {
                try
                {
                    Logger.Info($"Archiving {itemForArchive}");
                    ZipFile.CreateFromDirectory(itemForArchive, string.Concat(itemForArchive, ZipFileExtension));

                    try
                    {
                        Logger.Info($"Deleting {itemForArchive}");
                        Directory.Delete(itemForArchive, true);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e);
                        continue;
                    }

                }
                catch (IOException e)
                {
                    try
                    {
                        Logger.Info($"Deleting {itemForArchive}");
                        Directory.Delete(itemForArchive, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    continue;
                }
            }
        }

        private void DeleteFiles(List<string> zipFilesEntyties, int countToDeleteFiles)
        {
            var allToBeArchived = zipFilesEntyties.Take(countToDeleteFiles);

            foreach (var itemForDeleting in allToBeArchived)
            {
                try
                {
                    Console.WriteLine($"Deleting {itemForDeleting}");
                    File.Delete(itemForDeleting);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    continue;
                }
            }
        }

        private List<string> GetFileEntyties(string currentDirectory)
        {
            var entyties = Directory
                .GetDirectories(currentDirectory)
                .Where(d => d.Contains(this._scopeName))
                .OrderBy(fn => fn)
                .ToList();

            return entyties;
        }

        private List<string> GetZipFileEntyties(string currentDirectory)
        {
            var entyties = Directory
                .GetFiles(currentDirectory)
                .Where(d => d.Contains(this._scopeName))
                .Where(d => d.Contains(ZipFileExtension))
                .OrderBy(fn => fn)
                .ToList();

            return entyties;
        }

        private string GetDirectoryLocation()
        {
            return Directory.GetCurrentDirectory();
        }

        private string GetSortedNameForDirectory()
        {
            return string.Concat(GlobalConstants.ReindexStart.Year.ToString(),
                            GlobalConstants.ReindexStart.Month.ToString("00"),
                            GlobalConstants.ReindexStart.Day.ToString("00"));
        }
    }
}
