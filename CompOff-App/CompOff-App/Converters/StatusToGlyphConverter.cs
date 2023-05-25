using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompOff_App.Converters
{
    public class StatusToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            JobStatus status = (JobStatus)value;
            return status switch
            {
                JobStatus.Waiting => "\uef64",
                JobStatus.InQueue => "\ue8b5",
                JobStatus.Running => "\uef6f",
                JobStatus.Cancelled => "\ue5c9",
                JobStatus.Done => "\ue86c",
                JobStatus.Paused => "\ue1a2",
                _ => "\uef64",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
