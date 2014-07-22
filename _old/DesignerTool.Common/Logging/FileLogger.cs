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
                return Path.Combine(PathContext.Log, String.Format("{0:yyyyMMdd}.log", DateTime.Today));
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
    }
}
