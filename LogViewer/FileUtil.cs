using LogViewer.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogViewer
{
    public static class FileUtil
    {
        public static async Task<string> ReadAllTextAsync(string filePath)
        {
            var stringBuilder = new StringBuilder();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (fileStream)
            using (var streamReader = new StreamReader(fileStream))
            {
                string line = await streamReader.ReadLineAsync();
                while (line != null)
                {
                    stringBuilder.AppendLine(line);
                    line = await streamReader.ReadLineAsync();
                }
                return stringBuilder.ToString();
            }
        }
    }
}
