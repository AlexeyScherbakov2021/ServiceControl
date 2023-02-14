using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterInfo : RegisterInt
    {
        private string _VersionDev;
        public string VersionDev { get => _VersionDev; set { Set(ref _VersionDev, value); } }
        public string VersionPO { get; set; }
        public string Year { get; set; }
        public string NumberDev { get; set; }


        public override void SetResultValues(ushort[] val)
        {
            if (val == null || val.Length < 4) return;
            VersionPO = val[2].ToString();
            Year = (val[3] + 2000).ToString();
            NumberDev = val[4].ToString();
            VersionDev = val[1].ToString();
        }
    }
}
