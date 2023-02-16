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
    internal class StatusDKConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageAwesome img = null;

            if(value != null)
            {
                img = new ImageAwesome();
                bool status = (bool)value;

                if(status)
                {
                    img.Icon = EFontAwesomeIcon.Solid_Circle;
                    img.ToolTip = "замкнут";
                    img.Foreground = Brushes.SpringGreen;

                }
                else
                {
                    img.Icon = EFontAwesomeIcon.Solid_ExclamationTriangle;
                    img.ToolTip = "разрыв";
                    img.Foreground = Brushes.OrangeRed;

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
