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

        public StatusDK DK1 { get; set; }
        public string NameDK1 { get; set; } = "Пластина 1";
        public string ValueDK1String { get; set; }

        public StatusDK DK2 { get; set; }
        public string NameDK2 { get; set; } = "Пластина 2";
        public string ValueDK2String { get; set; }

        public StatusDK DK3 { get; set; }
        public string NameDK3 { get; set; } = "Пластина 3";
        public string ValueDK3String { get; set; }

        public StatusDoor IsOpenedDoor { get; set; }
        public string NameDoor { get; set; } = "Датчик вскрытия шкафа";
        public string ValueDoorString { get; set; }

        public StatusPower Power { get; set; }
        public string NamePower { get; set; } = "Состояние сети ~220В";
        public string ValuePowerString { get; set; }


        public override void SetResultValues(ushort[] val)
        {
            if (val.Length < 1) return;
            ushort valueStat = val[0];

            DK1 = (StatusDK)(valueStat & 0b1);
            ValueDK1String = DK1 == StatusDK.Norma ? "в норме" : "не норма";

            DK2 = (StatusDK)(valueStat & 0b10);
            ValueDK2String = DK2 == StatusDK.Norma ? "в норме" : "не норма";

            DK3 = (StatusDK)(valueStat & 0b100);
            ValueDK3String = DK3 == StatusDK.Norma ? "в норме" : "не норма";

            IsOpenedDoor = (StatusDoor)((valueStat >> 3) & 1);
            ValueDoorString = IsOpenedDoor == StatusDoor.Opened ? "шкаф вскрыт" : "шкаф закрыт";

            Power = (StatusPower)((valueStat >> 4) & 0b11);
            switch(Power)
            {
                case StatusPower.Norma:
                    ValuePowerString = "в норме";
                    break;

                case StatusPower.Absent:
                    ValuePowerString = "отсутствует";
                    break;

                case StatusPower.Lower:
                    ValuePowerString = "понижено";
                    break;

                case StatusPower.Upper:
                    ValuePowerString = "повышено";
                    break;
            }

        }

    }
}
