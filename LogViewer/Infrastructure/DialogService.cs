﻿using LogViewer.DialogWindows;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommonServiceLocator;
using LogViewer.Models;
using System.Linq;

namespace LogViewer
{
    public interface IDialogService
    {
        Task<bool> ShowAsync(object content);
    }

    public class DialogService : IDialogService
    {

        public async Task<bool> ShowAsync(object content)
        {
            bool bRes = false;

            if (content is ObservableCollection<LogFile> contentWithLogFiles)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;
                dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
                if (true == dialog.ShowDialog())
                {
                    foreach (string filename in dialog.FileNames)
                    {
                        bool bHasAlereadyLoaded = contentWithLogFiles.Any((logfile => { return logfile.FilePath.Equals(filename); }));
                        if (bHasAlereadyLoaded)
                            continue;
                        var currFile = new FileInfo(filename);
                        if (currFile.Exists)
                        {
                            var newFile = new LogFile { FileInfo = currFile, FilePath = filename };
                            contentWithLogFiles.Add(newFile);
                        }
                    }

                    //TODO:need to refactor
                    foreach (var newFile in contentWithLogFiles)
                    {
                        if(string.IsNullOrEmpty(newFile.Content))
                         _ = Task.Run(async () => await newFile.ReadFileContentAsync());
                    
                    }

                    return true;
                }
            }

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
