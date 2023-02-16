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
        public string CurrentStringValue => _ValueBool ? ResultText1 : ResultText0;
        public bool IsCorrectValue;
       
        private bool _IsAlarm;
        public bool IsAlarm { get => _IsAlarm; set { Set(ref _IsAlarm, value); } }

        private bool _ValueBool;
        public bool ValueBool { get => _ValueBool; set { Set(ref _ValueBool, value); } }


        public void SetResultValues(bool[] val)
        {
            if (val == null) return;
            ValueBool = val[0] != false;
            ValueString = ValueBool ? ResultText1 : ResultText0;
            IsAlarm = ValueBool != IsCorrectValue;
        }

        public bool SetOutput()
        {
            return ValueBool;
        }
    }


}
