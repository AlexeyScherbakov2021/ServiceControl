using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterRT : RegisterInt
    {
        private DateTime _RealTimeValue;
        public DateTime RealTimeValue { get => _RealTimeValue; set { Set(ref _RealTimeValue, value); }  }
        private new long? Value;

        public override void SetResultValues(ushort[] val)
        {
            if (val == null)
            {
                Value = null;
                return;
            }

            long res = (ushort)val[0];

            if (val.Length == 2)
            {
                for (int i = 1; i < val.Length; i++)
                {
                    res <<= 16;
                    res |= val[i];
                }
                Value = res;
            }
            else if (val.Length == 4)
            {
                for (int i = 1; i < val.Length; i++)
                {
                    long res2 = val[i];
                    res2 <<= 16 * i;
                    res |= res2;
                }
                Value = res;
            }
            else
            {
                Value = null;
                return;
            }

            try
            {
                RealTimeValue = new DateTime(1970, 01, 01).AddSeconds(Value.Value);
            }
            catch
            {

            }

            ValueString = RealTimeValue.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public override ushort[] SetOutput()
        {
            //ushort[] val = new ushort[Size];
            //RealTimeValue = DateTime.Now.AddMonths(-1);
            TimeSpan span = RealTimeValue.Subtract(new DateTime(1970, 01, 01));

            ushort[] res = new ushort[Size];
            long val = (long)span.TotalSeconds;

            if (Size == 2)
            {
                res[0] = (ushort)(val >> 16);
                res[1] = (ushort)(val );
            }

            else if (Size == 4)
            {
                for (int i = 0; i < Size; i++)
                {
                    res[i] = (ushort)val;
                    val >>= 16;
                }
            }
            return res;
        }

    }
}
