using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Logging
{
    public abstract class BaseLogger : ILogger
    {
        public abstract void Log(string message);
        public abstract void Log(Exception ex);
        public abstract void Log(string message, Exception ex);

        public string AppendExtraInfo(string message)
        {
            return String.Format("<LogItem>{0}<Message>{0}{1}</Message>{0}<DateTime>{2}</DateTime>{0}</LogItem>{0}", Environment.NewLine, message, DateTime.Now.ToFileTimeUtc());
        }
    }
}
