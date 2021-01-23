using Microsoft.Extensions.DependencyInjection;

namespace LogViewer
{
    public class ViewModelLocator
    {
        public LogViewerVM LogVM => App.Host.Services.GetRequiredService<LogViewerVM>();
        public FilterVM FilterVM => App.Host.Services.GetRequiredService<FilterVM>();
        public IDialogService DlgService => App.Host.Services.GetRequiredService<DialogService>();
    }
}
