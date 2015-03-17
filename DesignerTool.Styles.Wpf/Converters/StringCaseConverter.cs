using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DesignerTool.Styles.Wpf.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class StringCaseConverter : IValueConverter
    {
        /// <summary>
        /// Converts String To Upper or Lower case.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool toUpper = true; // Default is upper case
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out toUpper);
            }

            if (value != null && value.ToString().Trim() != string.Empty)
            {
                if (toUpper)
                {
                    // Upper Case
                    return value.ToString().ToUpper(culture);
                }
                else
                {
                    // Lower Case
                    return value.ToString().ToLower(culture);
                }
            }
            return value;
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
