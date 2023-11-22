using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ServiceControl.Modbus.Devices
{
    internal class DeviceKIPM5 : DeviceSlave
    {
        public RegisterFloat VoltPower;                 // Напряжение сети 
        public RegisterFloat CurrOut;                   // Ток на выходе СКЗ
        public RegisterFloat VoltOut;                   // Напряжение на выходе СКЗ
        public RegisterFloat SummPot;                   // Суммарный потенциал
        public RegisterFloat PolPot;                    // Поляризационный потенциал
        public RegisterFloat CurrPot;                   // Ток поляризации
        public RegisterFloat VoltNaveden;               // Наведенное напряжение
        public RegisterFloat FreqVoltNaveden;           // Частота наведенного напряжение
        public RegisterFloat TemperBoard;               // Температура внутри КИП
        public RegisterFloat SpeedKorr;                 // Скорость коррозии
        public RegisterFloat DeepKorr;                  // Глубина коррозии
        public RegisterFloat Power;                     // Потребляемая мощность
        public RegisterFloat CountPower;                // Счетчик эл.энергии
        public RegisterFloat isDoor;                    // Геркон двери
        public RegisterFloat FlagsWork;                 // Флаги работы КИП
        public RegisterFloat PeriodADC;                 // Период измерений АЦП
        public RegisterFloat UpLimitCurr;               // Верхняя уставка Iвых
        public RegisterFloat DownLimitCurr;             // Нижняя уставка Iвых
        public RegisterFloat UpLimitVolt;               // Верхняя уставка Uвых
        public RegisterFloat DownLimitVolt;             // Нижняя уставка Uвых
        public RegisterFloat UpLimitVoltSP;             // Верхняя уставка Uсп
        public RegisterFloat DownLimitVoltSP;           // Нижняя уставка Uсп
        public RegisterFloat OutUpLimitCurr;            // Выходы за верхнюю уставку Iвых
        public RegisterFloat OutDownLimitCurr;          // Выходы за нижнюю уставку Iвых
        public RegisterFloat OutUpLimitVolt;            // Выходы за верхнюю уставку Uвых
        public RegisterFloat OutDownLimitVolt;          // Выходы за нижнюю уставку Uвых
        public RegisterFloat OutUpLimitVoltSP;          // Выходы за верхнюю уставку Uсп
        public RegisterFloat OutDownLimitVoltSP;        // Выходы за нижнюю уставку Uсп
        public RegisterFloat SummPotRMS;                // Суммарный потенциал RMS
        public RegisterRT RealTime { get; set; }        // Текущее системное время
        public RegisterFloat K_shunt;                   // Номинал шунта в Амперах
        public RegisterFloat VoltControl;               // Напряжение управления СКЗ 


        List<Register> ListInput = new List<Register>();
        List<Register> ListWriteControl = new List<Register>();

        public DeviceKIPM5(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {

            VoltPower = new RegisterFloat()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напряжение сети ",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = 0,
                MaxValue = 264
            };
            ListInput.Add(VoltPower);

            CurrOut = new RegisterFloat()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток на выходе СКЗ",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -320f,
                MaxValue = 320f
            };
            ListInput.Add(CurrOut);

            VoltOut = new RegisterFloat()
            {
                Address = 0x03,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напряжение на выходе СКЗ",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -100f,
                MaxValue = 100f
            };
            ListInput.Add(VoltOut);

            SummPot = new RegisterFloat()
            {
                Address = 0x04,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Суммарный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uпп1",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListInput.Add(SummPot);

            PolPot = new RegisterFloat()
            {
                Address = 0x05,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Поляризационный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uпп1",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListInput.Add(PolPot);

            CurrPot = new RegisterFloat()
            {
                Address = 0x06,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток поляризации",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "Iпол1",
                Scale = 0.001f,
                MinValue = -50f,
                MaxValue = 50f
            };
            ListInput.Add(CurrPot);

            VoltNaveden = new RegisterFloat()
            {
                Address = 0x07,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Наведенное напряжение",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uвых",
                Scale = 0.1f,
                MinValue = 0f,
                MaxValue = 150f
            };
            ListInput.Add(VoltNaveden);

            FreqVoltNaveden = new RegisterFloat()
            {
                Address = 0x08,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Частота наведенного напряжение",
                NameRes = "",
                Measure = "Гц",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 160f
            };
            ListInput.Add(FreqVoltNaveden);

            TemperBoard = new RegisterFloat()
            {
                Address = 0x09,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Температура внутри КИП",
                NameRes = "",
                Measure = "°С",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = -55f,
                MaxValue = 125f
            };
            ListInput.Add(TemperBoard);

            SpeedKorr = new RegisterFloat()
            {
                Address = 0x0A,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Скорость коррозии",
                NameRes = "",
                Measure = "мкм/год",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(SpeedKorr);

            DeepKorr = new RegisterFloat()
            {
                Address = 0x0B,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Глубина коррозии",
                NameRes = "",
                Measure = "мкм",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(DeepKorr);

            Power = new RegisterFloat()
            {
                Address = 0x0C,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Потребляемая мощность",
                NameRes = "",
                Measure = "Вт",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(Power);

            CountPower = new RegisterFloat()
            {
                Address = 0x0D,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Счетчик эл.энергии",
                NameRes = "",
                Measure = "кВт/ч",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(CountPower);

            isDoor = new RegisterFloat()
            {
                Address = 0x0E,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Геркон двери",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 1f
            };
            ListInput.Add(isDoor);

            FlagsWork = new RegisterFloat()
            {
                Address = 0x0F,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Флаги работы КИП",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(FlagsWork);

            PeriodADC = new RegisterFloat()
            {
                Address = 0x10,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Период измерений АЦП",
                NameRes = "",
                Measure = "мсек",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(PeriodADC);

            UpLimitCurr = new RegisterFloat()
            {
                Address = 0x11,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Верхняя уставка Iвых",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "Iпол2",
                Scale = 0.01f,
                MinValue = -100f,
                MaxValue = 100f
            };
            ListInput.Add(UpLimitCurr);

            DownLimitCurr = new RegisterFloat()
            {
                Address = 0x12,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Нижняя уставка Iвых",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -320f,
                MaxValue = 320f
            };
            ListInput.Add(DownLimitCurr);

            UpLimitVolt = new RegisterFloat()
            {
                Address = 0x13,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Верхняя уставка Uвых",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -100f,
                MaxValue = 100f
            };
            ListInput.Add(UpLimitVolt);

            DownLimitVolt = new RegisterFloat()
            {
                Address = 0x14,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Нижняя уставка Uвых",
                NameRes = "",
                Measure = "мВ",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -100f,
                MaxValue = 100f
            };
            ListInput.Add(DownLimitVolt);

            UpLimitVoltSP = new RegisterFloat()
            {
                Address = 0x15,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Верхняя уставка Uсп",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListInput.Add(UpLimitVoltSP);

            DownLimitVoltSP = new RegisterFloat()
            {
                Address = 0x16,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Нижняя уставка Uсп",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "Bit_L",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListInput.Add(DownLimitVoltSP);

            OutUpLimitCurr = new RegisterFloat()
            {
                Address = 0x17,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Выходы за верхнюю уставку Iвых",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(OutUpLimitCurr);

            OutDownLimitCurr = new RegisterFloat()
            {
                Address = 0x18,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Выходы за нижнюю уставку Iвых",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(OutDownLimitCurr);

            OutUpLimitVolt = new RegisterFloat()
            {
                Address = 0x19,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Выходы за верхнюю уставку Uвых",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(OutUpLimitVolt);

            OutDownLimitVolt = new RegisterFloat()
            {
                Address = 0x1A,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Выходы за нижнюю уставку Uвых",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(OutDownLimitVolt);

            OutUpLimitVoltSP = new RegisterFloat()
            {
                Address = 0x1B,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Выходы за верхнюю уставку Uсп",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(OutUpLimitVoltSP);

            OutDownLimitVoltSP = new RegisterFloat()
            {
                Address = 0x1C,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Выходы за нижнюю уставку Uсп",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(OutDownLimitVoltSP);

            SummPotRMS = new RegisterFloat()
            {
                Address = 0x1D,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Суммарный потенциал RMS",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListInput.Add(SummPotRMS);


            RealTime = new RegisterRT()
            {
                Address = 0x20,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Текущее время сервера",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 2,
                Description = "Time_Now",
            };
            ListWriteControl.Add(RealTime);

            K_shunt = new RegisterFloat()
            {
                Address = 0x22,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Номинал шунта в Амперах",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 655f
            };
            ListWriteControl.Add(K_shunt);

            VoltControl = new RegisterFloat()
            {
                Address = 0x23,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напряжение управления СКЗ",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = 0f,
                MaxValue = 10f
            };
            ListWriteControl.Add(VoltControl);
        }


        public override Task RequestValue()
        {
            ReadRegisters(ListInput);
            ReadRegisters(ListWriteControl);
            return Task.CompletedTask;
        }

        public override Task StartRequestValue()
        {
            ReadRegisters(ListInput);
            ReadRegisters(ListWriteControl);
            return Task.CompletedTask;
        }

        protected override void CheckListRegister()
        {
            CheckReg(ListInput);
            CheckReg(ListWriteControl);
        }
    }
}
