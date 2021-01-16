using System;
using System.Collections.Generic;
using System.Text;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight;
using System.ComponentModel;

namespace LogViewer
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<LogViewerVM>();
            }
            else
            {
                SimpleIoc.Default.Register<LogViewerVM>();
            }
        }

        public LogViewerVM LogVM => ServiceLocator.Current.GetInstance<LogViewerVM>();

    }
}
