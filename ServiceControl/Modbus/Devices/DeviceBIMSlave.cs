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
    internal class DeviceBIMSlave : Device
    {
        public RegisterInt Status { get; set; }         // Флаги состояний
        public RegisterRT RealTime { get; set; }        // Текущее системное время
        public RegisterFloat NominalShunt;              // Номинал шунта
        public RegisterFloat SummPot;                   // Суммарный потенциал
        public RegisterFloat PolPot;                    // Поляризационный потенциал
        public RegisterFloat CurrPot;                   // Ток поляризации
        public RegisterFloat VoltOut;                   // Напряжение на выходе СКЗ
        public RegisterFloat CurrOut;                   // Ток на выходе СКЗ
        public RegisterFloat VoltNaveden;               // Наведенное напряжение
        public RegisterFloat FreqVoltNaveden;           // Частота наведенного напряжение
        public RegisterFloat TemperBoard;               // Температура на плате
        public RegisterFloat SpeedKorr;                 // Скорость коррозии
        public RegisterFloat DeepKorr;                  // Глубина коррозии
        public RegisterFloat SummPot2;                  // Суммарный потенциал 2
        public RegisterFloat PolPot2;                   // Поляризационный потенциал 2
        public RegisterFloat CurrPot2;                  // Ток поляризации 2
        public RegisterFloat VoltNaveden2;              // Наведенное напряжение 2
        public RegisterFloat FreqVoltNaveden2;          // Частота наведенного напряжение 2
        public RegisterFloat VoltCurrOtkosL;            // Напряжение на токовом относе лев.
        public RegisterFloat VoltCurrOtkosR;            // Напряжение на токовом относе прав.
        public RegisterFloat DataCurrBIT_L;             // Данные с индикатора тока БИТ лев.
        public RegisterFloat DataCurrBIT_R;             // Данные с индикатора тока БИТ прав.
        public RegisterFloat VoltPower;                 // Напряжение питания БИ

        public RegisterFloat Bi_addr;                   // Сетевой адрес БИ
        public RegisterRT TimeNow;                   // Текущее время сервера
        public RegisterFloat K_shunt;                   // Номинал шунта в Амперах

        List<Register> ListInput = new List<Register>();
        List<Register> ListInput2 = new List<Register>();
        //List<Register> ListInput3 = new List<Register>();
        List<Register> ListWriteControl = new List<Register>();

        public DeviceBIMSlave(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {

            Status = new RegisterInt()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Флаги состояний",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 999999
            };
            ListInput.Add(Status);

            RealTime = new RegisterRT()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Время устройства",
                NameRes = "TimeDevice",
                Measure = "сек",
                MeasureRes = "SEC",
                Size = 2,
                Description = "РВ",
                MinValue = 0,
                MaxValue = int.MaxValue
            };
            ListInput.Add(RealTime);

            NominalShunt = new RegisterFloat()
            {
                Address = 0x04,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Номинал шунта",
                NameRes = "",
                Measure = "А",
                MeasureRes = "OutCur",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListInput.Add(NominalShunt);

            SummPot = new RegisterFloat()
            {
                Address = 0x0B,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Суммарный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uсп1",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListInput2.Add(SummPot);
            
            PolPot = new RegisterFloat()
            {
                Address = 0x0C,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Поляризационный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uпп1",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListInput2.Add(PolPot);

            CurrPot = new RegisterFloat()
            {
                Address = 0x0D,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Ток поляризации",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "Iпол1",
                Scale = 0.01f,
                MinValue = -50f,
                MaxValue = 50f
            };
            ListInput2.Add(CurrPot);

            VoltOut = new RegisterFloat()
            {
                Address = 0x0E,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Напряжение на выходе СКЗ",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uвых",
                Scale = 0.01f,
                MinValue = -100f,
                MaxValue = 100f
            };
            ListInput2.Add(VoltOut);

            CurrOut = new RegisterFloat()
            {
                Address = 0x0F,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Ток на выходе СКЗ",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "Iвых",
                Scale = 0.01f,
                MinValue = -320f,
                MaxValue = 320f
            };
            ListInput2.Add(CurrOut);

            VoltNaveden = new RegisterFloat()
            {
                Address = 0x10,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Наведенное напряжение",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uнав1",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 150f
            };
            ListInput2.Add(VoltNaveden);

            FreqVoltNaveden = new RegisterFloat()
            {
                Address = 0x11,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Частота наведенного напряжение",
                NameRes = "",
                Measure = "Гц",
                MeasureRes = "",
                Size = 1,
                Description = "F1",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 160f
            };
            ListInput2.Add(FreqVoltNaveden);

            TemperBoard = new RegisterFloat()
            {
                Address = 0x12,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Температура на плате",
                NameRes = "",
                Measure = "°С",
                MeasureRes = "",
                Size = 1,
                Description = "T°",
                Scale = 0.1f,
                MinValue = -55f,
                MaxValue = 125f
            };
            ListInput2.Add(TemperBoard);

            RegisterFloat reserv = new RegisterFloat()
            {
                Address = 0x13,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Резерв",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 1f,
                MinValue = -100f,
                MaxValue = 100f
            };
            ListInput2.Add(reserv);

            SpeedKorr = new RegisterFloat()
            {
                Address = 0x14,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Скорость коррозии",
                NameRes = "",
                Measure = "мкм/год",
                MeasureRes = "",
                Size = 1,
                Description = "СК_ИКП1",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput2.Add(SpeedKorr);

            DeepKorr = new RegisterFloat()
            {
                Address = 0x15,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Глубина коррозии",
                NameRes = "",
                Measure = "мкм",
                MeasureRes = "",
                Size = 1,
                Description = "ГК_ИКП1",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput2.Add(DeepKorr);

            SummPot2 = new RegisterFloat()
            {
                Address = 0x16,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Суммарный потенциал 2",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uсп2",
                Scale = 100f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListInput2.Add(SummPot2);

            PolPot2 = new RegisterFloat()
            {
                Address = 0x17,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Поляризационный потенциал 2",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uпп2",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListInput2.Add(PolPot2);

            CurrPot2 = new RegisterFloat()
            {
                Address = 0x18,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Ток поляризации 2",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "Iпол2",
                Scale = 0.01f,
                MinValue = -50f,
                MaxValue = 50f
            };
            ListInput2.Add(CurrPot2);

            VoltNaveden2 = new RegisterFloat()
            {
                Address = 0x19,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Наведенное напряжение 2",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uнав2",
                Scale = 0.1f,
                MinValue = 0f,
                MaxValue = 150f
            };
            ListInput2.Add(VoltNaveden2);

            FreqVoltNaveden2 = new RegisterFloat()
            {
                Address = 0x1A,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Частота наведенного напряжение",
                NameRes = "",
                Measure = "Гц",
                MeasureRes = "",
                Size = 1,
                Description = "F2",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 160f
            };
            ListInput2.Add(FreqVoltNaveden2);

            VoltCurrOtkosL = new RegisterFloat()
            {
                Address = 0x1B,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Напряжение на токовом относе лев.",
                NameRes = "",
                Measure = "мВ",
                MeasureRes = "",
                Size = 1,
                Description = "V_относ_L",
                Scale = 0.1f,
                MinValue = -2000f,
                MaxValue = 2000f
            };
            ListInput2.Add(VoltCurrOtkosL);

            VoltCurrOtkosR = new RegisterFloat()
            {
                Address = 0x1C,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Напряжение на токовом относе пр.",
                NameRes = "",
                Measure = "мВ",
                MeasureRes = "",
                Size = 1,
                Description = "V_относ_R",
                Scale = 0.1f,
                MinValue = -2000f,
                MaxValue = 2000f
            };
            ListInput2.Add(VoltCurrOtkosR);

            DataCurrBIT_L = new RegisterFloat()
            {
                Address = 0x1D,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Данные с индикатора тока БИТ лев.",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "Bit_L",
                Scale = 0.01f,
                MinValue = -320f,
                MaxValue = 320f
            };
            ListInput2.Add(DataCurrBIT_L);

            DataCurrBIT_R = new RegisterFloat()
            {
                Address = 0x1E,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Данные с индикатора тока БИТ пр.",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "Bit_R",
                Scale = 0.01f,
                MinValue = -320f,
                MaxValue = 320f
            };
            ListInput2.Add(DataCurrBIT_R);

            VoltPower = new RegisterFloat()
            {
                Address = 0x1F,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Напряжение питания БИ",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "V_относ_R",
                Scale = 0.001f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput2.Add(VoltPower);


            Bi_addr = new RegisterFloat()
            {
                Address = 0x1001,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Сетевой адрес БИ",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "Bi_addr",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListWriteControl.Add(Bi_addr);

            TimeNow = new RegisterRT()
            {
                Address = 0x1002,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Текущее время сервера",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 2,
                Description = "Time_Now",
            };
            ListWriteControl.Add(TimeNow);

            K_shunt = new RegisterFloat()
            {
                Address = 0x1004,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Номинал шунта в Амперах",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "K_shunt",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListWriteControl.Add(K_shunt);


        }


        public override Task RequestValue()
        {
            ReadRegisters(ListInput);
            ReadRegisters(ListInput2);
            return Task.CompletedTask;
        }

        public override Task StartRequestValue()
        {
            ReadRegisters(ListInput);
            ReadRegisters(ListInput2);
            return Task.CompletedTask;
        }

        protected override void CheckListRegister()
        {
            CheckReg(ListInput);
            CheckReg(ListInput2);
        }
    }
}
