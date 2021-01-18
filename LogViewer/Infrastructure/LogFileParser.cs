using LogViewer.Models;
using System.Collections.Generic;

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
            foreach (var filter in filters)
                _parseString = filter.Apply(_file);

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
