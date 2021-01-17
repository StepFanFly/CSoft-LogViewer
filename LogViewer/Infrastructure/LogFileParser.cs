using LogViewer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogViewer
{
    interface Iparser<T> where T : LogFile
    {
        T GetLogFile();
        void ApplyAllFilters(IEnumerable<Filter> filters);
    }

    class LogFileParser : Iparser<LogFile>
    {
        public LogFileParser(LogFile file)
        {
            _file = file;
        }

        public void ApplyAllFilters(IEnumerable<Filter> filters)
        {
            foreach (var filter in filters) 
                filter.Apply(_file);
        }

        public LogFile GetLogFile()
        {
            return _file;
        }

        LogFile _file;
    }
}
