using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    public enum RezhStab { StabCurrent, StabSummPot, StabPolPot, StabNapr };

    internal class RegisterStab : RegisterInt
    {
        private bool _IsCurrentStab;
        public bool IsCurrentStab { get => _IsCurrentStab; 
            set 
            {
                if (Set(ref _IsCurrentStab, value) && value)
                    Value = (ushort)RezhStab.StabCurrent;
            } 
        }

        private bool _IsSummPotStab;
        public bool IsSummPotStab { get => _IsSummPotStab; 
            set 
            {
                if (Set(ref _IsSummPotStab, value) && value)
                    Value = (ushort)RezhStab.StabSummPot;
            } 
        }

        private bool _IsPolPotStab;
        public bool IsPolPotStab { get => _IsPolPotStab; 
            set 
            { 
                if(Set(ref _IsPolPotStab, value) && value)
                    Value = (ushort)RezhStab.StabPolPot;
            }
        }

        private bool _IsNaprStab;
        public bool IsNaprStab { get => _IsNaprStab; 
            set 
            { 
                if(Set(ref _IsNaprStab, value) && value)
                    Value = (ushort)RezhStab.StabNapr;
            }
        }


        public override void SetResultValues(ushort[] val)
        {
            base.SetResultValues(val);

            if (Value == null) return;

            switch((RezhStab)Value)
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
        }

    }
}
