using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    public enum StatusDK { Norma, Fail };
    public enum StatusDoor { Closed, Opened };
    public enum StatusPower { Norma, Absent, Lower, Upper };

    internal class RegisterStatus131 : RegisterInt
    {
        ushort valueStat;

        public StatusDK DK1 { get; set; }
        
        private string _NameDK1;
        public string NameDK1 { get => _NameDK1; set { Set(ref _NameDK1, value); } }
        
        private string _ValueDK1String;
        public string ValueDK1String { get => _ValueDK1String; set { Set(ref _ValueDK1String, value); } }

        public StatusDK DK2 { get; set; }
        
        private string _NameDK2;
        public string NameDK2 { get => _NameDK2; set { Set(ref _NameDK2, value); } }

        private string _ValueDK2String;
        public string ValueDK2String { get => _ValueDK2String; set { Set(ref _ValueDK2String, value); } }

        public StatusDK DK3 { get; set; }

        private string _NameDK3;
        public string NameDK3 { get => _NameDK3; set { Set(ref _NameDK3, value); } }

        private string _ValueDK3String;
        public string ValueDK3String { get => _ValueDK3String; set { Set(ref _ValueDK3String, value); } }

        public StatusDoor IsOpenedDoor { get; set; }
        
        private string _NameDoor;
        public string NameDoor { get => _NameDoor; set { Set(ref _NameDoor, value); } }

        private string _ValueDoorString;
        public string ValueDoorString { get => _ValueDoorString; set { Set(ref _ValueDoorString, value); } }

        public StatusPower Power { get; set; }
        
        private string _NamePower;
        public string NamePower { get => _NamePower; set { Set(ref _NamePower, value); } }

        private string _ValuePowerString;
        public string ValuePowerString { get => _ValuePowerString; set { Set(ref _ValuePowerString, value); } }

        public override void SetResultValues(ushort[] val)
        {
            if (val.Length < 1) return;
            valueStat = val[0];

            setValueString();

        }

        private void setValueString()
        {

            DK1 = (StatusDK)(valueStat & 0b1);
            ValueDK1String = DK1 == StatusDK.Norma
                ? App.Current?.Resources["Norm"]?.ToString()
                : App.Current?.Resources["Break"]?.ToString();

            DK2 = (StatusDK)(valueStat & 0b10);
            ValueDK2String = DK2 == StatusDK.Norma
                ? App.Current?.Resources["Norm"]?.ToString()
                : App.Current?.Resources["Break"]?.ToString();

            DK3 = (StatusDK)(valueStat & 0b100);
            ValueDK3String = DK3 == StatusDK.Norma
                ? App.Current?.Resources["Norm"]?.ToString()
                : App.Current?.Resources["Break"]?.ToString();

            IsOpenedDoor = (StatusDoor)((valueStat >> 3) & 1);
            ValueDoorString = IsOpenedDoor == StatusDoor.Opened 
                ? App.Current?.Resources["Opened"]?.ToString()
                : App.Current?.Resources["NotOpened"]?.ToString();

            Power = (StatusPower)((valueStat >> 4) & 0b11);
            switch (Power)
            {
                case StatusPower.Norma:
                    ValuePowerString = App.Current?.Resources["Norm"]?.ToString();
                    break;

                case StatusPower.Absent:
                    ValuePowerString = App.Current?.Resources["Absent"]?.ToString();
                    break;

                case StatusPower.Lower:
                    ValuePowerString = App.Current?.Resources["Lower"]?.ToString();
                    break;

                case StatusPower.Upper:
                    ValuePowerString = App.Current?.Resources["Upper"]?.ToString();
                    break;
            }

        }


        //-------------------------------------------------------------
        // Изменение языка
        //-------------------------------------------------------------
        public override void SetLanguage()
        {
            base.SetLanguage();
            NameDK1 = App.Current?.Resources["ResistPast1"]?.ToString();
            NameDK2 = App.Current?.Resources["ResistPast2"]?.ToString();
            NameDK3 = App.Current?.Resources["ResistPast3"]?.ToString();
            NameDoor = App.Current?.Resources["Door"]?.ToString();
            NamePower = App.Current?.Resources["StatusVoltage"]?.ToString();

            setValueString();

            //App.Current.Resources["ByCurrent"]?.ToString()
        }

    }
}
