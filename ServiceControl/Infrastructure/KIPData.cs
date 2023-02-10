using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Infrastructure
{
    internal class KIPData
    {
        public RegisterDouble RegCurrentPolyar { get; set; }
        public RegisterDouble RegPolyarPot { get; set; }
        public RegisterDouble RegSummPot { get; set; }
        public RegisterDouble RegtResistDK1 { get; set; }
        public RegisterDouble RegtResistDK2 { get; set; }
        public RegisterDouble RegtResistDK3 { get; set; }
        public RegisterDouble RegProtectCurrent { get; set; }
        public RegisterDouble RegDeepCorr { get; set; }
        public RegisterDouble RegSpeedCorr { get; set; }
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
