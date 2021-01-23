using GalaSoft.MvvmLight;
using LogViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
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

    public abstract class Filter : ViewModelBase, IEquatable<Filter>
    {

        #region Public fields
        /// <summary>
        /// This is a display label name of <see cref="Filter"/>
        /// </summary>
        public string Name { get => FilterFactory.NameByType(Type);}

        /// <summary>
        /// This is a type of Filter
        /// </summary>
        public abstract eFilterType Type { get; set; }


        /// <summary>
        /// Selected kWord
        /// </summary>
        public string SelectedKeyWord { get; set; }

        /// <summary>
        /// Collection of kWords
        /// </summary>
        public ObservableCollection<string> AllKeyWords { get; private set; } = new ObservableCollection<string>();

        /// <summary>
        /// Operation type to next filter
        /// </summary>
        public eOperationType SelectedOperationType { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Initialize filter
        /// </summary>
        /// <param name="selField"></param>
        internal void Init(SearhField selField)
        {
            _field = selField;
            ApplyRegex();
        }

        /// <summary>
        /// Run apply filter mechanism
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<int, string> ApplyFilter();

        /// <summary>
        /// Impl of <see cref="IEquatable<Filter>"/>
        /// </summary>
        /// <param name="other">other filter</param>
        /// <returns></returns>
        public bool Equals([AllowNull] Filter other)
        {
            bool bIsEquals = false;
            if (other != null && SelectedKeyWord != null)
            {
                bIsEquals = SelectedKeyWord.Equals(other.SelectedKeyWord) && Type == other.Type;
            }
            else if (string.IsNullOrEmpty(SelectedKeyWord))
            {
                bIsEquals = Type == other.Type;

            }
            return bIsEquals;
        }

        /// <summary>
        /// Apply Regex
        /// </summary>
        private void ApplyRegex()
        {
            if (string.IsNullOrEmpty(_field.Content))
                return;

            MatchCollection matches = InternalRegex.Matches(_field.Content);
            foreach (Match match in matches)
            {
                if (!AllKeyWords.Contains(match.Groups["target"].Value))
                {
                    AllKeyWords.Add(match.Groups["target"].Value);
                }
            }
        }

        #endregion

        protected SearhField _field;
        /// <summary>
        /// This is regular expression for finding keywords in input file
        /// </summary>
        protected abstract Regex InternalRegex { get; set; }
    }

    public class FileNameFilter : Filter
    {
        #region Public fields

        public override eFilterType Type { get; set; } = eFilterType.eFileName;

        #endregion

        #region Methods
        public override Dictionary<int, string> ApplyFilter()
        {
            Dictionary<int, string> res = new Dictionary<int, string>();
            if (SelectedKeyWord != null)
            {
                Regex regex = new Regex(@"(?<target>.*\[FILE:" + SelectedKeyWord + @".*)");
                foreach (var pair in _field.SplittedContent)
                {
                    Match match = regex.Match(pair.Value);
                    if (match.Success)
                    {
                        res.Add(pair.Key, pair.Value);
                    }
                }
            }
            return res;
        }
        #endregion

        protected override Regex InternalRegex { get; set; } = new Regex(@"(\[FILE:)(?<target>.*\.cpp)");
    }

    public class LabelFilter : Filter
    {
        #region Public fields

        public override eFilterType Type { get; set; } = eFilterType.eLabel;

        #endregion

        #region Methods
        public override Dictionary<int, string> ApplyFilter()
        {
            Dictionary<int, string> res = new Dictionary<int, string>();
            if (SelectedKeyWord != null)
            {
                Regex regex = new Regex(@"(?<target>.*\[" + SelectedKeyWord + @"\].*)");
                foreach (var pair in _field.SplittedContent)
                {
                    Match match = regex.Match(pair.Value);
                    if (match.Success)
                    {
                        res.Add(pair.Key, pair.Value);
                    }
                }
            }
            return res;
        }

        #endregion

        protected override Regex InternalRegex { get; set; } = new Regex(@"(\[)(?<target>[A-Z]*)(\])");
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

        public static string NameByType(eFilterType filterType)
        {
            if(eFilterType.eFileName == filterType)
                return "По имени файла";
            else if (eFilterType.eLabel == filterType)
                return "По меткам";

            return "";
        }
    }
}