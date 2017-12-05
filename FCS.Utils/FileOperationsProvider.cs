using System;
using System.IO;

using FCS.Contracts;

namespace FCS.Utils
{
    public class FileOperationsProvider : IFileOperationsProvider
    {
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        public void DeleteFile(string fullPath)
        {
            File.Delete(fullPath);
        }

        /// <summary>
        /// Existses the specified full path.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>
        /// If the file exists.
        /// </returns>
        public bool Exists(string fullPath)
        {
            return File.Exists(fullPath);
        }

        /// <summary>
        /// Gets the created date time of the file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>
        /// Datetime instance.
        /// </returns>
        public DateTime GetCreationTime(string fullPath)
        {
            return File.GetCreationTime(fullPath);
        }
    }
}
