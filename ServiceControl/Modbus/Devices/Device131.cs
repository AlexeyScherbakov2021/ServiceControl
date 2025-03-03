using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Devices
{

    internal class Device131 : DeviceSlave
    {
        public const int CountKIP = 32;

        public RegisterInt CoolerOn;
        public RegisterInt CoolerOff;
        public RegisterInt UoutSlope;
        public RegisterInt UsupplyOffset;
        public RegisterInt IoutOffset;
        public RegisterInt UoutOffset;
        public RegisterInt LastRegister;
        public RegisterInt NoName;

        public RegisterIntFloat CurrOutput;
        public RegisterIntFloat VoltOutput;
        public RegisterIntFloat Potencial;
        public RegisterIntFloat PolPotencial;
        public RegisterTime TimeProtect;
        public RegisterIntFloat NaprSeti;
        public RegisterIntFloat Temper;

        public RegisterStatusDC StatDC1 { get; set; }
        public RegisterStatusDC StatDC2 { get; set; }
        public RegisterStatus131 StatDK { get; set; }

        public RegisterRT RealTime { get; set; }
        public RegisterIntFloat SetCurrOutput;
        public RegisterIntFloat SetPotOutput;
        public RegisterIntFloat SetVoltageOutput;
        public RegisterInt SetCoolerOn;
        public RegisterInt SetCoolerOff;
        public RegisterInt SetUoutSlope;
        public RegisterInt SetUsupplyOffset;
        public RegisterInt SetIoutOffset;
        public RegisterInt SetUoutOffset;

        public RegisterMode131 Mode { get; set; }
        public RegisterInfo InfoReg { get; set; }

        //public RegisterFloat[] ListKIP { get; set; }

        List<Register> ListInput;
        List<Register> ListInputDop;
        List<Register> ListOutput;
        List<Register> ListOutput2;
        public List<Register> ListKIP;

        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device131(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {

            ListInput = new List<Register>();
            ListInputDop = new List<Register>();

            CoolerOn = new RegisterInt
            {
                Address = 1992,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "t° включения вентилятора",
                NameRes = "",
                Measure = "t°",
                MinValue = -2000,
                MaxValue = 2000
            };
            ListInputDop.Add(CoolerOn);

            CoolerOff = new RegisterInt
            {
                Address = 1993,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "t° выключения вентилятора",
                NameRes = "",
                Measure = "t°",
                MinValue = -2000,
                MaxValue = 2000
            };
            ListInputDop.Add(CoolerOff);

            UoutSlope = new RegisterInt
            {
                Address = 1994,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "U out slope",
                NameRes = "",
                Measure = "",
                MinValue = -2000,
                MaxValue = 2000
            };
            ListInputDop.Add(UoutSlope);

            UsupplyOffset = new RegisterInt
            {
                Address = 1995,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "U suppply offset",
                NameRes = "",
                Measure = "",
                MinValue = -2000,
                MaxValue = 2000
            };
            ListInputDop.Add(UsupplyOffset);

            IoutOffset = new RegisterInt
            {
                Address = 1996,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "I out offset",
                NameRes = "",
                Measure = "",
                MinValue = -2000,
                MaxValue = 2000
            };
            ListInputDop.Add(IoutOffset);

            UoutOffset = new RegisterInt
            {
                Address = 1997,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "U out offset",
                NameRes = "",
                Measure = "",
                MinValue = -2000,
                MaxValue = 2000
            };
            ListInputDop.Add(UoutOffset);

            LastRegister = new RegisterInt
            {
                Address = 1998,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Последний записанный регистр",
                NameRes = "",
                Measure = "",
                MinValue = 0,
                MaxValue = int.MaxValue
            };
            ListInputDop.Add(LastRegister);

            NoName = new RegisterInt
            {
                Address = 1999,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "",
                NameRes = "",
                Measure = "",
                MinValue = 0,
                MaxValue = int.MaxValue
            };
            ListInputDop.Add(NoName);

            CurrOutput = new RegisterIntFloat()
            {
                Address = 2000,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Выходной ток",
                NameRes = "OutCur",
                Measure = "A",
                Scale = 0.01f,
                MinValue = -320,
                MaxValue = 320
            };
            ListInput.Add(CurrOutput);

            VoltOutput = new RegisterIntFloat()
            {
                Address = 2001,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Выходное напряжение",
                NameRes = "OutNapr",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uвых",
                Scale = 0.1f,
                MinValue = -200,
                MaxValue = 200
            };
            ListInput.Add(VoltOutput);


            PolPotencial = new RegisterIntFloat()
            {
                Address = 2002,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Поляризационный потенциал",
                NameRes = "PolPot",
                Measure = "В",
                MeasureRes = "Volt",
                Scale = 0.01f,
                MinValue = -5,
                MaxValue = 5
            };


            Potencial = new RegisterIntFloat()
            {
                Address = 2002,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Суммарный потенциал",
                NameRes = "SummPot",
                Measure = "В",
                MeasureRes = "Volt",
                Scale = 0.01f,
                MinValue = -5,
                MaxValue = 5
            };
            ListInput.Add(Potencial);

            TimeProtect = new RegisterTime()
            {
                Address = 2003,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 2,
                Name = "Время защиты сооружения",
                NameRes = "TimeProtect",
                Measure = "ч",
                MeasureRes = "Hour",
                //Scale = 1/3600f,
                MinValue = 0,
                MaxValue = int.MaxValue
            };
            ListInput.Add(TimeProtect);

            NaprSeti = new RegisterIntFloat()
            {
                Address = 2005,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Напряжение сети",
                NameRes = "VoltCircuit1",
                Measure = "В",
                MeasureRes = "Volt",
                Scale = 1f,
                MinValue = 0,
                MaxValue = 300
            };
            ListInput.Add(NaprSeti);


            Temper = new RegisterIntFloat()
            {
                Address = 2006,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Температура",
                Measure = "°С",
                NameRes = "InnnerTemper",
                Description = "°C",
                MinValue = -99,
                MaxValue = 100
            };
            ListInput.Add(Temper);

            StatDC1 = new RegisterStatusDC()
            {
                Address = 2007,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Статус выпрямителей 1-8",
                NameRes = "",
            };
            ListInput.Add(StatDC1);

            StatDC2 = new RegisterStatusDC(9)
            {
                Address = 2008,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Статус выпрямителей 9-16",
                NameRes = "",
            };
            ListInput.Add(StatDC2);

            StatDK = new RegisterStatus131()
            {
                Address = 2009,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Состояние ДК",
                NameRes = "",

            };
            ListInput.Add(StatDK);


            RealTime = new RegisterRT()
            {
                Address = 2010,
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
            ListInput.Add(RealTime);

            ListOutput = new List<Register>();

            SetCurrOutput = new RegisterIntFloat()
            {
                Address = 2014,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Задание выходного тока",
                NameRes = "SetCurrent",
                Measure = "A",
                Description = "Iуст",
                Scale = 0.01f,
                MinValue = 0,
                MaxValue = 100
            };
            ListOutput.Add(SetCurrOutput);

            RegisterInt reserv = new RegisterInt()
            {
                Address = 2015,
                CodeFunc = ModbusFunc.HoldingRegister,
            };
            ListOutput.Add(reserv);

            SetVoltageOutput = new RegisterIntFloat()
            {
                Address = 2016,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Задание напряжения",
                NameRes = "SetVoltage",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "U",
                Scale = 0.1f,
                MinValue = 0,
                MaxValue = 48
            };


            SetPotOutput = new RegisterIntFloat()
            {
                Address = 2016,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Задание потенциала",
                NameRes = "SetSummPot",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uпотс",
                Scale = 0.01f,
                MinValue = -5,
                MaxValue = 5
            };
            ListOutput.Add(SetPotOutput);


            Mode = new RegisterMode131()
            {
                Address = 2017,
                CodeFunc = ModbusFunc.HoldingRegister,

            };
            //ListInput.Add(Mode);

            ListOutput2 = new List<Register>();

            SetCoolerOn = new RegisterInt
            {
                Address = 1992,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "t° включения вентилятора",
                NameRes = "",
                Measure = "t°",
                MinValue = 0,
                MaxValue = 200
            };
            ListOutput2.Add(SetCoolerOn);

            SetCoolerOff = new RegisterInt
            {
                Address = 1993,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "t° выключения вентилятора",
                NameRes = "",
                Measure = "t°",
                MinValue = 0,
                MaxValue = 200
            };
            ListOutput2.Add(SetCoolerOff);

            SetUoutSlope = new RegisterInt
            {
                Address = 1994,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "U out slope",
                NameRes = "",
                Measure = "",
                MinValue = -2000,
                MaxValue = 2000
            };
            ListOutput2.Add(SetUoutSlope);

            SetUsupplyOffset = new RegisterInt
            {
                Address = 1995,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "U suppply offset",
                NameRes = "",
                Measure = "",
                MinValue = -2000,
                MaxValue = 2000
            };
            ListOutput2.Add(SetUsupplyOffset);

            SetIoutOffset = new RegisterInt
            {
                Address = 1996,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "I out offset",
                NameRes = "",
                Measure = "",
                MinValue = -2000,
                MaxValue = 2000
            };
            ListOutput2.Add(SetIoutOffset);

            SetUoutOffset = new RegisterInt
            {
                Address = 1997,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "U out offset",
                NameRes = "",
                Measure = "",
                MinValue = -2000,
                MaxValue = 2000
            };
            ListOutput2.Add(SetUoutOffset);


            ListKIP = new List<Register>();
            for(int i = 0; i < CountKIP; i++)
            {
                ListKIP.Add( new RegisterIntFloat()
                {
                    Address = (ushort)(2018 + i),
                    CodeFunc = ModbusFunc.HoldingRegister,
                    Size = 1,
                    Number = i + 1,
                    Name = "КИП",
                    NameRes = "KIP",
                    Measure = "В",
                    MeasureRes = "Volt",
                    Scale = 0.01f,
                    MinValue = -5,
                    MaxValue = 5
                }
                );
                //ListInput.Add(ListKIP[i]);
            }

            InfoReg = new RegisterInfo() { Name = "Информация", NameRes = "" };

        }

        //-------------------------------------------------------------------------------------------
        // 
        //-------------------------------------------------------------------------------------------
        public override Task RequestValue()
        {
            ReadRegisters(ListInputDop);
            ReadRegisters(ListInput);
            ReadRegisters(ListKIP);
            //StatDC1.ValueStat[0].StatusDC = StatusDC.Off;
            ReadRegister(Mode);
            return Task.CompletedTask;
        }

        //-------------------------------------------------------------------------------------------
        // 
        //-------------------------------------------------------------------------------------------
        public override Task StartRequestValue()
        {
            //ReadInfoRegister(InfoReg);
            LastRegister.Value = 0;
            WriteRegister(LastRegister);
            ReadRegisters(ListInputDop);
            ReadRegisters(ListInput);
            ReadRegisters(ListOutput);
            ReadRegisters(ListOutput2);
            ReadRegisters(ListKIP);
            ReadRegister(Mode);
            return Task.CompletedTask;
        }

        //-------------------------------------------------------------------------------------------
        // проверка последовательноти регистров в списках
        //-------------------------------------------------------------------------------------------
        protected override void CheckListRegister()
        {
            CheckReg(ListInputDop);
            CheckReg(ListInput);
            CheckReg(ListOutput);
            CheckReg(ListOutput2);

        }

        //-------------------------------------------------------------------------------------------
        // Изменине языка для регистров
        //-------------------------------------------------------------------------------------------
        public override void ChangeLangRegister()
        {
            ListInput.ForEach(n => n.SetLanguage());
            ListOutput.ForEach(n => n.SetLanguage());
            ListKIP.ForEach(n => n.SetLanguage());
            InfoReg.SetLanguage();
            InfoReg.SetLanguage();
            Mode.SetLanguage();
            PolPotencial.SetLanguage();
        }

        public override void DumpRegisterValue(List<RegisterBase> list)
        {
            throw new NotImplementedException();
        }
    }
}
