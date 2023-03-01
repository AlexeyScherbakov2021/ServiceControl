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
        public StatusDK DK2 { get; set; }
        public StatusDK DK3 { get; set; }
        public StatusDoor IsOpenDoor { get; set; }
        public StatusPower Power { get; set; }


        public override void SetResultValues(ushort[] val)
        {
            if (val.Length < 1) return;
            ushort valueStat = val[0];

            DK1 = (StatusDK)(valueStat & 0b1);
            DK2 = (StatusDK)(valueStat & 0b10);
            DK3 = (StatusDK)(valueStat & 0b100);
            IsOpenDoor = (StatusDoor)(valueStat & 0b1000);
            Power = (StatusPower)((valueStat >> 4) & 0b11);

        }

    }
}
