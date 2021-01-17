using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;

namespace LogViewer
{

    public enum eFilterType 
    {
    eNone,
    eFileName,
    eLabel
    }

    public abstract class Filter : ViewModelBase
    {
        public abstract string Name { get; set; }

        public abstract eFilterType Type { get; set; }

        public abstract void Apply(LogFile file);
    }

    public class FileNameFilter : Filter
    {
        public override string Name { get; set; } = "По имени файла";

        public override eFilterType Type { get; set; } = eFilterType.eFileName;

        public override void Apply(LogFile file)
        {

        }
    }

    public class LabelFilter : Filter
    {
        public override string Name { get; set; } = "По меткам";

        public override eFilterType Type { get; set; } = eFilterType.eLabel;

        public override void Apply(LogFile file)
        {

        }
    }

    public class FilterList : ObservableCollection<Filter> 
    {
    
    }

    public static class FilterFactory
    {
        public static FilterList GetInstance() => Filters;

        public static FilterList Filters { get; set; } = new FilterList();

        static FilterFactory()
        {
            Filters.Add(new FileNameFilter());
            Filters.Add(new LabelFilter());
        }

        public static Filter Create(eFilterType filterType) 
        {
            switch (filterType)
            {
                case eFilterType.eNone:
                    break;
                case eFilterType.eFileName:
                    new FileNameFilter();
                    break;
                case eFilterType.eLabel:
                    new LabelFilter();
                    break;
            }

            return null;
        }


        public static eFilterType GetTypeByName(string filterName)
        {
           var FindedType = Filters.FirstOrDefault(curItm => { return curItm.Name.Equals(filterName); });

           return FindedType != null ? FindedType.Type : eFilterType.eNone;
           
        }

        //public static Filter CreateFileNameFilter() { return new FileNameFilter(); }
        //public static Filter CreateLabelFilter() { return new LabelFilter(); }
        //public static bool IsCurrent(string Name)
        //{

        //}
    }


    public class FilterVM : ViewModelBase 
    {
        public FilterList Filters { get; set; }

        Filter _SelectedFilter;
        public Filter SelectedFilter { get=> _SelectedFilter;
            set { _SelectedFilter = value;
                int n = 1;
            } }

        public RelayCommand NewFilterCommand => new RelayCommand(() => {
            Filters.Add(new FileNameFilter());
            /*_dlgService.Show(LogFiles);*/ });

        public RelayCommand<Filter> RemoveFilterCommand => new RelayCommand<Filter>((filter) =>
        {
            Filters.Remove(filter);
            /*_dlgService.Show(LogFiles);*/
        });

    }
}
