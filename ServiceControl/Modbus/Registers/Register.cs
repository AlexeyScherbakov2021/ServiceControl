using ServiceControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    //public enum TypeValue {Bool, Int16, Int32, Uint16};

    internal abstract class Register<T1, T2> : Observable
    {
        public ushort Address;
        public string Name { get; set; }
        public string Description;
        public ModbusFunc CodeFunc;
        public ushort Size = 1;
        
        private T2 _Value;
        public T2 Value { get => _Value; set { Set(ref _Value, value); } }

        public abstract T2 GetResult(T1[] val);
        public abstract ushort SetUshort();

    }
}
