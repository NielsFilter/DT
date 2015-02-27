using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Logging
{
    public interface ILogger
    {
        void Log(string message);
        void Log(Exception ex);
        void Log(string message, Exception ex);
    }
}
