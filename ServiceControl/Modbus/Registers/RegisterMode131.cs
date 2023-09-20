using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ServiceControl.Modbus.Registers
{
    public enum DistMode { Distance, Manual };
    public enum ModeStab { Current, SummPot, PolPot, Voltage, Wait, LimitCurr };

    internal class RegisterMode131 : RegisterInt
    {
        private DistMode _distMode;
        public DistMode distMode 
        { 
            get => _distMode; 
            set 
            {
                if (Set(ref _distMode, value))
                {
                    DistModeString = distMode == DistMode.Manual
                        ? App.Current?.Resources["Local"]?.ToString()
                        : App.Current?.Resources["Remote"]?.ToString();
                }
            } 
        }

        private string _DistModeName; // = "Режим управления";
        public string DistModeName { get => _DistModeName; set { Set(ref _DistModeName, value); } } 

        private string _DistModeString;
        public string DistModeString { get => _DistModeString; set { Set(ref _DistModeString, value); } }

        private ModeStab _Mode;
        public ModeStab Mode { get => _Mode; set { Set(ref _Mode, value); } }

        public override void SetResultValues(ushort[] val)
        {
            if (val == null || val.Length < 1) return;
            Value = val[0];

            switch(Value)
            {
                case 0x0:
                    distMode = DistMode.Manual;
                    Mode = ModeStab.Current;
                    break;
                case 0x1:
                    distMode = DistMode.Manual;
                    Mode = ModeStab.Voltage;
                    break;
                case 0x2:
                    distMode = DistMode.Manual;
                    Mode = ModeStab.PolPot;
                    break;
                case 0x3:
                    distMode = DistMode.Distance;
                    Mode = ModeStab.Current;
                    break;
                case 0x4:
                    distMode = DistMode.Distance;
                    Mode = ModeStab.Voltage;
                    break;
                case 0x5:
                    distMode = DistMode.Distance;
                    Mode = ModeStab.PolPot;
                    break;
                case 0x6:
                    distMode = DistMode.Manual;
                    Mode = ModeStab.SummPot;
                    break;
                case 0x7:
                    distMode = DistMode.Distance;
                    Mode = ModeStab.SummPot;
                    break;
                case 0x8:
                    distMode = DistMode.Distance;
                    Mode = ModeStab.Wait;
                    break;
                case 0x11:
                    distMode = DistMode.Distance;
                    Mode = ModeStab.LimitCurr;
                    break;
                case 0x12:
                    distMode = DistMode.Manual;
                    Mode = ModeStab.LimitCurr;
                    break;
            }


        }

        public override ushort[] SetOutput()
        {
            ushort[] res = new ushort[1];

            if(distMode == DistMode.Manual)
            {
                switch(Mode)
                {
                    case ModeStab.Current:
                        res[0] = 0x0;
                        break;
                    case ModeStab.Voltage:
                        res[0] = 0x1;
                        break;
                    case ModeStab.PolPot:
                        res[0] = 0x2;
                        break;
                    case ModeStab.SummPot:
                        res[0] = 0x6;
                        break;
                    case ModeStab.Wait:
                        res[0] = 0x8;
                        break;
                    case ModeStab.LimitCurr:
                        res[0] = 0x12;
                        break;
                }
            }
            else
            {
                switch (Mode)
                {
                    case ModeStab.Current:
                        res[0] = 0x3;
                        break;
                    case ModeStab.Voltage:
                        res[0] = 0x4;
                        break;
                    case ModeStab.PolPot:
                        res[0] = 0x5;
                        break;
                    case ModeStab.SummPot:
                        res[0] = 0x7;
                        break;
                    case ModeStab.Wait:
                        res[0] = 0x8;
                        break;
                    case ModeStab.LimitCurr:
                        res[0] = 0x11;
                        break;
                }
            }

            return res;
        }

        public override void SetLanguage()
        {
            base.SetLanguage();
            setValueString();
        }

        private void setValueString()
        {
            DistModeName = App.Current?.Resources["DistControl"]?.ToString();
            DistModeString = distMode == DistMode.Manual 
                ? App.Current?.Resources["Local"]?.ToString() 
                : App.Current?.Resources["Remote"]?.ToString();
        }


    }
}
