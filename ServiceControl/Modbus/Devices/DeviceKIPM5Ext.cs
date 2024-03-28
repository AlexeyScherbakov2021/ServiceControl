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
    internal class DeviceKIPM5Ext : DeviceSlave
    {
        public RegisterInt Address;                     // установка адреса устройства
        public RegisterInt AddressSet;                  // установка адреса устройства
        public RegisterFloat Shunt;                     // шунт
        public RegisterFloat ShuntSet;                  // установка шунта
        public RegisterInt Flags;                       // Флаги работы КИП
        public RegisterFloat CurrOut;                   // Ток на выходе СКЗ
        public RegisterFloat VoltOut;                   // Напряжение на выходе СКЗ
        public RegisterFloat SummPotRMS;                // постоянная составляющая Суммарного потенциала
        public RegisterFloat SummPot;                   // Суммарный потенциал
        public RegisterFloat PolPot;                    // Поляризационный потенциал
        public RegisterFloat CurrPot;                   // Ток поляризации
        public RegisterFloat VoltNaveden;               // Наведенное напряжение
        public RegisterFloat FreqVoltNaveden;           // Частота наведенного напряжение
        public RegisterFloat SummPotRMS2;               // Суммарный потенциал
        public RegisterFloat SummPot2;                  // Суммарный потенциал
        public RegisterFloat PolPot2;                   // Поляризационный потенциал
        public RegisterFloat CurrPot2;                  // Ток поляризации
        public RegisterFloat VoltNaveden2;              // Наведенное напряжение
        public RegisterFloat FreqVoltNaveden2;          // Частота наведенного напряжение
        public RegisterFloat SummPotRMS3;               // Суммарный потенциал
        public RegisterFloat SummPot3;                  // Суммарный потенциал
        public RegisterFloat PolPot3;                   // Поляризационный потенциал
        public RegisterFloat CurrPot3;                  // Ток поляризации
        public RegisterFloat VoltNaveden3;              // Наведенное напряжение
        public RegisterFloat FreqVoltNaveden3;          // Частота наведенного напряжение

        List<Register> ListHolding = new List<Register>();
        List<Register> ListHolding2 = new List<Register>();

        public DeviceKIPM5Ext(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {
            AddressSet = new RegisterInt()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Установка адреса устройства",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 255
            };

            Address = new RegisterInt()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Установка адреса устройства",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 255
            };
            ListHolding.Add(Address);

            Shunt = new RegisterFloat()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Номинал шунта",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Scale = 0.01f,
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 1500
            };
            ListHolding.Add(Shunt);

            ShuntSet = new RegisterFloat()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Установка номинала шунта",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Scale = 0.01f,
                Description = "",
                MinValue = 0,
                MaxValue = 1500
            };

            Flags = new RegisterInt()
            {
                Address = 0x30,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Флаги работы КИП",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding2.Add(Flags);

            CurrOut = new RegisterFloat()
            {
                Address = 0x31,
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
            ListHolding2.Add(CurrOut);

            VoltOut = new RegisterFloat()
            {
                Address = 0x32,
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
            ListHolding2.Add(VoltOut);

            SummPotRMS = new RegisterFloat()
            {
                Address = 0x33,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Постоянная составляющая сумм.потнц.",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(SummPotRMS);

            SummPot = new RegisterFloat()
            {
                Address = 0x34,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Суммарный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(SummPot);

            PolPot = new RegisterFloat()
            {
                Address = 0x35,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Поляризационный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(PolPot);

            CurrPot = new RegisterFloat()
            {
                Address = 0x36,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток поляризации",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -50f,
                MaxValue = 50f
            };
            ListHolding2.Add(CurrPot);

            VoltNaveden = new RegisterFloat()
            {
                Address = 0x37,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Наведенное напряжение",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = 0f,
                MaxValue = 150f
            };
            ListHolding2.Add(VoltNaveden);

            FreqVoltNaveden = new RegisterFloat()
            {
                Address = 0x38,
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
            ListHolding2.Add(FreqVoltNaveden);

            SummPotRMS2 = new RegisterFloat()
            {
                Address = 0x39,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Постоянная составляющая Суммарного потенциала",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(SummPotRMS2);

            SummPot2 = new RegisterFloat()
            {
                Address = 0x3A,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Суммарный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(SummPot2);

            PolPot2 = new RegisterFloat()
            {
                Address = 0x3B,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Поляризационный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(PolPot2);

            CurrPot2 = new RegisterFloat()
            {
                Address = 0x3C,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток поляризации",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -50f,
                MaxValue = 50f
            };
            ListHolding2.Add(CurrPot2);

            VoltNaveden2 = new RegisterFloat()
            {
                Address = 0x3D,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Наведенное напряжение",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = 0f,
                MaxValue = 150f
            };
            ListHolding2.Add(VoltNaveden2);

            FreqVoltNaveden2 = new RegisterFloat()
            {
                Address = 0x3E,
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
            ListHolding2.Add(FreqVoltNaveden2);

            SummPotRMS3 = new RegisterFloat()
            {
                Address = 0x3F,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "постоянная составляющая Суммарного потенциала",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(SummPotRMS3);

            SummPot3 = new RegisterFloat()
            {
                Address = 0x40,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Суммарный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(SummPot3);

            PolPot3 = new RegisterFloat()
            {
                Address = 0x41,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Поляризационный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(PolPot3);

            CurrPot3 = new RegisterFloat()
            {
                Address = 0x42,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток поляризации",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = -50f,
                MaxValue = 50f
            };
            ListHolding2.Add(CurrPot3);

            VoltNaveden3 = new RegisterFloat()
            {
                Address = 0x43,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Наведенное напряжение",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = 0f,
                MaxValue = 150f
            };
            ListHolding2.Add(VoltNaveden3);

            FreqVoltNaveden3 = new RegisterFloat()
            {
                Address = 0x44,
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
            ListHolding2.Add(FreqVoltNaveden3);

        }


        public override Task RequestValue()
        {
            ReadRegisters(ListHolding);
            ReadRegisters(ListHolding2);
            return Task.CompletedTask;
        }

        public override Task StartRequestValue()
        {
            ReadRegister(AddressSet);
            ReadRegister(ShuntSet);
            ReadRegisters(ListHolding);
            ReadRegisters(ListHolding2);
            return Task.CompletedTask;
        }

        protected override void CheckListRegister()
        {
            CheckReg(ListHolding);
            CheckReg(ListHolding2);
        }
    }
}
