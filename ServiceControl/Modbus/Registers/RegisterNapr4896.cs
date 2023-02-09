using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterNapr4896 : UshortRegister
    {
        private string _ValueString;
        public string ValueString { get => _ValueString; set { Set(ref _ValueString, value); } }

        public override ushort GetResult(ushort[] val)
        {
            if (val.Length < 1) return 0;
            Value = val[0];
            ValueString = Value == 0 ? "48 В" : "96 В";
            return Value;
        }

    }
}
