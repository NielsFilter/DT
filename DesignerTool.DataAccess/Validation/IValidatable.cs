using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Validation
{
    public interface IValidatable : IDataErrorInfo
    {
        List<string> ValidateAll();
    }
}
