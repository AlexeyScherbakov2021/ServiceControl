using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Devices
{
    internal class DeviceBAVR : DeviceSlave
    {
        public RegisterInt Mode;
        public RegisterInt Avar;
        public RegisterIntFloat Temper;
        public RegisterIntFloat VoltageAKB;
        public RegisterInt64 NumberCounter;
        public RegisterIntFloat Power;
        public RegisterIntFloat Uvalue;
        public RegisterIntFloat Ivalue;
        public RegisterIntFloat Pvalue;

        public RegisterInt ModeSave;
        public RegisterInt AddressBAVRSave;
        public RegisterInt64 NumberCounterSave;

        public RegisterBool EndingAvr;
        public RegisterBool isAKB;
        public RegisterBool ModeAuto;
        public RegisterBool ModeMain;
        public RegisterBool ModeSecond;

        List<Register> ListInput;
        List<Register> ListSave;


        public DeviceBAVR(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {
            // список входных регистров
            ListInput = new List<Register>();

            Mode = new RegisterInt()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Режим работы",
                NameRes = "Mode Work",
                Measure = "",
                MeasureRes = "",
                Description = "MW",
            };
            ListInput.Add(Mode);

            Avar = new RegisterInt()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Состояние концевика аварии",
                NameRes = "Avar",
                Measure = "",
                MeasureRes = "",
                Description = "SA",
            };
            ListInput.Add(Avar);

            Temper = new RegisterIntFloat()
            {
                Address = 0x03,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Температура",
                NameRes = "t­°",
                Measure = "t­°",
                MeasureRes = "",
                Scale = 0.1f,
                Description = "Temp",
            };
            ListInput.Add(Temper);

            VoltageAKB = new RegisterIntFloat()
            {
                Address = 0x04,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Напряжение АКБ",
                NameRes = "VoltAKB",
                Measure = "В",
                MeasureRes = "",
                Scale = 0.1f,
                Description = "VoltAKB",
            };
            ListInput.Add(VoltageAKB);

            NumberCounter = new RegisterInt64()
             {
                 Address = 0x05,
                 CodeFunc = ModbusFunc.InputRegister,
                 Size = 2,
                 Name = "Номер счетчика",
                 NameRes = "NumberCount",
                 isNeg = false,
                 Measure = "",
                 MeasureRes = "",
                 Description = "NumberCount",
             };
            ListInput.Add(NumberCounter);

            Power = new RegisterIntFloat()
             {
                 Address = 0x07,
                 CodeFunc = ModbusFunc.InputRegister,
                 Size = 2,
                 Name = "Счетчик кВт*ч",
                 NameRes = "",
                 Measure = "кВт*ч",
                 MeasureRes = "",
                 Scale = 0.1f,
                 Description = "",
             };
            ListInput.Add(Power);

            Uvalue = new RegisterIntFloat()
             {
                 Address = 0x09,
                 CodeFunc = ModbusFunc.InputRegister,
                 Size = 2,
                 Name = "Напряжение",
                 NameRes = "",
                 Measure = "В",
                 MeasureRes = "",
                 Scale = 0.1f,
                 Description = "Volt",
             };
            ListInput.Add(Uvalue);

            Ivalue = new RegisterIntFloat()
             {
                 Address = 0x0B,
                 CodeFunc = ModbusFunc.InputRegister,
                 Size = 2,
                 Name = "Ток",
                 NameRes = "",
                 Measure = "А",
                 MeasureRes = "",
                 Scale = 0.1f,
                 Description = "Curr",
             };
            ListInput.Add(Ivalue);

            Pvalue = new RegisterIntFloat()
             {
                 Address = 0x0D,
                 CodeFunc = ModbusFunc.InputRegister,
                 Size = 2,
                 Name = "Мощность",
                 NameRes = "",
                 Measure = "Вт",
                 MeasureRes = "",
                 Scale = 0.1f,
                 Description = "",
             };
            ListInput.Add(Pvalue);

            ListSave = new List<Register>();

            ModeSave = new RegisterInt()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Режим работы Запись",
                NameRes = "ModeWorkSave",
                Measure = "",
                MeasureRes = "",
                Description = "MWS",
            };
            ListSave.Add(ModeSave);

            AddressBAVRSave = new RegisterInt()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Адрес БАВР Запись",
                NameRes = "AddrSave",
                Measure = "",
                MeasureRes = "",
                Description = "AS",
            };
            ListSave.Add(AddressBAVRSave);

            NumberCounterSave = new RegisterInt64()
            {
                Address = 0x03,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 2,
                Name = "Номер счетчика запись",
                NameRes = "",
                isNeg = false,
                Measure = "",
                MeasureRes = "",
                Description = "NCS",
            };
            ListSave.Add(NumberCounterSave);

            EndingAvr = new RegisterBool()
            {
                Name = "Авария БУ",
                ResultText0 = "норма",
                ResultText1 = "авария"
            };

            isAKB = new RegisterBool()
            {
                Name = "Работа от АКБ",
                ResultText0 = "норма",
                ResultText1 = "от АКБ"
            };

            ModeAuto = new RegisterBool()
            {
                Name = "авто",
                ResultText0 = "включен",
                ResultText1 = "отключен"
            };

            ModeMain = new RegisterBool()
            {
                Name = "основной",
                ResultText0 = "включен",
                ResultText1 = "отключен"
            };

            ModeSecond = new RegisterBool()
            {
                Name = "резерв",
                ResultText0 = "включен",
                ResultText1 = "отключен"
            };
        }



        public override void DumpRegisterValue(List<RegisterBase> list)
        {
        }

        public override Task StartRequestValue()
        {
            ReadRegisters(ListSave);
            ReadRegisters(ListInput);
            SetStatus();
            return Task.CompletedTask;
        }

        public override Task RequestValue()
        {
            ReadRegisters(ListInput);
            SetStatus();
            return Task.CompletedTask;
        }

        protected override void CheckListRegister()
        {
            CheckReg(ListSave);
            CheckReg(ListInput);
        }

        void SetStatus()
        {
            EndingAvr.SetResultValues((Avar.Value.Value & 1) == 1);
            isAKB.SetResultValues((Avar.Value.Value & 2) == 2);

            ModeAuto.SetResultValues((Mode.Value.Value & 1) == 0);
            ModeMain.SetResultValues((Mode.Value.Value & 2) == 0);
            ModeSecond.SetResultValues((Mode.Value.Value & 4) == 0);
        }
    }
}
