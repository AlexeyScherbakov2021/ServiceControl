using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Based
{
    internal class KIPData
    {
        public RegisterKIP RegCurrentPolyar { get; set; }
        public RegisterKIP RegPolyarPot { get; set; }
        public RegisterKIP RegSummPot { get; set; }
        public RegisterKIP RegtResistDK1 { get; set; }
        public RegisterKIP RegtResistDK2 { get; set; }
        public RegisterKIP RegtResistDK3 { get; set; }
        public RegisterKIP RegProtectCurrent { get; set; }
        public RegisterKIP RegDeepCorr { get; set; }
        public RegisterKIP RegSpeedCorr { get; set; }
        public RegisterStatus RegStatus { get; set; }

        //public int Number { get; set; }
        //public bool IsSensorOpen { get; set; }
        //public BlockStatus StatusBlock { get; set; }
        //public bool IsDT1Break { get; set; }
        //public bool IsDT2Break { get; set; }
        //public bool IsDT3Break { get; set; }
        //public double CurrentPolar { get; set; }
        //public double PolarPot { get; set; }
        //public double SummaryPot { get; set; }
        //public double ResistCorr1 { get; set; }
        //public double ResistCorr2 { get; set; }
        //public double ResistCorr3 { get; set; }
        //public double ProtectCurrent { get; set; }
        //public double DeepCorr { get; set; }
        //public double SpeedCorr { get; set; }
        //public DateTime DeviceTime { get; set; }
    }
}
