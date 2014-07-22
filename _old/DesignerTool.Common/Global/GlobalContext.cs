using DesignerTool.Common.Logging;
using DesignerTool.Common.Mvvm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Global
{
    public static class GlobalContext
    {
        public static ILogger Logger
        {
            get
            {
                return ServiceLocator.Resolve<ILogger>();
            }
        }
    }
}
