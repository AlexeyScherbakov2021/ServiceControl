using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterTime : RegisterInt
    {
        public override void SetResultValues(ushort[] val)
        {
            if (val == null)
            {
                Value = null;
                return;
            }

            base.SetResultValues(val);
            ValueString = $"{Value/3600:0.##}:{(Value%3600)/60:D2}:{(Value%3600)%60:D2}";

        }

        public override ushort[] SetOutput()
        {
            return base.SetOutput();
        }

    }
}
