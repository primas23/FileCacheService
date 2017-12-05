namespace FCS.Contracts
{
    public interface IFileRead
    {
        /// <summary>
        /// Reads from file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>The content of the file.</returns>
        string ReadFromFile(string fullPath);
    }
}
