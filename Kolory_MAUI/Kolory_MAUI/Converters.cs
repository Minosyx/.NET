using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolory_MAUI
{
    public class DoubleToByteString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dvalue = (double)value;
            return Math.Round(255 * dvalue).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RGBToBrush : IMultiValueConverter
    {
        private SolidColorBrush brush = new SolidColorBrush();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(x => x == null)) return Brush.Black;
            double r = (double)values[0];
            double g = (double)values[1];
            double b = (double)values[2];
            brush.Color = Color.FromRgb(r, g, b);
            return brush.Color;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DoubleToByte : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dvalue = (double)value;
            return (byte)Math.Round(255 * dvalue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte dvalue = (byte)value;
            return (double)dvalue;
        }
    }

}
