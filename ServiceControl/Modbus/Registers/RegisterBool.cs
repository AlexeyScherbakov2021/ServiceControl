using ServiceControl.Based;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterBool : RegisterBase
    {
        private string _ResultText0;
        public string ResultText0 { get => _ResultText0; set { Set(ref _ResultText0, value); } }
        public string ResultText0Res;

        private string _ResultText1;
        public string ResultText1 { get => _ResultText1; set { Set(ref _ResultText1, value); } }
        public string ResultText1Res;

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

        //-------------------------------------------------------------
        // Изменение языка
        //-------------------------------------------------------------
        public override void SetLanguage()
        {
            base.SetLanguage();

            string name;
            if (!string.IsNullOrEmpty(ResultText0Res))
            {
                name = App.Current.Resources[ResultText0Res]?.ToString();
                if (!string.IsNullOrEmpty(name))
                    ResultText0 = name;
            }

            if (!string.IsNullOrEmpty(ResultText1Res))
            {
                name = App.Current.Resources[ResultText1Res]?.ToString();
                if (!string.IsNullOrEmpty(name))
                    ResultText1 = name;
            }
        }
    }


}
