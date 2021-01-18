using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LogViewer.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace LogViewer
{
    public class LogViewerVM : ViewModelBase 
    {
        public ObservableCollection<LogFile> LogFiles { get; set; } = new ObservableCollection<LogFile>() {  /*new LogFile { FileInfo = new FileInfo("Test"), FilePath ="sdfsdfsd"}*/ };

        public ObservableCollection<Filter> Filters { get; set; } = new ObservableCollection<Filter>() {/* new FileNameFilter { }, new FileNameFilter { }, new FileNameFilter { }, new FileNameFilter { } */};

        private readonly IDialogService _dlgService = ServiceLocator.Current.GetInstance<IDialogService>();

        private readonly Iparser<LogFile> Parser; 

        public LogFile SelLogFile { get; set; }

        public string ViewSting { get; set; }

        public LogViewerVM()
        {
            LogFiles = new ObservableCollection<LogFile>(){  new LogFile { FileInfo = new FileInfo("Super"), FilePath ="sdfsdfsd"}};
           // Filters = new ObservableCollection<Filter>() {/* new FileNameFilter { }, new FileNameFilter { }, new FileNameFilter { }, new FileNameFilter { } */};
            Parser = new LogFileParser(SelLogFile);
        }

        public RelayCommand OpenCommand => new RelayCommand(()=> { _dlgService.Show(LogFiles); });

        public RelayCommand OpenFilterDlgCommand => new RelayCommand(async  () => 
        {
            var dlgRes = await _dlgService.ShowAsync(Filters);
            if (dlgRes == true) 
            {
                Filters = ServiceLocator.Current.GetInstance<FilterVM>().Filters;
                Parser.ApplyAllFilters(Filters);
                ViewSting = Parser.GetResult();
            }
        });

        public RelayCommand<LogFile> RemoveSelLogCommand => new RelayCommand<LogFile>(logFileToRemove => LogFiles.Remove(logFileToRemove) );

        public RelayCommand<LogFile> ShowLogCommand { get { return new RelayCommand<LogFile>(async(selFile)=> 
        {
            if (selFile != null) 
            {
                await selFile.ReadFileContentAsync();
                ViewSting = selFile.Content;
            }
                
        }); } }
    }
}
