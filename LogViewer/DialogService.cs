using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;

namespace LogViewer
{
    public interface IDialogService
    {
        void Show(object content);
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
    }
}
