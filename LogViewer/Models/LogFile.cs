using GalaSoft.MvvmLight;
using LogViewer.Infrastructure;
using System.Collections.ObjectModel;
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
        /// All information about file
        /// </summary>
        public FileInfo FileInfo { get; set; }


        public string Content { get; set; }

        public async Task ReadFileContentAsync(bool bIsupdate = true)
        {
            await CommandHelper.RunCommandAsync(() => IsBusy, async () =>
             {
                 if (File.Exists(FilePath) && bIsupdate)
                 {
                     Content = await FileUtil.ReadAllTextAsync(FilePath);
                 }
             }, string.IsNullOrEmpty(Content) ? 1000 : 0);
        }

        public bool IsBusy { get; set; } = false;

    }
}