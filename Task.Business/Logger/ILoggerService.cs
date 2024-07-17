
using System;
using System.Collections.Generic;
using System.Text;


namespace Task.Business.Logger
{
	
	public interface ILoggerService
	{
		
		
        void LogInfo(string content, params object?[]? propertyValues);

		void LogError(string content, Exception ex, params object?[]? propertyValues);




        void LogWarning(string content, params object?[]? propertyValues);
    }
}
