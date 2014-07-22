using DesignerTool.Common.Logging;
using DesignerTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool
{
    public static class SessionContext
    {
        public static User LoggedInUser { get; set; }
    }
}
