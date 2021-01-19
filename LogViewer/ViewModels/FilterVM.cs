using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;

namespace LogViewer
{
    public class FilterVM : ViewModelBase
    {

        public void AddFilters(ObservableCollection<Filter> filters)
        {
            Filters.AsEnumerable().Concat(filters);
        }

        private readonly IDialogService _dlgService = ServiceLocator.Current.GetInstance<IDialogService>();

        public ObservableCollection<Filter> Filters { get; set; } = new ObservableCollection<Filter>();


        public Filter OuterListBoxSelectedFilter { get; set; }

        public RelayCommand<Filter> RemoveFilterCommand => new RelayCommand<Filter>((filter) =>
        {
            Filters.Remove(filter);
        });

        public Filter ItmToCreate { get; set; }

        public RelayCommand ChowFtDlgCommand => new RelayCommand(async () =>
        {
            OuterListBoxSelectedFilter = FilterFactory.Create(eFilterType.eFileName);
            var Result = await _dlgService.ShowAsync(OuterListBoxSelectedFilter);
            if (Result == true)
            {

                OuterListBoxSelectedFilter.Init(ServiceLocator.Current.GetInstance<LogViewerVM>().SelLogFile);
                Filters.Add(OuterListBoxSelectedFilter);
            }
            else {
                OuterListBoxSelectedFilter = null;
            }
        });
    }
}
