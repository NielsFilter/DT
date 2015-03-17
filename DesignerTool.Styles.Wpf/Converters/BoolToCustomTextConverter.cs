using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DesignerTool.Styles.Wpf.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToCustomTextConverter : IValueConverter
    {
        /// <summary>
        /// Converts Boolean to custom text passed as a parameter.
        /// The pipe character "|" must be used to split the true and false text representations
        /// </summary>
        /// <remarks>
        /// If no parameter is passed, then "Yes" or "No" is returned.
        /// </remarks>
        /// <example>
        /// If "Active|Inactive" is passed as a parameter.
        /// True: "Active" is returned, if False: "Inactive" is returned.
        /// </example>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = false; // Default is false
            if (value != null)
            {
                bool.TryParse(value.ToString(), out result);
            }

            if (parameter != null && parameter.ToString().Contains("|"))
            {
                var customTextArr = parameter.ToString().Split('|');
                return result ? customTextArr[0] : customTextArr[1];
            }
            else
            {
                // No custom text provided. Default: True = "Yes" and False = "No"
                return result ? "Yes" : "No";
            }
        }

        /// <summary>
        /// Convert back, but its not implemented as it's not yet needed
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
