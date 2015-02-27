using DesignerTool.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Logging
{
    public static class Logger
    {
        private static ILogger _currentLogger;
        public static ILogger CurrentLogger
        {
            get
            {
                if (_currentLogger == null)
                {
                    _currentLogger = createLogger();
                }
                return _currentLogger;
            }
        }

        public static void Log(string message)
        {
            CurrentLogger.Log(message);
        }

        public static void Log(string message, Exception ex)
        {
            CurrentLogger.Log(message, ex);
        }

        public static void Log(Exception ex)
        {
            CurrentLogger.Log(ex);
        }

        private static ILogger createLogger()
        {
            return new FileLogger();
        }
    }
}
