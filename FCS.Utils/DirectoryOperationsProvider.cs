using System.IO;

using FCS.Contracts;

namespace FCS.Utils
{
    public class DirectoryOperationsProvider : IDirectoryOperationsProvider
    {
        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// Information abount the directory.
        /// </returns>
        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Deletes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Delete(string path)
        {
            Directory.Delete(path);
        }

        /// <summary>
        /// Existses the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// If the directory exists.
        /// </returns>
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Gets the current directory.
        /// </summary>
        /// <returns>
        /// the location of the executing file.
        /// </returns>
        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
