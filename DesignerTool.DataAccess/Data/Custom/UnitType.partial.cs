using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Data
{
    public partial class UnitType : BaseModel
    {
        public static UnitType New()
        {
            // Set all the defaults.
            UnitType newUnitType = new UnitType();
            newUnitType.IsActive = true;

            return newUnitType;
        }
    }
}
