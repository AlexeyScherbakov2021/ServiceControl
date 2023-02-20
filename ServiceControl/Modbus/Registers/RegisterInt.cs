using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterInt : Register
    {
        public int MinValue = int.MinValue;
        public int MaxValue = int.MaxValue;

        private int? _ValueInt;
        public int? Value { get => _ValueInt; set { Set(ref _ValueInt, value); } }


        public override void SetResultValues(ushort[] val)
        {
            if (val == null || val.Length < 1)
            {
                Value = null;
                return;
            }
            uint res = (ushort)val[0];

            for (int i = 1; i < val.Length; i++)
            {
                uint res2 = val[i];
                res2 <<= 16 * i;
                res |= res2;
            }
            Value = (int)res;
            if (Value > MaxValue || Value < MinValue) Value = null;

        }

        public override ushort[] SetOutput()
        {
            ushort[] res = new ushort[Size];
            if (Value != null)
            {
                int val = Value.Value;

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
