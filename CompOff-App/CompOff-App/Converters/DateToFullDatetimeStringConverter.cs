using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompOff_App.Converters;

public class DateToFullDatetimeStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateTime dateTime = (DateTime)value;
        return dateTime.ToString("dd MMMM yyyy HH:mm", CultureInfo.CreateSpecificCulture("en-GB"));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
