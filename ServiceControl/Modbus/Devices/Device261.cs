using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class Device261 : Device
    {
        private const int CountKIP = 32;

        public Device261(MbWork modb, byte slave) : base(modb, slave)
        {
            ListHolding = new List<DoubleRegister>();

            for(int i = 0; i < CountKIP; i++)
            {
                DoubleRegister reg = new DoubleRegister() { Address = (ushort)(i + 3000), CodeFunc = ModbusFunc.Holding, Name = "Состояние НГК-БИ(ИКП)" };
                ListHolding.Add(reg);
                reg = new DoubleRegister() { Address = (ushort)(i + 3100), CodeFunc = ModbusFunc.Holding, Name = "Ток поляризации", Scale = 0.001 };
                ListHolding.Add(reg);
                reg = new DoubleRegister() { Address = (ushort)(i + 3200), CodeFunc = ModbusFunc.Holding, Name = "Поляризационный потенциал", Scale = 0.001 };
                ListHolding.Add(reg);
                reg = new DoubleRegister() { Address = (ushort)(i + 3300), CodeFunc = ModbusFunc.Holding, Name = "Суммарный потенциал", Scale = 0.01 };
                ListHolding.Add(reg);
                reg = new DoubleRegister() { Address = (ushort)(i + 3400), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 1 платины ДК", Scale = 0.01 };
                ListHolding.Add(reg);
                reg = new DoubleRegister() { Address = (ushort)(i + 3500), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 2 платины ДК", Scale = 0.01 };
                ListHolding.Add(reg);
                reg = new DoubleRegister() { Address = (ushort)(i + 3600), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 3 платины ДК", Scale = 0.01 };
                ListHolding.Add(reg);
                reg = new DoubleRegister() { Address = (ushort)(i + 3700), CodeFunc = ModbusFunc.Holding, Name = "Защитный ток", Scale = 0.01 };
                ListHolding.Add(reg);
                reg = new DoubleRegister() { Address = (ushort)(i + 3900), CodeFunc = ModbusFunc.Holding, Name = "Глубина коррозии", Scale = 1 };
                ListHolding.Add(reg);
                reg = new DoubleRegister() { Address = (ushort)(i + 4000), CodeFunc = ModbusFunc.Holding, Name = "Скорость коррозии", Scale = 1 };
                ListHolding.Add(reg);

            }

        }
    }
}
