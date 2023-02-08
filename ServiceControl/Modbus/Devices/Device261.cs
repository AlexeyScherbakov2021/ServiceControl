using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class Device261 : Device
    {
        public Device261(MbWork modb, byte slave) : base(modb, slave)
        {
            ListHolding = new List<DoubleRegister>()
            {
                new DoubleRegister() { Address = 3000, CodeFunc = ModbusFunc.Holding, Name = "Состояние НГК-БИ(ИКП)" },
                new DoubleRegister() { Address = 3100, CodeFunc = ModbusFunc.Holding, Name = "Ток поляризации", Scale = 0.001},
                new DoubleRegister() { Address = 3200, CodeFunc = ModbusFunc.Holding, Name = "Поляризационный потенциал", Scale = 0.001},
                new DoubleRegister() { Address = 3300, CodeFunc = ModbusFunc.Holding, Name = "Суммарный потенциал",  Scale = 0.01},
                new DoubleRegister() { Address = 3400, CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 1 платины ДК", Scale = 0.01},
                new DoubleRegister() { Address = 3500, CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 2 платины ДК", Scale = 0.01},
                new DoubleRegister() { Address = 3600, CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 3 платины ДК", Scale = 0.01},
                new DoubleRegister() { Address = 3700, CodeFunc = ModbusFunc.Holding, Name = "Защитный ток", Scale = 0.01},
                new DoubleRegister() { Address = 3900, CodeFunc = ModbusFunc.Holding, Name = "Глубина коррозии", Scale = 1},
                new DoubleRegister() { Address = 4000, CodeFunc = ModbusFunc.Holding, Name = "Скорость коррозии", Scale = 1},
            };
        }
    }
}
