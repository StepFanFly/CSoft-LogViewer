using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LogViewer.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace LogViewer
{
    public class LogViewerVM : ViewModelBase
    {
        public ObservableCollection<LogFile> LogFiles { get; set; } = new ObservableCollection<LogFile>() {  /*new LogFile { FileInfo = new FileInfo("Test"), FilePath ="sdfsdfsd"}*/ };

        public ObservableCollection<Filter> Filters { get; set; } = new ObservableCollection<Filter>() {/* new FileNameFilter { }, new FileNameFilter { }, new FileNameFilter { }, new FileNameFilter { } */};

        public LogFile SelLogFile { get; set; }

        public bool FontSelectorShowAll { get; set; } = true;

        public string ViewSting { get; set; }

        public LogViewerVM()
        {
            LogFiles = new ObservableCollection<LogFile>() { new LogFile { FileInfo = new FileInfo("Super"), FilePath = "sdfsdfsd" } };
            Parser = new LogFileParser();
        }

        public RelayCommand OpenCommand => new RelayCommand(() => { _dlgService.ShowAsync(LogFiles); });

        public RelayCommand OpenFilterDlgCommand => new RelayCommand(async () =>
        {
            var dlgRes = await _dlgService.ShowAsync(Filters);

            if (dlgRes == true)
            {
                Filters = ServiceLocator.Current.GetInstance<FilterVM>().Filters;
                Parser.ApplyAllFilters(SelLogFile, Filters);
                ViewSting = Parser.GetResult();
            }
        }, () => { return SelLogFile != null; });

        public RelayCommand<LogFile> RemoveSelLogCommand => new RelayCommand<LogFile>(logFileToRemove => LogFiles.Remove(logFileToRemove));

        public RelayCommand<Filter> RemoveFilterCommand => new RelayCommand<Filter>((filter) =>
        {
            Filters.Remove(filter);

            Parser.ApplyAllFilters(SelLogFile, Filters);
            ViewSting = Parser.GetResult();

        });

        public RelayCommand<LogFile> ShowLogCommand
        {
            get
            {
                return new RelayCommand<LogFile>(async (selFile) =>
                {
                if (selFile != null)
                {
                    await selFile.ReadFileContentAsync();
                    ViewSting = selFile.Content;
                }
                });
            }
        }


        public RelayCommand ExitApplicationCommand  => new RelayCommand(() =>
        {
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Close();
        });

        private readonly IDialogService _dlgService = ServiceLocator.Current.GetInstance<IDialogService>();

        private readonly Iparser<LogFile> Parser;
    }
}
