using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class UshortRegister : Register<ushort, ushort>
    {
        public ushort MinValue = ushort.MinValue;
        public ushort MaxValue = ushort.MaxValue;

        public override ushort GetResult(ushort[] val)
        {
            if (val.Length < 1) return 0;
            Value = val[0];
            return Value;

        }

        public override ushort SetUshort()
        {
            return Value;
        }
    }
}
