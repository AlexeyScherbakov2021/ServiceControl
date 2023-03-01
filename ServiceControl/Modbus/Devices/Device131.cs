﻿using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Devices
{

    internal class Device131 : Device
    {
        public const int CountKIP = 32;


        public RegisterFloat CurrOutput;
        public RegisterFloat VoltOutput;
        public RegisterFloat Potencial;
        public RegisterInt TimeProtect;
        public RegisterFloat NaprSeti;
        public RegisterFloat Temper;

        public RegisterStatusDC StatDC1;
        public RegisterStatusDC StatDC2;
        public RegisterStatus131 StatDK;

        public RegisterRT RealTime;
        public RegisterFloat SetCurrOutput;
        public RegisterFloat SetPotOutput;
        public RegisterMode131 Mode;
        public RegisterInfo InfoReg { get; set; }


        public RegisterFloat[] ListKIP;

        List<Register> ListInput;

        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device131(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {

            ListInput = new List<Register>();

            CurrOutput = new RegisterFloat()
            {
                Address = 2000,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Выходной ток",
                NameRes = "OutCur",
                Measure = "A",
                Scale = 0.01f,
                MinValue = 0,
                MaxValue = 100
            };
            ListInput.Add(CurrOutput);

            VoltOutput = new RegisterFloat()
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
                MinValue = 0,
                MaxValue = 48
            };
            ListInput.Add(VoltOutput);

            Potencial = new RegisterFloat()
            {
                Address = 2002,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Потенциал",
                NameRes = "SummPot",
                Measure = "В",
                MeasureRes = "Volt",
                Scale = 0.01f,
                MinValue = -5,
                MaxValue = 5
            };
            ListInput.Add(Potencial);

            TimeProtect = new RegisterInt()
            {
                Address = 2003,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 2,
                Name = "Время защиты сооружения",
                NameRes = "TimeProtect",
                Measure = "ч",
                MeasureRes = "Hour",
                MinValue = 0,
                MaxValue = 999999
            };
            ListInput.Add(TimeProtect);

            NaprSeti = new RegisterFloat()
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


            Temper = new RegisterFloat()
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

            StatDC2 = new RegisterStatusDC()
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


            SetCurrOutput = new RegisterFloat()
            {
                Address = 2014,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Задание выходного тока",
                NameRes = "SetCurrent",
                Measure = "A",
                Description = "Iуст",
                MinValue = 0,
                MaxValue = 100
            };
            ListInput.Add(SetCurrOutput);

            RegisterInt reserv = new RegisterInt()
            {
                Address = 2015,
                CodeFunc = ModbusFunc.HoldingRegister,
            };
            ListInput.Add(reserv);

            SetPotOutput = new RegisterFloat()
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
            ListInput.Add(SetPotOutput);


            Mode = new RegisterMode131()
            {
                Address = 2017,
                CodeFunc = ModbusFunc.HoldingRegister,

            };
            ListInput.Add(Mode);

            ListKIP = new RegisterFloat[CountKIP];
            for(int i = 0; i < CountKIP; i++)
            {
                ListKIP[i] = new RegisterFloat()
                {
                    Address = (ushort)(2018 + i),
                    CodeFunc = ModbusFunc.HoldingRegister,
                    Size = 1,
                    Name = $"КИП {i + 1}",
                    NameRes = "SummPot",
                    Measure = "В",
                    MeasureRes = "Volt",
                    Scale = 0.01f,
                    MinValue = -5,
                    MaxValue = 5
                };
                ListInput.Add(ListKIP[i]);
            }

            InfoReg = new RegisterInfo() { Name = "Информация", NameRes = "" };

        }

        //-------------------------------------------------------------------------------------------
        // 
        //-------------------------------------------------------------------------------------------
        public override Task RequestValue()
        {
            return Task.CompletedTask;
        }

        //-------------------------------------------------------------------------------------------
        // 
        //-------------------------------------------------------------------------------------------
        public override Task StartRequestValue()
        {
            ReadInfoRegister(InfoReg);
            ReadRegisters(ListInput);
            return Task.CompletedTask;
        }

        //-------------------------------------------------------------------------------------------
        // проверка последовательноти регистров в списках
        //-------------------------------------------------------------------------------------------
        protected override void CheckListRegister()
        {
            ReadInfoRegister(InfoReg);
            ReadRegisters(ListInput);
            CheckReg(ListInput);
        }

        //-------------------------------------------------------------------------------------------
        // Изменине языка для регистров
        //-------------------------------------------------------------------------------------------
        public override void ChangeLangRegister()
        { 
        }

    }
}