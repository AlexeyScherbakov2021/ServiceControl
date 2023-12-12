using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Devices
{
    public enum DevType { KS131, KS216, KS356, KS261, BI_M_Master, BI_M_Slave, KIP_M5, KIP_M5Ext, 
        KSSM, KIP_LC, KIP_UDZ, TERMINAL };
    internal class DeviceType
    {
        public string Name { get; set; }
        public DevType deviceType { get; set; }
        public bool isSlave;
    }
}
