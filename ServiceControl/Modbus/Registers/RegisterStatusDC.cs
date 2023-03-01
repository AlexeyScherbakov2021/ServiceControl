using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    public enum StatusDC { Norma, Off, Absent, Avar };

    internal class RegisterStatusDC : RegisterInt
    {
        public StatusDC[] ValueStat { get; set; } = new StatusDC[8];

        public override void SetResultValues(ushort[] val)
        {
            if (val.Length < 1) return;
            ushort valueStat = val[0];

            for (int i = 0; i < 8; i++)
            {
                ValueStat[i] = (StatusDC)(valueStat & 0b11);
                valueStat >>= 2;
            }
        }

    }
}
