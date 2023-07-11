using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HT.Views.Converters
{
    internal class ConverterNumber : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var valor = Convert.ToDouble(value) % 1 == 0 ? value : string.Format(CultureInfo.CurrentCulture, "{0:N}", value);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
