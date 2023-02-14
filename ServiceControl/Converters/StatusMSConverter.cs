using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ServiceControl.Converters
{
    internal class StatusMSConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage Source = null;

            StatusMS status = (StatusMS)value;

            //if (value is StatusMS status)
            //{

                switch (status)
                {
                    case StatusMS.On:
                        Source = new BitmapImage(new Uri("/ServiceControl;component/Resources/greenLed.png", UriKind.Relative));
                        break;

                    case StatusMS.Off:
                        Source = new BitmapImage(new Uri("/ServiceControl;component/Resources/blackLed.png", UriKind.Relative));
                        break;

                    case StatusMS.Absent:
                        Source = new BitmapImage(new Uri("/ServiceControl;component/Resources/Absent.png", UriKind.Relative));
                        break;

                    case StatusMS.Avar:
                        Source = new BitmapImage(new Uri("/ServiceControl;component/Resources/attention.png", UriKind.Relative));
                        break;
                }

            //}

            return Source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
