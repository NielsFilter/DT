using DesignerTool.Common.Enums;
using DesignerTool.Common.Global;
using DesignerTool.Common.Licensing;
using DesignerTool.Common.Logging;
using DesignerTool.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignerTool.DataAccess.Data
{
    public partial class License : BaseModel
    {
        public string LicenseDisplay { get; set; }
        public DateTime ExpiryDate { get; set; }
        public LicenseStateTypes State { get; set; }
    }
}
