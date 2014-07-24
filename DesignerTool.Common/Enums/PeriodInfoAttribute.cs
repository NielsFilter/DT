using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignerTool.Common.Enums
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public sealed class PeriodInfoAttribute : Attribute
    {
        public PeriodInfoAttribute(string display, string plural, string addPeriodMethod)
        {
            this.Display = display;
            this.Plural = plural;
            this.AddPeriodMethod = addPeriodMethod;
        }

        public string Display { get; private set; }
        public string Plural { get; private set; }
        public string AddPeriodMethod { get; private set; }

        #region Public Static Methods

        public static PeriodInfoAttribute GetAttribute(PeriodType periodType)
        {
            var mi = periodType.GetType().GetMember(periodType.ToString());
            if (mi != null && mi.Length > 0)
            {
                var attr = Attribute.GetCustomAttribute(mi[0], typeof(PeriodInfoAttribute));
                if (attr != null)
                {
                    return (PeriodInfoAttribute)attr;
                }
            }
            return null;
        }

        #endregion
    }
}
