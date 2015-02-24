using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.AppLogic.Security
{
    public interface IPermission
    {
        Type PermissionType { get; }

        bool CanRead { get; }
        bool CanModify { get; }
        bool CanDelete { get; }
    }
}
