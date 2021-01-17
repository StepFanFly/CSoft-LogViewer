using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight;

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
                SimpleIoc.Default.Register<FilterVM>();

                SimpleIoc.Default.Register<IDialogService, DialogService>();
            }
            else
            {
                SimpleIoc.Default.Register<LogViewerVM>();
                SimpleIoc.Default.Register<FilterVM>();

                SimpleIoc.Default.Register<IDialogService, DialogService>();
            }
        }

        public LogViewerVM LogVM => ServiceLocator.Current.GetInstance<LogViewerVM>();
        public FilterVM FilterVM => ServiceLocator.Current.GetInstance<FilterVM>();

        public IDialogService DlgService => ServiceLocator.Current.GetInstance<IDialogService>();
    }
}
