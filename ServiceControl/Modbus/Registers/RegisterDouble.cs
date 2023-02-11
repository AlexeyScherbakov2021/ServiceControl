using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterDouble : Register
    {
        public double Scale;

        private double? _ValueDouble;
        public double? ValueDouble { get => _ValueDouble; set { Set(ref _ValueDouble, value); } }

        public override void SetResultValues(ushort[] val)
        {
            if (val == null || val.Length < 1)
            {
                ValueDouble = null;
                return;
            }

            int res = (short)val[0];
            for(int i = 1; i < val.Length; i++)
            {
                int res2 = val[i];
                res2 <<= 16 * i;
                res |= (int)(short)val[i];
            }
            ValueDouble = (int)res * Scale;
        }

        public override ushort[] SetOutput()
        {

            ushort[] res = new ushort[Size];
            int val = (int)(ValueDouble / Scale);

            for (int i = Size - 1; i >= 0; i--)
            {
                res[i] = (ushort)val;
                val >>= 16;
            }

            return res;

            //ushort result = (ushort)(ValueDouble / Scale);
            //return result;
        }

    }
}
