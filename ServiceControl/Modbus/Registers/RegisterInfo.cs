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

        private string _VersionPO;
        public string VersionPO { get => _VersionPO; set { Set(ref _VersionPO, value); } }

        private string _Year;
        public string Year { get => _Year; set { Set(ref _Year, value); } }

        private string _NumberDev;
        public string NumberDev { get => _NumberDev; set { Set(ref _NumberDev, value); } }


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
