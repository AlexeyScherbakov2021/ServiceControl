using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class BoolRegister : Register<bool, bool>
    {
        public string ResultText0;
        public string ResultText1;

        public override bool GetResult(bool[] val)
        {
            bool res = val[0] != false;
            return res;
        }

        public override ushort SetUshort()
        {
            ushort result = (ushort) (Value ? 1 : 0);
            return result;
        }
    }
}
