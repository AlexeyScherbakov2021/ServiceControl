using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterRT : RegisterInt
    {

        public override void SetResultValues(ushort[] val)
        {
            base.SetResultValues(val);
            long sek = (long)ValueInt.Value * 10000000;
            DateTime dt = new DateTime(1970, 01, 01).AddSeconds(ValueInt.Value);
            ValueString = dt.ToString("dd.MM.yyyy HH:mm.ss");
        }

    }
}
