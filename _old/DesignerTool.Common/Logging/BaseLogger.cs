using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Logging
{
    public abstract class BaseLogger : ILogger
    {
        public abstract void Log(string message);

        public string AppendExtraInfo(string message)
        {
            return String.Format("{0}{1}DateTime: {2}", message, Environment.NewLine, DateTime.Now.ToFileTimeUtc());
        }
    }
}
