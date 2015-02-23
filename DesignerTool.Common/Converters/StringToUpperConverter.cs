using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DesignerTool.Common.Mvvm.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class StringToUpperConverter : IValueConverter
    {
        /// <summary>
        /// Converts string to upper case
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value as string;
            return val != null ? val.ToUpper() : value;
        }

        /// <summary>
        /// Convert back, but its not implemented as it's not yet needed
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
