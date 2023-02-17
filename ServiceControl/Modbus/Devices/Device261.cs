using ServiceControl.ViewModel;
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
        public List<RegisterFloat> ListCurrentPol { get; set; }
        public List<RegisterFloat> ListPolPot { get; set; }
        public List<RegisterFloat> ListSummPot { get; set; }
        public List<RegisterFloat> ListResistDK1 { get; set; }
        public List<RegisterFloat> ListResistDK2 { get; set; }
        public List<RegisterFloat> ListResistDK3 { get; set; }
        public List<RegisterFloat> ListProtectCurrent { get; set; }
        public List<RegisterFloat> ListDeepCorr { get; set; }
        public List<RegisterFloat> ListSpeedCorr { get; set; }



        public Device261(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {
            ListStatus = new List<RegisterStatus>();
            ListCurrentPol = new List<RegisterFloat>();
            ListPolPot = new List<RegisterFloat>();
            ListSummPot = new List<RegisterFloat>();
            ListResistDK1 = new List<RegisterFloat>();
            ListResistDK2 = new List<RegisterFloat>();
            ListResistDK3 = new List<RegisterFloat>();
            ListProtectCurrent = new List<RegisterFloat>();
            ListDeepCorr = new List<RegisterFloat>();
            ListSpeedCorr = new List<RegisterFloat>();

            //ListHolding = new List<DoubleRegister>();

            for (int i = 1; i <= CountKIP; i++)
            {
                RegisterStatus regStat = new RegisterStatus() { Number = i,  Address = (ushort)(i + 3000), CodeFunc = ModbusFunc.Holding, Name = "Состояние НГК-БИ(ИКП)" };
                ListStatus.Add(regStat);

                RegisterFloat reg = new RegisterFloat() { Number = i, Address = (ushort)(i + 3100), CodeFunc = ModbusFunc.Holding, Name = "Ток поляризации", Scale = 0.001f };
                ListCurrentPol.Add(reg);

                reg = new RegisterFloat() { Number = i, Address = (ushort)(i + 3200), CodeFunc = ModbusFunc.Holding, Name = "Поляризационный потенциал", Scale = 0.001f };
                ListPolPot.Add(reg);

                reg = new RegisterFloat() { Number = i, Address = (ushort)(i + 3300), CodeFunc = ModbusFunc.Holding, Name = "Суммарный потенциал", Scale = 0.01f };
                ListSummPot.Add(reg);

                reg = new RegisterFloat() { Number = i, Address = (ushort)(i + 3400), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 1 платины ДК", Scale = 0.01f };
                ListResistDK1.Add(reg);

                reg = new RegisterFloat() { Number = i, Address = (ushort)(i + 3500), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 2 платины ДК", Scale = 0.01f };
                ListResistDK2.Add(reg);

                reg = new RegisterFloat() { Number = i, Address = (ushort)(i + 3600), CodeFunc = ModbusFunc.Holding, Name = "Сопротивление 3 платины ДК", Scale = 0.01f };
                ListResistDK3.Add(reg);

                reg = new RegisterFloat() { Number = i, Address = (ushort)(i + 3700), CodeFunc = ModbusFunc.Holding, Name = "Защитный ток", Scale = 0.01f };
                ListProtectCurrent.Add(reg);

                reg = new RegisterFloat() { Number = i, Address = (ushort)(i + 3900), CodeFunc = ModbusFunc.Holding, Name = "Глубина коррозии", Scale = 1 };
                ListDeepCorr.Add(reg);

                reg = new RegisterFloat() { Number = i, Address = (ushort)(i + 4000), CodeFunc = ModbusFunc.Holding, Name = "Скорость коррозии", Scale = 1 };
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
