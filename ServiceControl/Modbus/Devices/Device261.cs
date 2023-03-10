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
        public List<RegisterKIP> ListCurrentPol { get; set; }
        public List<RegisterKIP> ListPolPot { get; set; }
        public List<RegisterKIP> ListSummPot { get; set; }
        public List<RegisterKIP> ListResistDK1 { get; set; }
        public List<RegisterKIP> ListResistDK2 { get; set; }
        public List<RegisterKIP> ListResistDK3 { get; set; }
        public List<RegisterKIP> ListProtectCurrent { get; set; }
        public List<RegisterKIP> ListDeepCorr { get; set; }
        public List<RegisterKIP> ListSpeedCorr { get; set; }
        public RegisterRT RealTime { get; set; }


        public Device261(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {
            ListStatus = new List<RegisterStatus>();
            ListCurrentPol = new List<RegisterKIP>();
            ListPolPot = new List<RegisterKIP>();
            ListSummPot = new List<RegisterKIP>();
            ListResistDK1 = new List<RegisterKIP>();
            ListResistDK2 = new List<RegisterKIP>();
            ListResistDK3 = new List<RegisterKIP>();
            ListProtectCurrent = new List<RegisterKIP>();
            ListDeepCorr = new List<RegisterKIP>();
            ListSpeedCorr = new List<RegisterKIP>();

            for (int i = 0; i < CountKIP; i++)
            {
                RegisterStatus regStat = new RegisterStatus() { Number = i + 1,  Address = (ushort)(i + 3000), 
                    CodeFunc = ModbusFunc.HoldingRegister, Name = "Состояние НГК-БИ(ИКП)" };
                ListStatus.Add(regStat);

                RegisterKIP reg = new RegisterKIP() { Number = i + 1, Address = (ushort)(i + 3100), 
                    MinValue = -5, MaxValue = 5,
                    CodeFunc = ModbusFunc.HoldingRegister, Name = "Ток поляризации", Scale = 0.001f };
                ListCurrentPol.Add(reg);

                reg = new RegisterKIP() { Number = i + 1, Address = (ushort)(i + 3200),
                    MinValue = -2,
                    MaxValue = 2,
                    CodeFunc = ModbusFunc.HoldingRegister, Name = "Поляризационный потенциал", Scale = 0.001f };
                ListPolPot.Add(reg);

                reg = new RegisterKIP() { Number = i + 1, Address = (ushort)(i + 3300),
                    MinValue = -10,
                    MaxValue = 10,
                    CodeFunc = ModbusFunc.HoldingRegister, Name = "Суммарный потенциал", Scale = 0.01f };
                ListSummPot.Add(reg);

                reg = new RegisterKIP() { Number = i + 1, Address = (ushort)(i + 3400),
                    MinValue = 0,
                    MaxValue = 110,
                    CodeFunc = ModbusFunc.HoldingRegister, Name = "Сопротивление 1 платины ДК", Scale = 0.01f };
                ListResistDK1.Add(reg);

                reg = new RegisterKIP() { Number = i + 1, Address = (ushort)(i + 3500),
                    MinValue = 0,
                    MaxValue = 110,
                    CodeFunc = ModbusFunc.HoldingRegister, Name = "Сопротивление 2 платины ДК", Scale = 0.01f };
                ListResistDK2.Add(reg);

                reg = new RegisterKIP() { Number = i + 1, Address = (ushort)(i + 3600),
                    MinValue = 0,
                    MaxValue = 110,
                    CodeFunc = ModbusFunc.HoldingRegister, Name = "Сопротивление 3 платины ДК", Scale = 0.01f };
                ListResistDK3.Add(reg);

                reg = new RegisterKIP() { Number = i + 1, Address = (ushort)(i + 3700),
                    MinValue = 0,
                    MaxValue = 50,
                    CodeFunc = ModbusFunc.HoldingRegister, Name = "Защитный ток", Scale = 0.01f };
                ListProtectCurrent.Add(reg);

                reg = new RegisterKIP() { Number = i + 1, Address = (ushort)(i + 3900),
                    MinValue = 0,
                    MaxValue = 65535,
                    CodeFunc = ModbusFunc.HoldingRegister, Name = "Глубина коррозии", Scale = 1 };
                ListDeepCorr.Add(reg);

                reg = new RegisterKIP() { Number = i + 1, Address = (ushort)(i + 4000),
                    MinValue = 0,
                    MaxValue = 65535,
                    CodeFunc = ModbusFunc.HoldingRegister, Name = "Скорость коррозии", Scale = 1 };
                ListSpeedCorr.Add(reg);

            }

            RealTime = new RegisterRT()
            {
                Address = 4101,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Время устройства",
                NameRes = "TimeDevice",
                Measure = "сек",
                MeasureRes = "SEC",
                Size = 4,
                Description = "РВ",
                MinValue = 0,
                MaxValue = int.MaxValue
            };
        }

        public override Task StartRequestValue()
        {
            return RequestValue();
        }

        public override Task RequestValue()
        {
            ReadRegisters(ListStatus);

            //ListStatus[0].StatusBlock = BlockStatus.Absent;
            //ListStatus[1].StatusBlock = BlockStatus.Alarm;
            //ListStatus[2].StatusBlock = BlockStatus.Opened;
            //ListStatus[2].IsDK1Break = true;
            //ListStatus[3].IsDK1Break = true;
            //ListStatus[4].IsDK1Break = true;
            
            //ListStatus[5].IsSensorOpen = true;
            //ListStatus[6].IsSensorOpen = true;

            ReadRegisters(ListCurrentPol);
            ReadRegisters(ListPolPot);
            ReadRegisters(ListSummPot);
            ReadRegisters(ListResistDK1);
            ReadRegisters(ListResistDK2);
            ReadRegisters(ListResistDK3);
            ReadRegisters(ListProtectCurrent);
            ReadRegisters(ListDeepCorr);
            ReadRegisters(ListSpeedCorr);

            ReadRegister(RealTime);

            SetValueFromStatus();

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


        private void SetValueFromStatus()
        {
            for (int i = 0; i < CountKIP; i++)
            {
                if (ListStatus[i].StatusBlock != BlockStatus.Absent)
                {
                    ListCurrentPol[i].SetValueFromStatus(true);
                    ListPolPot[i].SetValueFromStatus(true);
                    ListSummPot[i].SetValueFromStatus(true);
                    ListResistDK1[i].SetValueFromStatus(true);
                    ListResistDK2[i].SetValueFromStatus(true);
                    ListResistDK3[i].SetValueFromStatus(true);
                    ListProtectCurrent[i].SetValueFromStatus(true);
                    ListDeepCorr[i].SetValueFromStatus(true);
                    ListSpeedCorr[i].SetValueFromStatus(true);
                }
            }
        }
    }
}
