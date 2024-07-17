#region Using ...

using Microsoft.Extensions.Configuration;

using Serilog;
using Serilog.Events;
using Serilog.Settings.Configuration;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

#endregion


namespace Task.Business.Logger
{

    public class LoggerService : ILoggerService
    {
        #region Data Members
        private readonly string _rootPath = "logs\\";
        private readonly ILogger _logger;
        private  IConfiguration _appConfiguration;


        #endregion

        #region Constructors
       
        public LoggerService( IConfiguration appConfiguration)
        {
            this._appConfiguration = appConfiguration;

            _logger = new LoggerConfiguration().
                ReadFrom.Configuration(_appConfiguration, new ConfigurationReaderOptions { SectionName = "Serilog" })
                          
                            .CreateLogger();
            _logger.Information("startlogging");
            _logger.Debug("startlogging");
            _logger.Error("startlogging");
            _logger.Warning("startlogging");



        }
        #endregion

        #region ILoggerService
        
        public void LogError(string content,Exception ex, params object?[]? propertyValues)
        {
            _logger.Error(ex,content, propertyValues);
        }
        
        public void LogInfo(string content,  params object?[]? propertyValues)
        {
            _logger.Information( content, propertyValues);
        }
      

       
        public void LogWarning(string content, params object?[]? propertyValues)
        {
            _logger.Warning(content, propertyValues);
        }


        #endregion
    }
}
