using LogViewer.Models;
using System.Collections.Generic;
using System.Linq;

namespace LogViewer
{
    interface Iparser<T> where T : SearhField
    {
        void ApplyAllFilters(SearhField file, IEnumerable<Filter> filters);
        string GetResult();
    }

    class LogFileParser : Iparser<SearhField>
    {
     
        public void ApplyAllFilters(SearhField file, IEnumerable<Filter> filters)
        {
            _parseString = "";
            //for each filter we call for apply, get dictionary in result
            //then collapse alldictionaries
            //or just calculate result on go
            if (filters.Count() > 0) {
                SortedDictionary<int, string> result = new SortedDictionary<int, string>();
                eOperationType storedOperation = eOperationType.eUnion;
                foreach (var filter in filters)
                {
                    Dictionary<int, string> tmp = new Dictionary<int, string>();
                    tmp = filter.ApplyFilter();
                    switch (storedOperation)
                    {
                        case eOperationType.eUnion:
                            tmp.ToList().ForEach(x => result[x.Key] = x.Value);
                            break;
                        case eOperationType.eIntersect:
                            Dictionary<int, string> dict = new Dictionary<int, string>();
                            dict = result.Where(x => tmp.ContainsKey(x.Key)).ToDictionary(x => x.Key, x=>x.Value);
                            result = new SortedDictionary<int, string>(dict);
                            break;
                        case eOperationType.eShielding:
                            var keyToRemove = result.Keys.Where(x => (tmp.ContainsKey(x))).ToList();
                            foreach (var key in keyToRemove)
                                result.Remove(key);
                            break;
                        default:
                            break;
                    }

                    storedOperation = filter.SelectedOperationType;
                }
                foreach (var pair in result)
                    _parseString += pair.Value + "\n";
            }
            else
            {
                _parseString = file.Content;
            }
        }

        public string GetResult()
        {
            return _parseString;
        }

        string _parseString = new string("");
    }
}
