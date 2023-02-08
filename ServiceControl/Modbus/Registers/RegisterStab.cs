using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    public enum RezhStab { StabCurrent, StabSummPot, StabPolPot, StabNapr };

    internal class RegisterStab : UshortRegister
    {
        private bool _IsCurrentStab;
        public bool IsCurrentStab { get => _IsCurrentStab; set { Set(ref _IsCurrentStab, value); } }

        private bool _IsSummPotStab;
        public bool IsSummPotStab { get => _IsSummPotStab; set { Set(ref _IsSummPotStab, value); } }

        private bool _IsPolPotStab;
        public bool IsPolPotStab { get => _IsPolPotStab; set { Set(ref _IsPolPotStab, value); } }

        private bool _IsNaprStab;
        public bool IsNaprStab { get => _IsNaprStab; set { Set(ref _IsNaprStab, value); } }


        private string _ValueString;
        public string ValueString { get => _ValueString; set { Set(ref _ValueString, value); } }

        public override ushort GetResult(ushort[] val)
        {
            RezhStab result = (RezhStab)base.GetResult(val);

            switch(result)
            {
                case RezhStab.StabCurrent:
                    ValueString = "00 - стабилизация тока";
                    IsCurrentStab = true;
                    break;

                case RezhStab.StabSummPot:
                    ValueString = "01 - стабилизация сумм.потенциала";
                    IsSummPotStab = true;
                    break;

                case RezhStab.StabPolPot:
                    ValueString = "02 - стабилизация поляр.потенциала";
                    IsPolPotStab = true;
                    break;

                case RezhStab.StabNapr:
                    ValueString = "03 - стабилизация напряжения";
                    IsNaprStab = true;
                    break;
            }

            return (ushort)result;
        }

    }
}
