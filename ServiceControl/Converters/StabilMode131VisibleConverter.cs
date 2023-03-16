using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ServiceControl.Converters
{
    internal class StabilMode131VisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visible = Visibility.Hidden;

            if (values[0] is RezhStab stab && values[1] is ModeStab valStab)
            {
                visible = (ModeStab)stab == valStab ? Visibility.Visible : Visibility.Hidden;
            }

            return visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
