using LogViewer.DialogWindows;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommonServiceLocator;
using LogViewer.Models;

namespace LogViewer
{
    public interface IDialogService
    {
        void Show(object content);

        Task<bool> ShowAsync(object content);
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

        public async Task<bool> ShowAsync(object content)
        {
            bool bRes = false;

            if (content is ObservableCollection<Filter> filters)
            {
                ServiceLocator.Current.GetInstance<FilterVM>().AddFilters(filters);
                var view = new FilterDialog();
                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog", null, (sender, eventArgs) => { bRes = (bool)eventArgs.Parameter; });
            }

            if (content is Filter) 
            {
                var view = new FilterTypeDialog();
                //show the dialog
                var result = await DialogHost.Show(view, "FilterDialog", null, (sender, eventArgs)=> { bRes = (bool)eventArgs.Parameter; });
            }

            return bRes;
        }
    }
}
