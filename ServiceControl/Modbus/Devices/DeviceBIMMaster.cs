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
    internal class DeviceBIMMaster : DeviceMaster
    {
        public RegisterInt Status { get; set; }         // Флаги состояний
        public RegisterInt ID;                          // идентификатор БИ
        public RegisterInt NumPacket;                   // номер передаваемого пакета
        public RegisterFloat VoltPower;                 // Напряжение питания БИ
        public RegisterFloat SummPot;                   // Суммарный потенциал
        public RegisterFloat PolPot;                    // Поляризационный потенциал
        public RegisterFloat CurrPot;                   // Ток поляризации
        public RegisterFloat SummPot2;                  // Суммарный потенциал 2
        public RegisterFloat PolPot2;                   // Поляризационный потенциал 2
        public RegisterFloat CurrPot2;                  // Ток поляризации 2
        public RegisterFloat VoltCurrOtnosL;            // Напряжение на токовом относе лев.
        public RegisterFloat VoltCurrOtnosR;            // Напряжение на токовом относе прав.
        public RegisterFloat DataCurrBIT_L;             // Данные с индикатора тока БИТ лев.
        public RegisterFloat DataCurrBIT_R;             // Данные с индикатора тока БИТ прав.
        public RegisterInt SpeedKorr;                   // скорость коррозии
        public RegisterInt DeepKorr;                    // глубина коррозии
        public RegisterInt SlaveID;                     // сетевой адрес slave устройства
        public RegisterRT RealTime { get; set; }        // Текущее системное время
        public RegisterInt WakeUp { get; set; }         // время до следующего пробуждения
        public RegisterRT TimeAwak { get; set; }        // время последнего пробуждения
        public RegisterFloat VoltOut;                   // Напряжение на выходе СКЗ
        public RegisterFloat CurrOut;                   // Ток на выходе СКЗ
        public RegisterFloat TemperBoard;               // Температура на плате
        public RegisterFloat ConstSumPot;               // постоянная составляющая суммарного потенциала
        public RegisterFloat VoltNaveden;               // Наведенное напряжение
        public RegisterFloat FreqVoltNaveden;           // Частота наведенного напряжение
        public RegisterFloat SummPotC;                  // Калибровочный коэффициент суммарного потенциала
        public RegisterFloat PolPotC;                   // Калибровочный коэффициент поляризационный потенциал
        public RegisterFloat CurrPotC;                  // Калибровочный коэффициент ток поляризации
        public RegisterFloat VoltOutC;                  // Калибровочный коэффициент напряжение на выходе СКЗ
        public RegisterFloat CurrOutC;                  // Калибровочный коэффициент ток на выходе СКЗ
        public RegisterFloat NominalShunt;              // Номинал шунта
        public RegisterInt ReadWriteCalb;               // чтение-запись калибровок
        public RegisterInt DenyCalibr;                  // запрет изменения калибровок
        public RegisterInt CntWriteEEPROM;              // количество циклов записи EEPROM
        public RegisterFloat ConstSumPot2;              // постоянная составляющая суммарного потенциала
        public RegisterFloat VoltNaveden2;              // Наведенное напряжение
        public RegisterInt FreqVoltNaveden2;            // Частота наведенного напряжение
        public RegisterFloat SummPotC2;                 // Калибровочный коэффициент суммарного потенциала
        public RegisterFloat PolPotC2;                  // Калибровочный коэффициент поляризационный потенциал
        public RegisterFloat CurrPotC2;                 // Калибровочный коэффициент ток поляризации
        public RegisterFloat ValtageOtnosC;             // калибровочный коэффициент канала измерения напряжения относа
        public RegisterFloat Bit_L_0;                   // калибровочный коэффициент
        public RegisterFloat Bit_L_Koef;                // калибровочный коэффициент
        public RegisterFloat Bit_R_0;                   // калибровочный коэффициент
        public RegisterFloat Bit_R_Koef;                // калибровочный коэффициент


        List<Register> ListHolding = new List<Register>();
        List<Register> ListHolding2 = new List<Register>();
        List<Register> ListHolding3 = new List<Register>();
        List<Register> ListHolding4 = new List<Register>();
        List<Register> ListHolding41 = new List<Register>();
        List<Register> ListHolding5 = new List<Register>();
        List<Register> ListHolding6 = new List<Register>();

        public DeviceBIMMaster(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {

            Status = new RegisterInt()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Флаги состояний",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding.Add(Status);

            ID = new RegisterInt()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Идентификатор БИ",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding.Add(ID);

            NumPacket = new RegisterInt()
            {
                Address = 0x03,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Номер пакета",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding.Add(NumPacket);

            VoltPower = new RegisterFloat()
            {
                Address = 0x04,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напряжение питания БИ",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.001f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListHolding.Add(VoltPower);

            SummPot = new RegisterFloat()
            {
                Address = 0x0E,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Суммарный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(SummPot);

            PolPot = new RegisterFloat()
            {
                Address = 0x0F,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Поляриз. потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(PolPot);

            CurrPot = new RegisterFloat()
            {
                Address = 0x10,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток поляризации",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -50f,
                MaxValue = 50f
            };
            ListHolding2.Add(CurrPot);

            SummPot2 = new RegisterFloat()
            {
                Address = 0x11,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Суммарный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(SummPot2);

            PolPot2 = new RegisterFloat()
            {
                Address = 0x12,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Поляриз. потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding2.Add(PolPot2);

            CurrPot2 = new RegisterFloat()
            {
                Address = 0x13,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток поляризации",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -50f,
                MaxValue = 50f
            };
            ListHolding2.Add(CurrPot2);

            VoltCurrOtnosL = new RegisterFloat()
            {
                Address = 0x14,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напр. на токовом относе лев.",
                NameRes = "",
                Measure = "мВ",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -100f,
                MaxValue = 100f
            };
            ListHolding2.Add(VoltCurrOtnosL);

            VoltCurrOtnosR = new RegisterFloat()
            {
                Address = 0x15,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напр. на токовом относе прав.",
                NameRes = "",
                Measure = "мВ",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -100f,
                MaxValue = 100f
            };
            ListHolding2.Add(VoltCurrOtnosR);

            DataCurrBIT_L = new RegisterFloat()
            {
                Address = 0x16,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Индик. тока БИТ лев.",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -320f,
                MaxValue = 320f
            };
            ListHolding2.Add(DataCurrBIT_L);

            DataCurrBIT_R = new RegisterFloat()
            {
                Address = 0x17,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Индик. тока БИТ прав.",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -320f,
                MaxValue = 320f
            };
            ListHolding2.Add(DataCurrBIT_R);

            SpeedKorr = new RegisterInt()
            {
                Address = 0x1D,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Скорость коррозии",
                NameRes = "",
                Measure = "мкм/год",
                MeasureRes = "",
                Size = 1,
                Description = "СК_ИКП1",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding3.Add(SpeedKorr);

            DeepKorr = new RegisterInt()
            {
                Address = 0x1E,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Глубина коррозии",
                NameRes = "",
                Measure = "мкм",
                MeasureRes = "",
                Size = 1,
                Description = "ГК_ИКП1",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding3.Add(DeepKorr);

            SlaveID = new RegisterInt()
            {
                Address = 0x30,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Сетевой адрес БИ",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "Bi_addr",
                MinValue = 0,
                MaxValue = 254
            };
            ListHolding41.Add(SlaveID);

            RealTime = new RegisterRT()
            {
                Address = 0x31,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Системное время",
                NameRes = "TimeDevice",
                Measure = "",
                MeasureRes = "",
                Size = 2,
                Description = "РВ",
            };
            ListHolding41.Add(RealTime);

            WakeUp = new RegisterInt()
            {
                Address = 0x33,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Время до пробуждения",
                NameRes = "",
                Measure = "мин.",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535,
                Value = 0,
            };
            ListHolding41.Add(WakeUp);

            RegisterInt reserv = new RegisterInt()
            {
                Address = 0x34,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Резерв",
                Size = 1,
            };
            ListHolding41.Add(reserv);

            TimeAwak = new RegisterRT()
            {
                Address = 0x35,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Время посл. пробуждения",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 2,
                Description = "",
            };
            ListHolding4.Add(TimeAwak);

            VoltOut = new RegisterFloat()
            {
                Address = 0x62,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напр. на выходе СКЗ",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uвых",
                Scale = 0.01f,
                MinValue = -100f,
                MaxValue = 100f
            };
            ListHolding5.Add(VoltOut);

            CurrOut = new RegisterFloat()
            {
                Address = 0x63,
                CodeFunc = ModbusFunc.HoldingRegister,
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
            ListHolding5.Add(CurrOut);

            TemperBoard = new RegisterFloat()
            {
                Address = 0x64,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Температ. на плате",
                NameRes = "",
                Measure = "°С",
                MeasureRes = "",
                Size = 1,
                Description = "T°",
                Scale = 0.1f,
                MinValue = -55f,
                MaxValue = 125f
            };
            ListHolding5.Add(TemperBoard);

            ConstSumPot = new RegisterFloat()
            {
                Address = 0x65,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Постоянная сумм. потенциала",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -5f,
                MaxValue = 5f
            };
            ListHolding5.Add(ConstSumPot);

            VoltNaveden = new RegisterFloat()
            {
                Address = 0x66,
                CodeFunc = ModbusFunc.HoldingRegister,
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
            ListHolding5.Add(VoltNaveden);

            FreqVoltNaveden = new RegisterFloat()
            {
                Address = 0x67,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Частота наведенного напр.",
                NameRes = "",
                Measure = "Гц",
                MeasureRes = "",
                Size = 1,
                Description = "F1",
                Scale = 1f,
                MinValue = 0f,
                MaxValue = 160f
            };
            ListHolding5.Add(FreqVoltNaveden);

            SummPotC = new RegisterFloat()
            {
                Address = 0x68,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Суммарного потенциала",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListHolding5.Add(SummPotC);

            PolPotC = new RegisterFloat()
            {
                Address = 0x69,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Поляризац. потенциала",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListHolding5.Add(PolPotC);

            CurrPotC = new RegisterFloat()
            {
                Address = 0x6A,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток поляризации",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListHolding5.Add(CurrPotC);

            VoltOutC = new RegisterFloat()
            {
                Address = 0x6B,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напряжение на выходе СКЗ",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListHolding5.Add(VoltOutC);

            CurrOutC = new RegisterFloat()
            {
                Address = 0x6C,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток на выходе СКЗ",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListHolding5.Add(CurrOutC);

            NominalShunt = new RegisterFloat()
            {
                Address = 0x6D,
                CodeFunc = ModbusFunc.HoldingRegister,
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
            ListHolding5.Add(NominalShunt);

            ReadWriteCalb = new RegisterInt()
            {
                Address = 0x6E,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Чтение-запись калибровок",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding5.Add(ReadWriteCalb);

            DenyCalibr = new RegisterInt()
            {
                Address = 0x6F,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Запрет изменения калибровок",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding5.Add(DenyCalibr);

            CntWriteEEPROM = new RegisterInt()
            {
                Address = 0x70,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Кол. циклов записи EEPROM",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding5.Add(CntWriteEEPROM);



            ConstSumPot2 = new RegisterFloat()
            {
                Address = 0x75,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Пост. составляющая сумм. потенциала 2",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 10f
            };
            ListHolding6.Add(ConstSumPot2);

            VoltNaveden2 = new RegisterFloat()
            {
                Address = 0x76,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Наведенное напряжение 2",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "Uпп2",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 150f
            };
            ListHolding6.Add(VoltNaveden2);

            FreqVoltNaveden2 = new RegisterInt()
            {
                Address = 0x77,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Частота наведенного напр. 2",
                NameRes = "",
                Measure = "Гц",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 160
            };
            ListHolding6.Add(FreqVoltNaveden2);

            SummPotC2 = new RegisterFloat()
            {
                Address = 0x78,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Суммарного потенциала 2",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListHolding6.Add(SummPotC2);

            PolPotC2 = new RegisterFloat()
            {
                Address = 0x79,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Поляриз. потенциала 2",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListHolding6.Add(PolPotC2);

            CurrPotC2 = new RegisterFloat()
            {
                Address = 0x7A,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток поляризации 2",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListHolding6.Add(CurrPotC2);

            ValtageOtnosC = new RegisterFloat()
            {
                Address = 0x7B,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Канал измерения напр. относа",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535f
            };
            ListHolding6.Add(ValtageOtnosC);

            Bit_L_0 = new RegisterFloat()
            {
                Address = 0x7C,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Индик. тока БИТ лев.",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding6.Add(Bit_L_0);

            Bit_L_Koef = new RegisterFloat()
            {
                Address = 0x7D,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Индик.тока БИТ лев.коэф.",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding6.Add(Bit_L_Koef);

            Bit_R_0 = new RegisterFloat()
            {
                Address = 0x7E,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Индик. тока БИТ прав.",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding6.Add(Bit_R_0);

            Bit_R_Koef = new RegisterFloat()
            {
                Address = 0x7F,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Индик.тока БИТ прав.коэф.",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.0001f,
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding6.Add(Bit_R_Koef);

        }

        //public override Task RequestValue()
        //{
        //    ReadRegisters(ListHolding);
        //    return Task.CompletedTask;
        //}

        //public override Task StartRequestValue()
        //{
        //    ReadRegisters(ListInput);
        //    ReadRegisters(ListInput2);
        //    ReadRegisters(ListWriteControl);
        //    return Task.CompletedTask;
        //}

        protected override void CheckListRegister()
        {
            CheckReg(ListHolding);
            CheckReg(ListHolding2);
            CheckReg(ListHolding3);
            CheckReg(ListHolding4);
            CheckReg(ListHolding41);
            CheckReg(ListHolding5);
            CheckReg(ListHolding6);
        }

        protected override void SetAllRegister()
        {
            ListAll.AddRange(ListHolding);
            ListAll.AddRange(ListHolding2);
            ListAll.AddRange(ListHolding3);
            ListAll.AddRange(ListHolding4);
            ListAll.AddRange(ListHolding5);
            ListAll.AddRange(ListHolding6);
        }


    }
}
