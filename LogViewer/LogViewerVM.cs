using CommonServiceLocator;
using GalaSoft.MvvmLight;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace LogViewer
{

    public class OpenLogFileCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is LogViewerVM vm)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;
                dialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
                if (true == dialog.ShowDialog())
                {
                    foreach (string filename in dialog.FileNames)
                    {
                        Debug.Print(filename);
                    }
                    
                }
            }
        }
    }

    public class LogViewerVM : INotifyPropertyChanged
    {

        public String MyText { get; set; }
        public OpenLogFileCommand OpenCommand { get; set; } = new OpenLogFileCommand();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
