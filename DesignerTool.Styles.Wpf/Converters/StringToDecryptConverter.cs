using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using DesignerTool.Common.Utils;

namespace DesignerTool.Styles.Wpf.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class StringToDecryptConverter : IValueConverter
    {
        /// <summary>
        /// Converts Encrypted String to Plain Text.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string password = "filterniels";
            if (parameter != null)
            {
                password = parameter.ToString();
            }

            try
            {
                return value.ToString().Decrypt(password);
            }
            catch
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Convert back to the Encrypted string from plain text
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string password = "filterniels";
            if (parameter != null)
            {
                password = parameter.ToString();
            }

            try
            {
                return value.ToString().Encrypt(password);
            }
            catch
            {
                return value.ToString();
            }
        }
    }

}
