using ServiceControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterBool : RegisterBase
    {
        public string ResultText0 { get; set; }
        public string ResultText1 { get; set; }

        private bool _ValueBool;
        public bool ValueBool { get => _ValueBool; set { Set(ref _ValueBool, value); } }


        public void SetResultValues(bool[] val)
        {
            ValueBool = val[0] != false;
            ValueString = ValueBool ? ResultText1 : ResultText0;
        }

        public bool SetOutput()
        {
            return ValueBool;
        }
    }


}
