using System;
using System.Windows.Data;
using System.Windows;

namespace mainHMI.valueconverter
{
    public class Converter_boolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool bSignal = (bool)value;
            object vis;

            if (false == bSignal)
                vis = Visibility.Hidden;
            else
                vis = Visibility.Visible;

            return vis;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
