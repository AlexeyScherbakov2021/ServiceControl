using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class Device261 : Device
    {
        public static readonly int CountKIP = 32;
        public List<RegisterStatus> ListStatus { get; set; }
        public List<DoubleRegister> ListCurrentPol { get; set; }
        public List<DoubleRegister> ListPolPot { get; set; }
        public List<DoubleRegister> ListSummPot { get; set; }
        public List<DoubleRegister> ListResistDK1 { get; set; }
        public List<DoubleRegister> ListResistDK2 { get; set; }
        public List<DoubleRegister> ListResistDK3 { get; set; }
        public List<DoubleRegister> ListProtectCurrent { get; set; }
        public List<DoubleRegister> ListDeepCorr { get; set; }
        public List<DoubleRegister> ListSpeedCorr { get; set; }



        public Device261(MbWork modb, int slave) : base(modb, slave)
        {
            ListStatus = new List<RegisterStatus>();
            ListCurrentPol = new List<DoubleRegister>();
            ListPolPot = new List<DoubleRegister>();
            ListSummPot = new List<DoubleRegister>();
            ListResistDK1 = new List<DoubleRegister>();
            ListResistDK2 = new List<DoubleRegister>();
            ListResistDK3 = new List<DoubleRegister>();
            ListProtectCurrent = new List<DoubleRegister>();
            ListDeepCorr = new List<DoubleRegister>();
            ListSpeedCorr = new List<DoubleRegister>();

            //ListHolding = new List<DoubleRegister>();

            for (int i = 1; i <= CountKIP; i++)
            {
                RegisterStatus regStat = new RegisterStatus() { Number = i,  Address = (ushort)(i + 3000), CodeFunc = ModbusFunc.Holding, Name = "Состояние НГК-БИ(ИКП)" };
                ListStatus.Add(regStat);

                DoubleRegister reg = new DoubleRegister() { Number = i, Address = (ushort)(i + 3100), CodeFunc = ModbusFunc.Holding, Name = "Ток поляризации", Scale = 0.001 };
                ListCurrentPol.Add(reg);

                reg = new DoubleRegister() { Number = i, Address = (ushort)(i + 3200), CodeFunc = ModbusFunc.Holding, Name = "Поляризационный потенциал", Scale = 0.001 };
                ListPolPot.Add(reg);

                reg = new DoubleRegister() { Number = i, Address = (ushort)(i + 3300), CodeFunc = ModbusFunc.Holding, Name = "Суммарный потенциал", Scale = 0.01 };
                ListSummPot.Add(reg);

                reg = new DoubleRegister() { Number = i, Address = (ushort)(i + 3400), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 1 платины ДК", Scale = 0.01 };
                ListResistDK1.Add(reg);

                reg = new DoubleRegister() { Number = i, Address = (ushort)(i + 3500), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 2 платины ДК", Scale = 0.01 };
                ListResistDK2.Add(reg);

                reg = new DoubleRegister() { Number = i, Address = (ushort)(i + 3600), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 3 платины ДК", Scale = 0.01 };
                ListResistDK3.Add(reg);

                reg = new DoubleRegister() { Number = i, Address = (ushort)(i + 3700), CodeFunc = ModbusFunc.Holding, Name = "Защитный ток", Scale = 0.01 };
                ListProtectCurrent.Add(reg);

                reg = new DoubleRegister() { Number = i, Address = (ushort)(i + 3900), CodeFunc = ModbusFunc.Holding, Name = "Глубина коррозии", Scale = 1 };
                ListDeepCorr.Add(reg);

                reg = new DoubleRegister() { Number = i, Address = (ushort)(i + 4000), CodeFunc = ModbusFunc.Holding, Name = "Скорость коррозии", Scale = 1 };
                ListSpeedCorr.Add(reg);

            }

        }

        public override Task RequestValue()
        {
            ReadRegisters(ListStatus);
            ReadRegisters(ListCurrentPol);
            ReadRegisters(ListPolPot);
            ReadRegisters(ListSummPot);
            ReadRegisters(ListResistDK1);
            ReadRegisters(ListResistDK2);
            ReadRegisters(ListResistDK3);
            ReadRegisters(ListProtectCurrent);
            ReadRegisters(ListDeepCorr);
            ReadRegisters(ListSpeedCorr);

            return Task.CompletedTask;
        }
    }
}
