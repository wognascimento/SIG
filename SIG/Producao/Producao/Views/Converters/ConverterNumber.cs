using System;
using System.Globalization;
using System.Windows.Data;

namespace Producao
{
    public class ConverterNumber : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var valorFormatado = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:D}", value);
                return valorFormatado;
            }
            catch (Exception ex)
            {
            }
            return value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
