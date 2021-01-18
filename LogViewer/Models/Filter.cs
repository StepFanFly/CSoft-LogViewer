using GalaSoft.MvvmLight;
using LogViewer.Models;
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

        public abstract string Apply(LogFile file);

<<<<<<< HEAD
=======

>>>>>>> 63a338d1bd5316f82bf5c589f9ccecf0916e7419
        public ObservableCollection<string> KeyWords { get; set; } = new ObservableCollection<string>();

    }

    public class FileNameFilter : Filter
    {
        public override string Name { get; set; } = "По имени файла";

        public override eFilterType Type { get; set; } = eFilterType.eFileName;

        public override string Apply(LogFile file)
        {
<<<<<<< HEAD

=======
>>>>>>> 63a338d1bd5316f82bf5c589f9ccecf0916e7419
            //Fill KeyWords
            //TODO: parse File

            return string.Empty;
        }
    }

    public class LabelFilter : Filter
    {
        public override string Name { get; set; } = "По меткам";

        public override eFilterType Type { get; set; } = eFilterType.eLabel;

        public override string Apply(LogFile file)
        {
            //Fill KeyWords
            //TODO: parse File
            return string.Empty;
        }
    }


    public class CreationFilter : Filter
    {
        public override string Name { get; set; }

        public override eFilterType Type { get; set; } = eFilterType.eNone;

        public override string Apply(LogFile file)
        {
            //Fill KeyWords
            //TODO: parse File
            return string.Empty;
        }
    }


    public static class FilterFactory
    {
        public static ObservableCollection<Filter> GetInstance() => Filters;

        public static ObservableCollection<Filter> Filters { get; set; } = new ObservableCollection<Filter>();

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
                    return new FileNameFilter();
                case eFilterType.eLabel:
                    return new LabelFilter();
            }

            return null;
        }


        public static eFilterType GetTypeByName(string filterName)
        {
           var FindedType = Filters.FirstOrDefault(curItm => { return curItm.Name.Equals(filterName); });

           return FindedType != null ? FindedType.Type : eFilterType.eNone;
           
        }
    }
}