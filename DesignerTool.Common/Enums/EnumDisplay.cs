using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.Common.Enums
{
    public class EnumDisplay<T>
    {
        #region Constructors

        public EnumDisplay(T value)
        {
            this.Value = value;
            this.Display = EnumHelper.GetEnumDisplayName((Enum)(object)value);
        }

        #endregion

        #region Properties

        public T Value { get; set; }
        public string Display { get; set; }

        #endregion
    }
}
