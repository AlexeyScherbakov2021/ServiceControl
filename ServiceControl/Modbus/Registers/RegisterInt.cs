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
        public int? ValueInt { get => _ValueInt; set { Set(ref _ValueInt, value); } }


        public override void SetResultValues(ushort[] val)
        {
            if (val == null || val.Length < 1)
            {
                ValueInt = null;
                return;
            }
            int res = (short)val[0];

            for (int i = 1; i < val.Length; i++)
            {
                int res2 = val[i];
                res2 <<= 16 * i;
                res |= res2;
            }
            ValueInt = res;
        }

        public override ushort[] SetOutput()
        {
            ushort[] res = new ushort[Size]; 
            int val = ValueInt.Value;

            for(int i = 0; i < Size; i++)
            {
                res[i] = (ushort)val;
                val >>= 16;
            }

            return res;

            //return (ushort)ValueInt;
        }
    }
}
