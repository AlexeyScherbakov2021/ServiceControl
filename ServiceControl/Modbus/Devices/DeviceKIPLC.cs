using Modbus.Message;
using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ServiceControl.Modbus.Devices
{
    internal class  DeviceKIPLC : DeviceMaster
    {
        public RegisterInt NumberPacket;                // Номер передаваемого пакета  
        public RegisterFloat VoltPower;                 // Напряжение питания БИ
        public RegisterInt FlagsStatus;                 // Неисправность, вскрытие, обрыв цепей, ДК (датчики коррозии)
        public RegisterFloat SummPot1;                  // Суммарный потенциал 1
        public RegisterFloat PolPot1;                   // Поляризационный потенциал 1
        public RegisterFloat CurrPot1;                  // Ток поляризации 1
        public RegisterFloat VoltNaveden1;              // Переменное наведенное напряжение на сооружении
        public RegisterInt FreqVoltNaveden1;            // Частота наведенного напряжение
        public RegisterRT RealTime { get; set; }        // Текущее системное время сервера
        public RegisterFloat KoefSummPot1;              // Калибровочный коэффициент канала измерения суммарного потенциала
        public RegisterFloat KoefPolPot1;               // Калибровочный коэффициент канала измерения поляризационного потенциала
        public RegisterFloat KoCurrPot1;                // Калибровочный коэффициент канала измерения тока поляризации
        public RegisterInt AddressBIM;                  // Сетевой адрес подключенного Slave-устройства сбора данных (БИ-М) 
        public RegisterInt AddressBIMChange;            // Смена адреса подключенного Slave-устройства сбора данных (1 - сохранение адреса) 
        public RegisterInt SetID_BI;                    // Установка идентификатора БИ 

        List<Register> ListInput = new List<Register>();
        List<Register> ListInput2 = new List<Register>();
        List<Register> ListHolding = new List<Register>();
        List<Register> ListHolding2 = new List<Register>();

        public DeviceKIPLC(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {

            NumberPacket = new RegisterInt()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Номер передаваемого пакета",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding.Add(NumberPacket);

            VoltPower = new RegisterFloat()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напряжение питания БИ",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 656f
            };
            ListHolding.Add(VoltPower);

            FlagsStatus = new RegisterInt()
            {
                Address = 0x03,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напряжение на выходе СКЗ",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
            };
            ListHolding.Add(FlagsStatus);

            SummPot1 = new RegisterFloat()
            {
                Address = 0x10,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Суммарный потенциал 1",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(SummPot1);

            PolPot1 = new RegisterFloat()
            {
                Address = 0x11,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Поляризационный потенциал 1",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(PolPot1);

            CurrPot1 = new RegisterFloat()
            {
                Address = 0x12,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток поляризации 1",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "Iпол1",
                Scale = 0.01f,
                MinValue = -50f,
                MaxValue = 50f
            };
            ListHolding2.Add(CurrPot1);

            VoltNaveden1 = new RegisterFloat()
            {
                Address = 0x13,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Наведенное напряжение",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(VoltNaveden1);

            FreqVoltNaveden1 = new RegisterInt()
            {
                Address = 0x14,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Частота наведенного напряжение",
                NameRes = "",
                Measure = "Гц",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 160
            };
            ListHolding2.Add(FreqVoltNaveden1);

            RealTime = new RegisterRT()
            {
                Address = 0x30,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Текущее время сервера",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 2,
                Description = "Time_Now",
            };
            ListInput.Add(RealTime);

            Register reserv = new RegisterInt()
            {
                Address = 0x32,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
            };
            ListInput.Add(reserv);


            KoefSummPot1 = new RegisterFloat()
            {
                Address = 0x33,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Калибровочный коэффициент сумм. потенциала",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(KoefSummPot1);

            KoefPolPot1 = new RegisterFloat()
            {
                Address = 0x34,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Калибровочный коэффициент пол. потенциала",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(KoefPolPot1);

            KoCurrPot1 = new RegisterFloat()
            {
                Address = 0x35,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Калибровочный коэффициент тока поляризации",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0f,
                MaxValue = 65535f
            };
            ListInput.Add(KoCurrPot1);

            AddressBIM = new RegisterInt()
            {
                Address = 0x3F,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Slave-ID",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 254
            };
            ListInput2.Add(AddressBIM);

            AddressBIMChange = new RegisterInt()
            {
                Address = 0x40,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Смена адреса Slave-ID",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListInput2.Add(AddressBIMChange);

            SetID_BI = new RegisterInt()
            {
                Address = 0x41,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Установка ID БИ",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListInput2.Add(SetID_BI);

        }


        //public override Task RequestValue()
        //{
        //    ReadRegisters(ListInput);
        //    ReadRegisters(ListInput2);
        //    ReadRegisters(ListHolding);
        //    ReadRegisters(ListHolding2);
        //    return Task.CompletedTask;
        //}

        //public override Task StartRequestValue()
        //{
        //    ReadRegisters(ListInput);
        //    ReadRegisters(ListInput2);
        //    ReadRegisters(ListHolding);
        //    ReadRegisters(ListHolding2);
        //    return Task.CompletedTask;
        //}

        protected override void CheckListRegister()
        {
            CheckReg(ListInput);
            CheckReg(ListInput2);
            CheckReg(ListHolding);
            CheckReg(ListHolding2);
        }

        protected override void GetRegisterData(ushort StartAddress, ModbusFunc CodeFunc, ReadOnlyCollection<ushort> listData)
        {
        }

        public override void SetRegister(Register reg)
        {
            throw new NotImplementedException();
        }
    }
}
