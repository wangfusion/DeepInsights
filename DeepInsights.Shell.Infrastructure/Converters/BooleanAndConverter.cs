using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace DeepInsights.Shell.Infrastructure.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanAndConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return values.OfType<IConvertible>().All(System.Convert.ToBoolean);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo cultureInfo)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
