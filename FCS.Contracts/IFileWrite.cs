namespace FCS.Contracts
{
    public interface IFileWrite
    {
        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="text">The text.</param>
        void WriteToFile(string fullPath, string text);
    }
}
