using ServiceControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal abstract class RegisterBase : Observable
    {
        public int Number { get; set; }
        public ushort Address { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ushort Size = 1;
        public ModbusFunc CodeFunc;

        private string _ValueString;
        public string ValueString { get => _ValueString; set { Set(ref _ValueString, value); } }

        //public abstract void SetResultValues(T[] val);

        //public abstract T SetOutput();

    }
}
