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
        public List<RegisterDouble> ListCurrentPol { get; set; }
        public List<RegisterDouble> ListPolPot { get; set; }
        public List<RegisterDouble> ListSummPot { get; set; }
        public List<RegisterDouble> ListResistDK1 { get; set; }
        public List<RegisterDouble> ListResistDK2 { get; set; }
        public List<RegisterDouble> ListResistDK3 { get; set; }
        public List<RegisterDouble> ListProtectCurrent { get; set; }
        public List<RegisterDouble> ListDeepCorr { get; set; }
        public List<RegisterDouble> ListSpeedCorr { get; set; }



        public Device261(MbWork modb, int slave) : base(modb, slave)
        {
            ListStatus = new List<RegisterStatus>();
            ListCurrentPol = new List<RegisterDouble>();
            ListPolPot = new List<RegisterDouble>();
            ListSummPot = new List<RegisterDouble>();
            ListResistDK1 = new List<RegisterDouble>();
            ListResistDK2 = new List<RegisterDouble>();
            ListResistDK3 = new List<RegisterDouble>();
            ListProtectCurrent = new List<RegisterDouble>();
            ListDeepCorr = new List<RegisterDouble>();
            ListSpeedCorr = new List<RegisterDouble>();

            //ListHolding = new List<DoubleRegister>();

            for (int i = 1; i <= CountKIP; i++)
            {
                RegisterStatus regStat = new RegisterStatus() { Number = i,  Address = (ushort)(i + 3000), CodeFunc = ModbusFunc.Holding, Name = "Состояние НГК-БИ(ИКП)" };
                ListStatus.Add(regStat);

                RegisterDouble reg = new RegisterDouble() { Number = i, Address = (ushort)(i + 3100), CodeFunc = ModbusFunc.Holding, Name = "Ток поляризации", Scale = 0.001 };
                ListCurrentPol.Add(reg);

                reg = new RegisterDouble() { Number = i, Address = (ushort)(i + 3200), CodeFunc = ModbusFunc.Holding, Name = "Поляризационный потенциал", Scale = 0.001 };
                ListPolPot.Add(reg);

                reg = new RegisterDouble() { Number = i, Address = (ushort)(i + 3300), CodeFunc = ModbusFunc.Holding, Name = "Суммарный потенциал", Scale = 0.01 };
                ListSummPot.Add(reg);

                reg = new RegisterDouble() { Number = i, Address = (ushort)(i + 3400), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 1 платины ДК", Scale = 0.01 };
                ListResistDK1.Add(reg);

                reg = new RegisterDouble() { Number = i, Address = (ushort)(i + 3500), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 2 платины ДК", Scale = 0.01 };
                ListResistDK2.Add(reg);

                reg = new RegisterDouble() { Number = i, Address = (ushort)(i + 3600), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 3 платины ДК", Scale = 0.01 };
                ListResistDK3.Add(reg);

                reg = new RegisterDouble() { Number = i, Address = (ushort)(i + 3700), CodeFunc = ModbusFunc.Holding, Name = "Защитный ток", Scale = 0.01 };
                ListProtectCurrent.Add(reg);

                reg = new RegisterDouble() { Number = i, Address = (ushort)(i + 3900), CodeFunc = ModbusFunc.Holding, Name = "Глубина коррозии", Scale = 1 };
                ListDeepCorr.Add(reg);

                reg = new RegisterDouble() { Number = i, Address = (ushort)(i + 4000), CodeFunc = ModbusFunc.Holding, Name = "Скорость коррозии", Scale = 1 };
                ListSpeedCorr.Add(reg);

            }

            //CheckListRegister();
            //StartRequestValue();
        }

        public override Task StartRequestValue()
        {
            return RequestValue();

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

        protected override void CheckListRegister()
        {
            CheckReg(ListStatus);
            CheckReg(ListCurrentPol);
            CheckReg(ListPolPot);
            CheckReg(ListSummPot);
            CheckReg(ListResistDK1);
            CheckReg(ListResistDK2);
            CheckReg(ListResistDK3);
            CheckReg(ListProtectCurrent);
            CheckReg(ListDeepCorr);
            CheckReg(ListSpeedCorr);
        }

    }
}
