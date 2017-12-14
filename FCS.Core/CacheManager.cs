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

        private readonly ICacheService cacheService;
        private readonly string uniqueNameOfDirectory;

        private string scopeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager"/> class.
        /// </summary>
        /// <param name="scopeName">Name of the scope in which the indexing should save the files.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CacheManager(string scopeName, ICacheService cacheService)
        {
            if (string.IsNullOrWhiteSpace(scopeName))
            {
                throw new ArgumentNullException(string.Concat($"{nameof(scopeName)}", GlobalConstants.ExceptionMessageShouldNotBeNull));
            }

            this.scopeName = scopeName;

            string sortableData = GetNameForDirectory();

            this.uniqueNameOfDirectory = string.Concat(this.scopeName, "_", sortableData);

            this.cacheService = cacheService;
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
            string name = string.Concat(this.uniqueNameOfDirectory, "\\", itemName);

            return this.cacheService.Get(name, getDataFunc, durationInSeconds);
        }

        public void Dispose()
        {
            try
            {
                Logger.Info("Start archiving...");
                string currentDirectory = GetDirectoryLocation();

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
                .Where(d => d.Contains(this.scopeName))
                .OrderBy(fn => fn)
                .ToList();

            return entyties;
        }

        private List<string> GetZipFileEntyties(string currentDirectory)
        {
            var entyties = Directory
                .GetFiles(currentDirectory)
                .Where(d => d.Contains(this.scopeName))
                .Where(d => d.Contains(ZipFileExtension))
                .OrderBy(fn => fn)
                .ToList();

            return entyties;
        }

        private static string GetDirectoryLocation()
        {
            return Directory.GetCurrentDirectory();
        }

        private static string GetNameForDirectory()
        {
            return string.Concat(GlobalConstants.ReindexStart.Year.ToString(),
                            GlobalConstants.ReindexStart.Month.ToString("00"),
                            GlobalConstants.ReindexStart.Day.ToString("00"));
        }
    }
}
