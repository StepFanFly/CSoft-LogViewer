using LogViewer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogViewer
{
    interface Iparser<T> where T : LogFile
    {
        T GetLogFile();
<<<<<<< HEAD
        void ApplyAllFilters( IEnumerable<Filter> filters);
        string GetResult();
=======
        void ApplyAllFilters(IEnumerable<Filter> filters);
>>>>>>> 63a338d1bd5316f82bf5c589f9ccecf0916e7419
    }

    class LogFileParser : Iparser<LogFile>
    {
        public LogFileParser(LogFile file)
        {
            _file = file;
        }

<<<<<<< HEAD
        public void ApplyAllFilters( IEnumerable<Filter> filters)
        {
            foreach (var filter in filters)
                _parseString = filter.Apply(_file);
=======
        public void ApplyAllFilters(IEnumerable<Filter> filters)
        {
            foreach (var filter in filters) 
                filter.Apply(_file);
>>>>>>> 63a338d1bd5316f82bf5c589f9ccecf0916e7419
        }

        public LogFile GetLogFile()
        {
            return _file;
        }

<<<<<<< HEAD
        public string GetResult()
        {
            return _parseString;
        }

        LogFile _file;
        string _parseString = new string("");
=======
        LogFile _file;
>>>>>>> 63a338d1bd5316f82bf5c589f9ccecf0916e7419
    }
}
