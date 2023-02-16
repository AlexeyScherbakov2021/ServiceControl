using FontAwesome5;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ServiceControl.Converters
{
    internal class StatusMSConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageAwesome img = null;

            //BitmapImage Source = null;

            if(value != null)
            {
                img = new ImageAwesome();
                StatusMS status = (StatusMS)value;
                
            //if (value is StatusMS status)
            //{

                switch (status)
                {
                    case StatusMS.On:
                        img.Icon = EFontAwesomeIcon.Solid_Circle;
                        img.ToolTip = "Включен";
                        img.Foreground = Brushes.SpringGreen;

                        //Source = new BitmapImage(new Uri("/ServiceControl;component/Resources/greenLed.png", UriKind.Relative));
                        break;

                    case StatusMS.Off:
                        img.Icon = EFontAwesomeIcon.Solid_Circle;
                        img.ToolTip = "Выключен";
                        img.Foreground = Brushes.Gray;
                        //Source = new BitmapImage(new Uri("/ServiceControl;component/Resources/blackLed.png", UriKind.Relative));
                        break;

                    case StatusMS.Absent:
                        img.Icon = EFontAwesomeIcon.Solid_Times;
                        img.ToolTip = "Отсутствует";
                        img.Foreground = Brushes.LightSalmon;
                        //Source = new BitmapImage(new Uri("/ServiceControl;component/Resources/Absent.png", UriKind.Relative));
                        break;

                    case StatusMS.Avar:
                        img.Icon = EFontAwesomeIcon.Solid_Exclamation;
                        img.ToolTip = "Авария";
                        img.Foreground = Brushes.Red;
                        //Source = new BitmapImage(new Uri("/ServiceControl;component/Resources/attention.png", UriKind.Relative));
                        break;
                }

            }

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
