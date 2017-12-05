using System;

namespace FCS.Contracts
{
    public interface IFileOperationsProvider
    {
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        void DeleteFile(string fullPath);

        /// <summary>
        /// Gets the created date time of the file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>Datetime instance.</returns>
        DateTime GetCreationTime(string fullPath);

        /// <summary>
        /// Existses the specified full path.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>If the file exists.</returns>
        bool Exists(string fullPath);
    }
}
