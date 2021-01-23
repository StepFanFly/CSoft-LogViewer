

using LogViewer.Infrastructure;
using LogViewer.Models;
using LogViewer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace LogViewer
{
    public class LogViewerVM : ViewModelBase
    {
        #region Public fields
        /// <summary>
        /// Collection of search field which also may be LogFile
        /// </summary>
        public ObservableCollectionExt<SearhField> LogFiles { get; set; } = new ObservableCollectionExt<SearhField>() {  /*new LogFile { FileInfo = new FileInfo("Test"), FilePath ="sdfsdfsd"}*/ };

        /// <summary>
        /// This is sel field
        /// </summary>
        public SearhField SelSearchField { get; set; }

        public bool FontSelectorShowAll { get; set; } = true;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LogViewerVM()
        {
            var test = new LogFile { NodeName = "Super", FileInfo = new FileInfo("Super"), FilePath = "sdfsdfsd" };
            test.ChildSearchedFields.Add(new SearhField() { NodeName = "Child 1",Content = "bla bla bla 1", Parent = test });
            test.ChildSearchedFields.Add(new SearhField() { NodeName = "Child 2", Content = "bla bla bla 2", Parent = test });

            LogFiles.Add(test);
            Parser = new LogFileParser();
        }
        #endregion

        #region Relay Commands
        /// <summary>
        /// Open dialog and choose selected files
        /// </summary>
        public RelayCommand OpenAndChooseLogFielsCommand => new RelayCommand(() => { _dlgService.ShowAsync(LogFiles); });

        /// <summary>
        /// Invoke filters dialog and apply all of their to <see cref="SelSearchField"/>
        /// </summary>
        public RelayCommand OpenFilterDlgCommand => new RelayCommand(async () =>
        {
            var dlgRes = await _dlgService.ShowAsync(SelSearchField.Filters);

            if (dlgRes == true)
            {
                Parser.ApplyAllFilters(SelSearchField, SelSearchField.Filters);
                SelSearchField.ViewSting = Parser.GetResult();
            }
        }, () => { return SelSearchField != null; });


        /// <summary>
        /// Removes <see cref="SelSearchField"/> from list of <see cref="LogFiles"/>
        /// </summary>
        public RelayCommand<LogFile> RemoveSelLogCommand => new RelayCommand<LogFile>(logFileToRemove => LogFiles.Remove(logFileToRemove));

        /// <summary>
        /// Remove selected filter from <see cref="SelSearchField"/>
        /// </summary>
        public RelayCommand<Filter> RemoveFilterCommand => new RelayCommand<Filter>((filter) =>
        {
            SelSearchField.Filters.Remove(filter);
            Parser.ApplyAllFilters(SelSearchField, SelSearchField.Filters);
            SelSearchField.ViewSting = Parser.GetResult();
        });

        /// <summary>
        /// Start the reading log file from file
        /// </summary>
        public RelayCommand LoadLogFromFileCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                   await ((LogFile)SelSearchField).ReadFileContentAsync();
                   SelSearchField.ViewSting = SelSearchField.Content;

                }, () => { return SelSearchField != null && SelSearchField is LogFile; });
            }
        }

        /// <summary>
        /// Closes application
        /// </summary>
        public RelayCommand ExitApplicationCommand  => new RelayCommand(() =>
        {
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Close();
        });

        #endregion

        private IDialogService _dlgService => App.Host.Services.GetRequiredService<IDialogService>();

        private readonly Iparser<SearhField> Parser;
    }
}
