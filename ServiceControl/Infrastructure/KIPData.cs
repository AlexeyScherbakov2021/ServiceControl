using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Infrastructure
{
    internal class KIPData
    {
        public string SensorOpen { get; set; }
        public string StatusBloc { get; set; }
        public string SensorCorr1 { get; set; }
        public string SensorCorr2 { get; set; }
        public string SensorCorr3 { get; set; }

        public double CurrentPolar { get; set; }
        public double PolarPot { get; set; }
        public double SummaryPot { get; set; }
        public double ResistCorr1 { get; set; }
        public double ResistCorr2 { get; set; }
        public double ResistCorr3 { get; set; }
        public double ProtectCurrent { get; set; }
        public double DeepCorr { get; set; }
        public double SpeedCorr { get; set; }

        public DateTime DeviceTime { get; set; }
    }
}
