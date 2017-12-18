using System.IO;
using System.Text;

using FCS.Contracts;

namespace FCS.Utils
{
    public class FileWrite : IFileWrite
    {
        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="text">The text.</param>
        public void WriteToFile(string fullPath, string text)
        {
            using (FileStream file = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
            {
                writer.Write(text);
            }
        }
    }
}
