using GalaSoft.MvvmLight;
using System.IO;
using System.Threading.Tasks;

namespace LogViewer.Models
{
    /// <summary>
    /// This is a simple log file
    /// </summary>
    public class LogFile : ViewModelBase
    {
        /// <summary>
        /// Path to file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// All inforramtion about file
        /// </summary>
        public FileInfo FileInfo { get; set; }


        public string Content { get; set; }

        public async Task ReadFileContentAsync()
        {

            if (File.Exists(FilePath))
            {
                Content = await FileUtil.ReadAllTextAsync(FilePath);
            }
        }
    }
}
