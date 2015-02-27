using DesignerTool.Common.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Logging
{
    public class FileLogger : BaseLogger
    {
        private string logFileName
        {
            get
            {
                return Path.Combine(ApplicationPaths.Log, String.Format("{0:yyyyMMdd}.log", DateTime.Today));
            }
        }

        public override void Log(string message)
        {
            try
            {
                File.AppendAllText(this.logFileName, base.AppendExtraInfo(message));
            }
            catch (Exception)
            {
                // TODO: Event Log
            }
        }

        public override void Log(Exception ex)
        {
            this.Log(String.Empty, ex);
        }

        public override void Log(string message, Exception ex)
        {
            if(!string.IsNullOrWhiteSpace(message))
            {
                message += ": ";
            }

            // Exception Message
            StringBuilder exceptionBuilder = new StringBuilder();
            exceptionBuilder.AppendLine(String.Format("<Exception>{0}: {1}</Exception>", message, ex.Message));

            // Inner Exceptions
            Exception innerEx = ex.InnerException;
            int count = 1;
            while (innerEx != null && count < 5)
            {
                exceptionBuilder.AppendLine(String.Format("<InnerException{0}>{1}</InnerException{0}>", count++, innerEx.Message));
                innerEx = innerEx.InnerException;
            }

            exceptionBuilder.AppendLine(String.Format("<StackTrace>{0}</StackTrace>", ex.StackTrace));

            this.Log(exceptionBuilder.ToString());
        }
    }
}
