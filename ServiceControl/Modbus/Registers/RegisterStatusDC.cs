using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    public enum StatusDC { Norma, Off, Absent, Avar };

    public class ClassStatus
    {
        public int Number { get; set;}
        public StatusDC StatusDC { get; set;}
    }



    internal class RegisterStatusDC : RegisterInt
    {
        public ClassStatus[] ValueStat { get; set; }

        public RegisterStatusDC(int StartNum = 1)
        {
            ValueStat = new ClassStatus[8];
            for (int i = 0; i < 8; i++)
            {
                ValueStat[i] = new ClassStatus() { Number = i + StartNum };
            }
        }


        public override void SetResultValues(ushort[] val)
        {
            if (val.Length < 1) return;
            ushort valueStat = val[0];

            for (int i = 0; i < 8; i++)
            {
                ValueStat[i].StatusDC = (StatusDC)(valueStat & 0b11);
                valueStat >>= 2;
            }
        }

    }
}
