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
        public DoubleRegister RegCurrentPolyar { get; set; }
        public DoubleRegister RegPolyarPot { get; set; }
        public DoubleRegister RegSummPot { get; set; }
        public DoubleRegister RegtResistDK1 { get; set; }
        public DoubleRegister RegtResistDK2 { get; set; }
        public DoubleRegister RegtResistDK3 { get; set; }
        public DoubleRegister RegProtectCurrent { get; set; }
        public DoubleRegister RegDeepCorr { get; set; }
        public DoubleRegister RegSpeedCorr { get; set; }
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
