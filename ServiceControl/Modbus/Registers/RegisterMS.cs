using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterMS : UshortRegister
    {
        private string _ValueString;
        public string ValueString { get => _ValueString; set { Set(ref _ValueString, value); } }


        public override ushort GetResult(ushort[] val)
        {
            RezhStab result = (RezhStab)base.GetResult(val);

            switch (result)
            {
                case RezhStab.StabCurrent:
                    ValueString = "00 - включен";
                    break;

                case RezhStab.StabSummPot:
                    ValueString = "01 - выключен";
                    break;

                case RezhStab.StabPolPot:
                    ValueString = "02 - отсутствует";
                    break;

                case RezhStab.StabNapr:
                    ValueString = "03 - авария";
                    break;
            }

            return (ushort)result;
        }

    }
}
