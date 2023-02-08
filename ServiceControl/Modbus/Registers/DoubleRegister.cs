using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ServiceControl.Modbus.Registers
{
    internal class DoubleRegister : Register<ushort, double>
    {
        public int MinValue = int.MinValue;
        public int MaxValue = int.MaxValue;
        public double Scale;

        public override double GetResult(ushort[] val)
        {
            if (val.Length < 1) return double.NaN;

            int res = (short)val[0];
            for(int i = 1; i < val.Length; i++)
            {
                res <<= 16;
                res |= (int)(short)val[i];
            }
            Value = (int)res * Scale;
            return Value;
        }

        public override ushort SetUshort()
        {
            ushort result = (ushort)(Value / Scale);
            return result;
        }

    }
}
