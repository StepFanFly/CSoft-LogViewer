using CommonServiceLocator;


using LogViewer.Infrastructure;
using LogViewer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;

namespace LogViewer
{

    /// <summary>
    /// This is simple filter VM
    /// </summary>
    public class FilterVM : ViewModelBase
    {


        public FilterVM(IDialogService dlgService)
        {
            _dlgService = dlgService;
        }

        #region Public fields

        /// <summary>
        /// New instance of filter
        /// </summary>
        public Filter ItmToCreate { get; set; }

        /// <summary>
        /// Selected filter
        /// </summary>
        public Filter OuterListBoxSelectedFilter { get; set; }

        /// <summary>
        /// Collection of filters
        /// </summary>
        public ObservableCollectionExt<Filter> Filters { get; set; }

        #endregion

        #region Relay Commands

        public RelayCommand<Filter> RemoveFilterCommand => new RelayCommand<Filter>((filter) =>
        {
            Filters.Remove(filter);
        });

        public RelayCommand ChowFtDlgCommand => new RelayCommand(async () =>
        {
            ItmToCreate = FilterFactory.Create(eFilterType.eFileName);
            var Result = await _dlgService.ShowAsync(ItmToCreate);
            if (Result == true)
            {
                OuterListBoxSelectedFilter = ItmToCreate;
                OuterListBoxSelectedFilter.Init(App.Host.Services.GetRequiredService<LogViewerVM>().SelSearchField);
                Filters.Add(OuterListBoxSelectedFilter);
            }
            else
            {
                OuterListBoxSelectedFilter = null;
            }
        });

        #endregion

        #region Methods

        public void AddFilters(ObservableCollectionExt<Filter> filters)
        {
            Filters = filters;
            OuterListBoxSelectedFilter = null;
        }

        #endregion

        private readonly IDialogService _dlgService;
    }
}
