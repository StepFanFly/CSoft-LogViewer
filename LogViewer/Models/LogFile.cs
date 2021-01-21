using GalaSoft.MvvmLight;
using LogViewer.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;

namespace LogViewer.Models
{

    //public class SearhField
    //{
    //    //public Dictionary<int, string> SplittedContent { get; set; } = new Dictionary<int, string>();
    //    //public ObservableCollection<Filter> Filters { get; set; }
    //}

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


        /// <summary>
        /// This is the content of file
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// This is the slitted string of <see cref="Content"/>
        /// </summary>
        public Dictionary<int, string> SplittedContent { get; set; } = new Dictionary<int, string>();

        public async Task ReadFileContentAsync(bool bIsupdate = true)
        {
            await CommandHelper.RunCommandAsync(() => IsBusy, async () =>
             {
                 if (File.Exists(FilePath) && bIsupdate)
                 {
                     Content = await FileUtil.ReadAllTextAsync(FilePath);

                     SplitContentString();
                 }
             }, !IsLoadedFile ? 1000 : 0);
        }


        private void SplitContentString()
        {
            SplittedContent.Clear();

            if (string.IsNullOrEmpty(Content))
                return;

            //split the string
            var matches = Regex.Split(Content, @"[\r\n|\r|\n]");

            int nCounter = 0;
            Array.ForEach(matches, curSplittedLine => { SplittedContent.Add(nCounter++, curSplittedLine); });
        }

        public bool IsBusy { get; set; } = false;

        private bool IsLoadedFile => !string.IsNullOrEmpty(Content);

        public override string ToString()
        {
            return FileInfo.Exists ? $"{FileInfo.Name} / {FileInfo.Length/ 1024} КБ / {FileInfo.CreationTime}" : FileInfo.Name;
        }

    }
}