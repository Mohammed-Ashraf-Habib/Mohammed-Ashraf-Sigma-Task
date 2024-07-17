using Serilog.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;
using System.IO;
using Serilog.Formatting.Compact;

namespace Task.Business.Logger
{
    public static class LogExtentions
    {
        public static LoggerConfiguration MapToFile(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            long fileSizeLimitBytes,
            bool rollOnFileSizeLimit,
            string _rootPath,
            int sinkMapCountLimit,
            int flushToDiskInterval,
            int rollingInterval,
            int restrictedToMinimumLevel)
        {
            return loggerSinkConfiguration.Map(

                le => new Tuple<DateTime, LogEventLevel>(new DateTime(le.Timestamp.Year, le.Timestamp.Month, le.Timestamp.Day), le.Level),
                (key, log) => log.File(new CompactJsonFormatter(), path:
                Path.Combine(_rootPath, $"{key.Item1:yyyy-MM-dd}/{key.Item2}-.txt"),
                fileSizeLimitBytes: fileSizeLimitBytes, rollOnFileSizeLimit: rollOnFileSizeLimit, rollingInterval: (RollingInterval)rollingInterval, retainedFileCountLimit: null, flushToDiskInterval: TimeSpan.FromSeconds(flushToDiskInterval)),
                    sinkMapCountLimit: sinkMapCountLimit,restrictedToMinimumLevel: (LogEventLevel)restrictedToMinimumLevel);
        }
    }
}
