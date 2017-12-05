using System.IO;

using FCS.Contracts;

namespace FCS.Utils
{
    public class FileRead : IFileRead
    {
        /// <summary>
        /// Reads from file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>The content of the file.</returns>
        public string ReadFromFile(string fullPath)
        {
            string data;

            using (StreamReader reader = new StreamReader(fullPath))
            {
                data = reader.ReadToEnd();
            }

            return data;
        }
    }
}
