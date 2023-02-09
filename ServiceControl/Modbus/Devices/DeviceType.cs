using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Devices
{
    public enum DevType { KS131, KS216, KS356, KS261 };
    internal class DeviceType
    {
        public string Name { get; set; }
        public DevType deviceType { get; set; }
    }
}
