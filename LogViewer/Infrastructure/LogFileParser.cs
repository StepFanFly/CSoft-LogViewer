using LogViewer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace LogViewer
{
    interface Iparser<T> where T : LogFile
    {
        T GetLogFile();
        void ApplyAllFilters( IEnumerable<Filter> filters);
        string GetResult();
    }

    class LogFileParser : Iparser<LogFile>
    {
        public LogFileParser(LogFile file)
        {
            _file = file;
        }


        public void ApplyAllFilters( IEnumerable<Filter> filters)
        {
            _parseString = "";
            //for each filter we call for apply, get dictionary in result
            //then collapse alldictionaries
            //or just calculate result on go
            if (filters.Count() > 0) {
                Dictionary<int, string> result = new Dictionary<int, string>();
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
                            result = dict;
                            break;
                        case eOperationType.eShielding:
                            //remove from result all pairs from tmp somehow
                            //dict = result.Where(x => tmp.ContainsKey(x.Key)).ToDictionary(x => x.Key, x => x.Value);
                            //result = dict;
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
                _parseString = _file.Content;
            }
        }

        public LogFile GetLogFile()
        {
            return _file;
        }

        public string GetResult()
        {
            return _parseString;
        }

        LogFile _file;
        string _parseString = new string("");
    }
}
