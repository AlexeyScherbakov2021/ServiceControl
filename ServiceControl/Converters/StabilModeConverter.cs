using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ServiceControl.Converters
{
    internal class StabilModeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brush = null;

            if( values[0] is RezhStab stab && values[1] is int valStab)
            {
                ushort addr = (ushort)values[2];

                if (addr == 0x10)
                {
                    brush = (int)stab == valStab ? Brushes.SpringGreen : Brushes.Gray;
                }
                if(addr == 0x84)
                {
                    brush = (int)stab == valStab ? Brushes.YellowGreen : Brushes.Transparent;
                }

            }

            return brush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
