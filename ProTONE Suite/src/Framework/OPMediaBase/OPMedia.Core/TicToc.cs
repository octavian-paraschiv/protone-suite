using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPMedia.Core.Logging;
using System.Diagnostics;

namespace OPMedia.Core
{
    public class TicToc : IDisposable
    {
        private DateTime _tic = DateTime.MinValue;
        private Stopwatch _sw = new Stopwatch();

        string _opName = "";

        double _totalTime = 0;
        long _totalCount = 0;
        object _syncRoot = new object();
        
        int _avgReportCount = 20;
        long _longOpThreshold = 10;

        public TicToc(string opName, long longOpThreshold = 10, int avgReportCount = 20)
        {
            _opName = opName;
            _avgReportCount = avgReportCount;
            _longOpThreshold = longOpThreshold;

            Tic();
        }

        public void Tic()
        {
            _sw.Restart();
        }

        public void Toc()
        {
            TocInternal(false);
        }

        void TocInternal(bool disposing)
        {
            _sw.Stop();
            long diff = _sw.ElapsedMilliseconds;

            if (diff > _longOpThreshold)
                Logger.LogToConsole(string.Format("Last \"{0}\" operation took {1:0.000} msec", _opName, diff));

            lock (_syncRoot)
            {
                _totalTime += diff;
                _totalCount++;
                double avg = _totalTime / _totalCount;

                if (disposing || _totalCount % _avgReportCount == 0)
                    Logger.LogToConsole(string.Format("\"{0}\" operation takes {1:0.000} msec in average", _opName, diff));
            }
        }

        public void Dispose()
        {
            TocInternal(true);
        }
    }
}
