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
            if (val == null || val.Length < 4)
            {
                Value = null;
                return;
            }
            long res = (ushort)val[0];

            for (int i = 1; i < val.Length; i++)
            {
                long res2 = val[i];
                res2 <<= 16 * i;
                res |= res2;
            }
            Value = res;

            RealTimeValue = new DateTime(1970, 01, 01).AddSeconds(Value.Value);
            ValueString = RealTimeValue.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public override ushort[] SetOutput()
        {
            //ushort[] val = new ushort[Size];
            //RealTimeValue = DateTime.Now.AddMonths(-1);
            TimeSpan span = RealTimeValue.Subtract(new DateTime(1970, 01, 01));

            ushort[] res = new ushort[Size];
            long val = (long)span.TotalSeconds;

            for (int i = 0; i < Size; i++)
            {
                res[i] = (ushort)val;
                val >>= 16;
            }

            return res;
        }

    }
}
