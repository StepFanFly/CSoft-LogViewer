using GalaSoft.MvvmLight;
using LogViewer.Models;
using System;
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

    public enum eOperationType 
    {
    eNone,
    eUnion,
    eIntersect,
    eShielding
    }

    public abstract class Filter : ViewModelBase
    {

        public abstract string Name { get; set; }
        public abstract eFilterType Type { get; set; }
        public abstract string Apply(LogFile file);
        public string SelectedKeyWord { get; set; }
        public ObservableCollection<string> AllKeyWords { get; private set; } = new ObservableCollection<string>();
        public eOperationType SelectedOperationType {get;set;}


        private void ApplyRegex() {
            if (string.IsNullOrEmpty(_file.Content))
                return;

            MatchCollection matches = InternalRegex.Matches(_file.Content);
            foreach (Match match in matches)
            {
                if (!AllKeyWords.Contains(match.Groups["target"].Value))
                {
                    AllKeyWords.Add(match.Groups["target"].Value);
                }
            }
        }
        protected abstract string ApplyFilter();

        internal void Init(LogFile selLogFile)
        {
            _file = selLogFile;
            ApplyRegex();
        }

        protected LogFile _file;
        protected abstract Regex InternalRegex { get; set; }
    }

    public class FileNameFilter : Filter
    {
        public override string Name { get; set; } = "По имени файла";
        protected override Regex InternalRegex { get; set; } = new Regex(@"(\[FILE:)(?<target>.*\.cpp)");

        public override eFilterType Type { get; set; } = eFilterType.eFileName;

        public override string Apply(LogFile file)
        {
            return ApplyFilter();
        }
        protected override string ApplyFilter()
        {
            string res = new string("");
            if (SelectedKeyWord != null)
            {
                Regex regex = new Regex(@"([\r\n|\r|\n])(?<target>.*\[FILE:" + SelectedKeyWord + @".*)([\r\n|\r|\n])");
                MatchCollection matches = regex.Matches(_file.Content);
                foreach (Match match in matches)
                {
                    var groups = match.Groups;
                    res += groups["target"].Value + "\n";
                }
            }
            return res;
        }
    }

    public class LabelFilter : Filter
    {
        public override string Name { get; set; } = "По меткам";
        protected override Regex InternalRegex { get; set; } = new Regex(@"(\[)(?<target>[A-Z]*)(\])");
        public override eFilterType Type { get; set; } = eFilterType.eLabel;

        public override string Apply(LogFile file)
        {
            return ApplyFilter();
        }
        protected override string ApplyFilter()
        {
            string res = new string("");
            if (SelectedKeyWord != null)
            {
                Regex regex = new Regex(@"([\r\n|\r|\n])(?<target>.*\[" + SelectedKeyWord + @"\].*)([\r\n|\r|\n])");
                MatchCollection matches = regex.Matches(_file.Content);
                foreach (Match match in matches)
                {
                    var groups = match.Groups;
                    res += groups["target"].Value + "\r\n";
                }
            }
            return res;
        }
    }

    public static class FilterFactory
    {
        public static ObservableCollection<Filter> GetInstance() => Filters;

        public static ObservableCollection<Filter> Filters { get; set; } = new ObservableCollection<Filter>();

        public static ObservableCollection<eOperationType> GetOperationTypes() => OperationTypes;

        public static ObservableCollection<eOperationType> OperationTypes { get; set; } = new ObservableCollection<eOperationType>();


        static FilterFactory()
        {
            Filters.Add(new FileNameFilter());
            Filters.Add(new LabelFilter());

            OperationTypes.Add(eOperationType.eIntersect);
            OperationTypes.Add(eOperationType.eShielding);
            OperationTypes.Add(eOperationType.eUnion);
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