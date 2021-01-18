using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LogViewer.Models;
using System.Collections.ObjectModel;
<<<<<<< HEAD
using System.IO;
=======
>>>>>>> 63a338d1bd5316f82bf5c589f9ccecf0916e7419

namespace LogViewer
{
    public class LogViewerVM : ViewModelBase 
    {
<<<<<<< HEAD
        //files
=======
>>>>>>> 63a338d1bd5316f82bf5c589f9ccecf0916e7419
        public ObservableCollection<LogFile> LogFiles { get; set; } = new ObservableCollection<LogFile>() {  /*new LogFile { FileInfo = new FileInfo("Test"), FilePath ="sdfsdfsd"}*/ };

        public ObservableCollection<Filter> Filters { get; set; } = new ObservableCollection<Filter>() {/* new FileNameFilter { }, new FileNameFilter { }, new FileNameFilter { }, new FileNameFilter { } */};

        private readonly IDialogService _dlgService = ServiceLocator.Current.GetInstance<IDialogService>();

        private readonly Iparser<LogFile> Parser; 

        public LogFile SelLogFile { get; set; }

<<<<<<< HEAD
        public string ViewSting { get; set; }
        
        public LogViewerVM()
        {
            LogFiles = new ObservableCollection<LogFile>(){  new LogFile { FileInfo = new FileInfo("Super"), FilePath ="sdfsdfsd"}};
=======
        public LogViewerVM()
        {
            //LogFiles = new ObservableCollection<LogFile>(){  /*new LogFile { FileInfo = new FileInfo("Test"), FilePath ="sdfsdfsd"}*/ };
>>>>>>> 63a338d1bd5316f82bf5c589f9ccecf0916e7419
           // Filters = new ObservableCollection<Filter>() {/* new FileNameFilter { }, new FileNameFilter { }, new FileNameFilter { }, new FileNameFilter { } */};
            Parser = new LogFileParser(SelLogFile);
        }

        public RelayCommand OpenCommand => new RelayCommand(()=> { _dlgService.Show(LogFiles); });

        public RelayCommand OpenFilterDlgCommand => new RelayCommand(async  () => 
        {
            var dlgRes = await _dlgService.ShowAsync(Filters);
<<<<<<< HEAD
            if (dlgRes == true) 
            {
                Filters = ServiceLocator.Current.GetInstance<FilterVM>().Filters;
                Parser.ApplyAllFilters(Filters);
                ViewSting = Parser.GetResult();
            }
=======
            if (dlgRes == true)
                Filters = ServiceLocator.Current.GetInstance<FilterVM>().Filters;

>>>>>>> 63a338d1bd5316f82bf5c589f9ccecf0916e7419
        });

        public RelayCommand<LogFile> RemoveSelLogCommand => new RelayCommand<LogFile>(logFileToRemove => LogFiles.Remove(logFileToRemove) );

        public RelayCommand<LogFile> ShowLogCommand { get { return new RelayCommand<LogFile>(async(selFile)=> 
        {
<<<<<<< HEAD
            if (selFile != null) 
            {
                await selFile.ReadFileContentAsync();
                ViewSting = selFile.Content;
            }
                
=======
            if(selFile != null)
                await selFile.ReadFileContentAsync();
>>>>>>> 63a338d1bd5316f82bf5c589f9ccecf0916e7419
        }); } }
    }
}
