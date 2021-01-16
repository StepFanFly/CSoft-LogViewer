using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer
{

    
    static class Converter
    {
      readonly static System.Text.Encoding WINDOWS1251 = Encoding.GetEncoding(1251);
    readonly static System.Text.Encoding UTF8 = Encoding.UTF8;

    public static string ConvertWin1251ToUTF8(string inString)
    {
        return UTF8.GetString(WINDOWS1251.GetBytes(inString));
    }

}
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
                Content = await File.ReadAllTextAsync(FilePath);
            }
        }
    }

    public class LogViewerVM : ViewModelBase 
    {
        public ObservableCollection<LogFile> LogFiles { get; set; }

        private readonly IDialogService _dlgService;

        public LogFile SelLogFile { get; set; }

        public LogViewerVM()
        {
            _dlgService = new DialogService();
            LogFiles = new ObservableCollection<LogFile>();// { new LogFile { FileInfo = new FileInfo("Test"), FilePath ="sdfsdfsd"} };
        }

        public RelayCommand OpenCommand => new RelayCommand(()=> { _dlgService.Show(LogFiles); });

        public RelayCommand<LogFile> RemoveSelLogCommand => new RelayCommand<LogFile>(logFileToRemove => LogFiles.Remove(logFileToRemove) );

        public RelayCommand<LogFile> ShowLogCommand { get { return new RelayCommand<LogFile>(async(selFile)=> 
        {
            await selFile.ReadFileContentAsync();
        }); } }
    }

}
