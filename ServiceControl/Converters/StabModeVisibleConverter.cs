using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ServiceControl.Converters
{
    internal class StabModeVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visible = Visibility.Hidden;

            if (values[0] is RezhStab stab && values[1] is int valStab)
            {
                visible = (int)stab == valStab ? Visibility.Visible : Visibility.Hidden;
            }

            return visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
