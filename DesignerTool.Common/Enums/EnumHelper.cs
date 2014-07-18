using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DesignerTool.Common.Enums
{
    public static class EnumHelper
    {
        public static string GetEnumDisplayName(System.Enum en)
        {
            if (en == null)
            {
                return string.Empty;
            }

            string retVal = string.Empty;
            System.Reflection.FieldInfo fInfo = en.GetType().GetField(en.ToString());

            foreach (DescriptionAttribute displayName in fInfo.GetCustomAttributes(typeof(DescriptionAttribute), false))
            {
                retVal = displayName.Description.Trim();
            }

            return retVal;
        }

        public static ObservableCollection<EnumDisplay<TEnum>> GetDisplayCollection<TEnum>()
        {
            var coll = new ObservableCollection<EnumDisplay<TEnum>>();
            var enumItems = Enum.GetValues(typeof(TEnum)); // Get all the values in that enum
            if (enumItems != null && enumItems.Length > 0)
            {
                foreach(var item in enumItems)
                {
                    // For each enum item, create an EnumDisplay instance and add it to the collection
                    coll.Add(new EnumDisplay<TEnum>((TEnum)item));
                }
            }

            return coll;
        }
    }
}
