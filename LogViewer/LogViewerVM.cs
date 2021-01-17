using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace LogViewer
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


        public string Content { get;  set; }

        public async Task ReadFileContentAsync() 
        {
            
            if (File.Exists(FilePath)) 
            {
                Content = await FileUtil.ReadAllTextAsync(FilePath);
            }
        }
    }




    public class LogViewerVM : ViewModelBase 
    {
        public ObservableCollection<LogFile> LogFiles { get; set; }

        public FilterList Filters { get; set; }

        private readonly IDialogService _dlgService;

        public LogFile SelLogFile { get; set; }

        public LogViewerVM()
        {
            _dlgService = new DialogService();
            LogFiles = new ObservableCollection<LogFile>(){ new LogFile { FileInfo = new FileInfo("Test"), FilePath ="sdfsdfsd"} };
            Filters = new FilterList() { new FileNameFilter { } };
        }

        public RelayCommand OpenCommand => new RelayCommand(()=> { _dlgService.Show(LogFiles); });

        public RelayCommand OpenFilterDlgCommand => new RelayCommand(async  () => {await _dlgService.ShowAsync(Filters); });

        public RelayCommand<LogFile> RemoveSelLogCommand => new RelayCommand<LogFile>(logFileToRemove => LogFiles.Remove(logFileToRemove) );

        public RelayCommand<LogFile> ShowLogCommand { get { return new RelayCommand<LogFile>(async(selFile)=> 
        {
            if(selFile != null)
                await selFile.ReadFileContentAsync();
        }); } }
    }

}
