using ServiceControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{

    internal abstract class Register : RegisterBase
    {
        public int MinValue = int.MinValue;
        public int MaxValue = int.MaxValue;
        public string Measure { get; set; }

        public abstract void SetResultValues(ushort[] val);

        public abstract ushort[] SetOutput();

    }
}
