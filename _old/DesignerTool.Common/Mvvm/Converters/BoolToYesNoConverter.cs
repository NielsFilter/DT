using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DesignerTool.Common.Mvvm.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToYesNoConverter : IValueConverter
    {
        /// <summary>
        /// Converts Boolean to "Yes" or "No" string.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = false; // Default is false
            if (value != null)
            {
                bool.TryParse(value.ToString(), out result);
            }

            bool invert = false;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out invert);
            }

            if (invert)
            {
                // Inverted - True = "No" and False = "Yes"
                return result ? "No" : "Yes";
            }
            else
            {
                // Normal (Not inverted) - True = "Yes" and False = "No"
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
