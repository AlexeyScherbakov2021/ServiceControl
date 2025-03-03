using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterInt64 : Register
    {
        public long MinValue = long.MinValue;
        public long MaxValue = long.MaxValue;
        public bool isNeg = true;

        private long? _Value;
        public long? Value 
        { 
            get => _Value; 
            set 
            {
                Set(ref _Value, value);
            } 
        }


        public override void SetResultValues(ushort[] val)
        {
            if (val == null || val.Length < 1)
            {
                Value = null;
                return;
            }

            int res = (short)val[0];
            for (int i = 1; i < val.Length; i++)
            {
                int res2 = val[i];
                res2 <<= 16 * i;
                res |= res2;
            }
            if(isNeg)
                Value =  (int)res;
            else
                Value = (uint)res;

            //}
            //else
            //{
            //    uint res = (ushort)val[0];
            //    for (int i = 1; i < val.Length; i++)
            //    {
            //        uint res2 = val[i];
            //        res2 <<= 16 * i;
            //        res |= res2;
            //    }
            //    Value = (int)(uint)res;
            //    ValuePos = (uint)Value;
            //}

            if (Value > MaxValue || Value < MinValue) Value = null;
            ValueString = $"{Value}";

        }

        public override ushort[] SetOutput()
        {
            ushort[] res = null;
            if (Value != null)
            {
                res = new ushort[Size];
                int val = (int)Value.Value;

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
