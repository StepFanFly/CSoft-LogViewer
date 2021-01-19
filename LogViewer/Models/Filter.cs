using GalaSoft.MvvmLight;
using LogViewer.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

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
        public void Init(LogFile file)
        {
            _file = file;
            ApplyRegex();
        }
        public abstract string Name { get; set; }
        public abstract eFilterType Type { get; set; }

        public abstract string Apply(LogFile file);

        public string SelectedKeyWord { get; set; }
        public ObservableCollection<string> AllKeyWords { get;  private set; } = new ObservableCollection<string>();
        private void ApplyRegex() {
            if (string.IsNullOrEmpty(_file.Content))
                return;

            MatchCollection matches = InternalRegex.Matches(_file.Content);
            foreach (Match match in matches)
            {
                if (!AllKeyWords.Contains(match.Value))
                {
                    AllKeyWords.Add(match.Value);
                }
            }
        }
        protected virtual string ApplyFilter()
        {
            string res = new string("");
            if (SelectedKeyWord != null)
            {
                var lines = Regex.Split(_file.Content, "\r\n|\r|\n");

                foreach (string line in lines)
                {
                    if (line.Contains(SelectedKeyWord))
                    {
                        res += line + "\n";
                    }
                }
            }
            return res;
        }
        protected LogFile _file;
        protected abstract Regex InternalRegex { get; set; }
    }

    public class FileNameFilter : Filter
    {
        public override string Name { get; set; } = "По имени файла";
        protected override Regex InternalRegex { get; set; } = new Regex(@"([a-zA-Z]*\.cpp)");

        public override eFilterType Type { get; set; } = eFilterType.eFileName;

        public override string Apply(LogFile file)
        {
            return ApplyFilter();
        }
    }

    public class LabelFilter : Filter
    {
        public override string Name { get; set; } = "По меткам";
        protected override Regex InternalRegex { get; set; } = new Regex(@"(\[[A-Z]*\])");
        public override eFilterType Type { get; set; } = eFilterType.eLabel;

        public override string Apply(LogFile file)
        {
            return ApplyFilter();
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