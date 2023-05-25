using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompOff_App.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            JobStatus status = (JobStatus)value;
            return status switch
            {
                JobStatus.Waiting => Color.FromRgba("#FAD116"),
                JobStatus.InQueue => Color.FromRgba("#4BB4DE"),
                JobStatus.Running => Color.FromRgba("#345DA7"),
                JobStatus.Cancelled => Color.FromRgba("#FF0000"),
                JobStatus.Done => Color.FromRgba("#00DE00"),
                JobStatus.Paused => Color.FromRgba("#FF9933"),
                _ => Color.FromRgba("#F1EAE6"),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
