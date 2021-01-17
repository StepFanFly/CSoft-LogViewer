using LogViewer.DialogWindows;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace LogViewer
{
    public interface IDialogService
    {
        void Show(object content);

        Task ShowAsync(object content);
    }

    public class DialogService : IDialogService
    {
        public void Show(object content)
        {
            if (content is ObservableCollection<LogFile> contentWithLogFiles) 
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;
                dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
                if (true == dialog.ShowDialog())
                {
                    foreach (string filename in dialog.FileNames) 
                    {
                        var currFile = new FileInfo(filename);
                        if (currFile.Exists) 
                        {
                            contentWithLogFiles.Add(new LogFile { FileInfo = currFile, FilePath = filename });
                        }
                    }
                }
            }
        }

        public async Task ShowAsync(object content)
        {
            if (content is FilterList filters)
            {

                FilterList filtersCpy = new FilterList();
                filtersCpy.AsEnumerable().Concat(filters);

                //  filtersCpy.AsEnumerable<Filter>.AddRange(filters);
                var view = new FilterDialog
                {
                    DataContext = new FilterVM() { Filters = filtersCpy }
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            }
                
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
           // throw new NotImplementedException();
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            //throw new NotImplementedException();
        }
    }
}
