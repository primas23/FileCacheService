using System.IO;

namespace FCS.Contracts
{
    public interface IDirectoryOperationsProvider
    {
        /// <summary>
        /// Existses the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>If the directory exists.</returns>
        bool Exists(string path);

        /// <summary>
        /// Deletes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        void Delete(string path);

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Information abount the directory.</returns>
        DirectoryInfo CreateDirectory(string path);

        /// <summary>
        /// Gets the current directory.
        /// </summary>
        /// <returns>the location of the executing file.</returns>
        string GetCurrentDirectory();
    }
}
