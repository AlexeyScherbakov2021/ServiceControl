using Modbus.Data;
using Modbus.Message;
using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ServiceControl.Modbus.Devices
{
    internal class DeviceKIPUDZ : DeviceMaster
    {
        private readonly int cntArchive = 25;


        //public struct FlagsUDZ
        //{
        //    public bool isOpen;
        //    public bool isSummPot;
        //    public bool isPolPot;
        //    public bool isCurrPot;
        //    public bool isVoltageTR;
        //    public bool isDrenageCurr;
        //    public bool isResistTR;
        //    public bool isVolatge;
        //    public bool isTemperature;
        //}

        public class Archive
        {
            //public int time { get; set; }
            public string time { get; set; }
            public RegisterFloat VoltageTRa { get; set; }                // Напряжение труба/рельс
            public RegisterFloat CurrTRa { get; set; }                   // Ток труба/рельс
            public RegisterFloat IndicBITLefta { get; set; }             // Данные с индикатора тока БИТ (левый)
            public RegisterFloat IndicBITRighta { get; set; }            // Данные с индикатора тока БИТ (правый)
        }

        public RegisterRT RealTime { get; set; }        // Текущее системное время
        public RegisterInt WakeUp;                      // Время до следующего пробуждения в минутах
        public RegisterInt FlagsAlarm { get; set; }     // Регистр флагов сигнализации выхода за уставки
        public RegisterFloat PowerMax;                  // Уставка по максимальному напряжению питания 
        public RegisterFloat PowerMin;                  // Уставка по минимальному напряжению питания
        public RegisterFloat SummPotMax;                // Уставка по максимальному сумм.потенциалу
        public RegisterFloat SummPotMin;                // Уставка по минимальному сумм.потенциалу
        public RegisterFloat PolPotMax;                 // Уставка по максимальному пол.потенциалу
        public RegisterFloat PolPotMin;                 // Уставка по минимальному пол.потенциалу
        public RegisterFloat CurrPotMax;                // Уставка по максимальному току поляризации
        public RegisterFloat CurrPotMin;                // Уставка по минимальному току поляризации
        public RegisterFloat VoltTRMax;                 // Уставка по максимальному напряжению труба/рельс
        public RegisterFloat VoltTRMin;                 // Уставка по минимальному напряжению труба/рельс
        public RegisterFloat CurrTRMax;                 // Уставка по максимальному току труба/рельс
        public RegisterFloat CurrTRMin;                 // Уставка по минимальному току труба/рельс
        public RegisterFloat ResistTRMax;               // Уставка по максимальному сопротивлению труба/рельс
        public RegisterFloat ResistTRMin;               // Уставка по минимальному сопротивлению труба/рельс
        public RegisterFloat TemperMax;                 // Уставка по максимальной температуре в корпусе
        public RegisterFloat TemperMin;                 // Уставка по минимальной температуре в корпусе

        public RegisterInt Number;                      // Номер пакета
        public RegisterRT TimeNow;                      // временной маркер пакета, старшие 2 байта
        public RegisterInt Flags { get; set; }          // Флаги сработавших уставок
        public RegisterFloat Voltage;                   // Напряжение питания
        public RegisterFloat VoltageInduct;             // Наведенное напряжение
        public RegisterInt FreqInduct;                  // Частота наведенного напряжения
        public RegisterFloat SummPot;                   // Суммарный потенциал
        public RegisterFloat PolPot;                    // Поляризационный потенциал
        public RegisterFloat CurrPol;                   // Ток поляризации
        public RegisterFloat ResistTR;                  // сопротивление труба/рельс (65535 Ом - обрыв)
        public RegisterFloat Temper1;                   // Температурный датчик №1
        public RegisterFloat Temper2;                   // Температурный датчик №2
        public RegisterInt SpeedKorr;                   // Скорость коррозии
        public RegisterInt DeepKorr;                    // Глубина коррозии

        public RegisterFloat VoltageTR;                 // Напряжение труба/рельс
        public RegisterFloat CurrTR;                    // Ток труба/рельс
        public RegisterFloat IndicBITLeft;              // Данные с индикатора тока БИТ (левый)
        public RegisterFloat IndicBITRight;             // Данные с индикатора тока БИТ (правый)

        public List<Archive> listArchive { get; set; } = new List<Archive>();
        Archive dayArchive;
        //public Archive[] listArchive = new Archive[25];

        List<Register> ListInput = new List<Register>();
        List<Register> ListHolding = new List<Register>();
        //List<Register> ListAllArchive = new List<Register>();
        //List<Register> ListAll = new List<Register>();

        public DeviceKIPUDZ(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {

            RealTime = new RegisterRT()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Текущее время сервера",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 2,
                Description = "",
            };
            ListInput.Add(RealTime);

            WakeUp = new RegisterInt()
            {
                Address = 0x03,
                CodeFunc = ModbusFunc.InputRegister,
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
            ListInput.Add(WakeUp);

            FlagsAlarm = new RegisterInt()
            {
                Address = 0x04,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Регистр флагов",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Value = 0,
            };
            ListInput.Add(FlagsAlarm);

            PowerMax = new RegisterFloat()
            {
                Address = 0x05,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по напряжению",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 50f,
                Value = 40f,
            };
            ListInput.Add(PowerMax);

            PowerMin = new RegisterFloat()
            {
                Address = 0x06,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по напряжению",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 50f,
                Value = 5
            };
            ListInput.Add(PowerMin);

            SummPotMax = new RegisterFloat()
            {
                Address = 0x07,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по максимальному сумм.потенциалу",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f,
                Value = -0.9f
            };
            ListInput.Add(SummPotMax);

            SummPotMin = new RegisterFloat()
            {
                Address = 0x08,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по сумм.потенциалу",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f,
                Value = -3.5f

            };
            ListInput.Add(SummPotMin);

            PolPotMax = new RegisterFloat()
            {
                Address = 0x09,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по максимальному пол.потенциалу",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f,
                Value = -0.2f
            };
            ListInput.Add(PolPotMax);

            PolPotMin = new RegisterFloat()
            {
                Address = 0x0A,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по пол.потенциалу",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f,
                Value = -3
            };
            ListInput.Add(PolPotMin);

            CurrPotMax = new RegisterFloat()
            {
                Address = 0x0B,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по максимальному току поляризации",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -50f,
                MaxValue = 50f,
                Value = 40
            };
            ListInput.Add(CurrPotMax);

            CurrPotMin = new RegisterFloat()
            {
                Address = 0x0C,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по току поляризации",
                NameRes = "",
                Measure = "мА",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -50f,
                MaxValue = 50f,
                Value = -40
            };
            ListInput.Add(CurrPotMin);

            VoltTRMax = new RegisterFloat()
            {
                Address = 0x0D,
                CodeFunc = ModbusFunc.InputRegister,
                Name = " Уставка по максимальному напряжению труба/рельс",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -250f,
                MaxValue = 250f,
                Value = 240
            };
            ListInput.Add(VoltTRMax);

            VoltTRMin = new RegisterFloat()
            {
                Address = 0x0E,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по напряжению труба/рельс",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -250f,
                MaxValue = 250f,
                Value = -240
            };
            ListInput.Add(VoltTRMin);

            CurrTRMax = new RegisterFloat()
            {
                Address = 0x0F,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по максимальному току труба/рельс",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = -500f,
                MaxValue = 500f,
                Value = 450
            };
            ListInput.Add(CurrTRMax);

            CurrTRMin = new RegisterFloat()
            {
                Address = 0x10,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по току труба/рельс",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = -500f,
                MaxValue = 500f,
                Value = -450
            };
            ListInput.Add(CurrTRMin);

            ResistTRMax = new RegisterFloat()
            {
                Address = 0x11,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по максимальному сопротивлению труба/рельс",
                NameRes = "",
                Measure = "Ом",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 100f,
                Value = 80
            };
            ListInput.Add(ResistTRMax);

            ResistTRMin = new RegisterFloat()
            {
                Address = 0x12,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по сопротивлению труба/рельс",
                NameRes = "",
                Measure = "Ом",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = 0f,
                MaxValue = 100f,
                Value = 5
            };
            ListInput.Add(ResistTRMin);

            TemperMax = new RegisterFloat()
            {
                Address = 0x13,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по максимальной температуре в корпусе",
                NameRes = "",
                Measure = "°С",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = -55f,
                MaxValue = 125f,
                Value = 80
            };
            ListInput.Add(TemperMax);

            TemperMin = new RegisterFloat()
            {
                Address = 0x14,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Уставка по температуре в корпусе",
                NameRes = "",
                Measure = "°С",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = -55f,
                MaxValue = 125f,
                Value = -10
            };
            ListInput.Add(TemperMin);


            Number = new RegisterInt()
            {
                Address = 0x01,
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
            ListHolding.Add(Number);

            TimeNow = new RegisterRT()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "временной маркер пакета",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 2,
                Description = "",
            };
            ListHolding.Add(TimeNow);

            Flags = new RegisterInt()
            {
                Address = 0x04,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Флаги сработавших уставок",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding.Add(Flags);

            Voltage = new RegisterFloat()
            {
                Address = 0x05,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напряжение питания",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 50F
            };
            ListHolding.Add(Voltage);

            VoltageInduct = new RegisterFloat()
            {
                Address = 0x06,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Наведенное напряжение",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 100f
            };
            ListHolding.Add(VoltageInduct);

            FreqInduct = new RegisterInt()
            {
                Address = 0x07,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Частота наведенного напряжения",
                NameRes = "",
                Measure = "Гц",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 150
            };
            ListHolding.Add(FreqInduct);

            SummPot = new RegisterFloat()
            {
                Address = 0x08,
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
            ListHolding.Add(SummPot);

            PolPot = new RegisterFloat()
            {
                Address = 0x09,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Поляризационный потенциал",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -10f,
                MaxValue = 10f
            };
            ListHolding.Add(PolPot);

            CurrPol = new RegisterFloat()
            {
                Address = 0x0A,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток поляризации",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -50f,
                MaxValue = 50f
            };
            ListHolding.Add(CurrPol);

            VoltageTR = new RegisterFloat()
            {
                Address = 0x0B,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Напряжение труба/рельс",
                NameRes = "",
                Measure = "В",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = -250f,
                MaxValue = 250f
            };
            ListHolding.Add(VoltageTR);

            CurrTR = new RegisterFloat()
            {
                Address = 0x0C,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Ток труба/рельс",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = -500f,
                MaxValue = 500f
            };
            ListHolding.Add(CurrTR);

            ResistTR = new RegisterFloat()
            {
                Address = 0x0D,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "сопротивление труба/рельс",
                NameRes = "",
                Measure = "Ом",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.01f,
                MinValue = 0f,
                MaxValue = 100f
            };
            ListHolding.Add(ResistTR);

            Temper1 = new RegisterFloat()
            {
                Address = 0x0E,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Температурный датчик №1",
                NameRes = "",
                Measure = "°С",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = -55f,
                MaxValue = 125f
            };
            ListHolding.Add(Temper1);

            SpeedKorr = new RegisterInt()
            {
                Address = 0x0F,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Скорость коррозии",
                NameRes = "",
                Measure = "мкм/год",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding.Add(SpeedKorr);

            DeepKorr = new RegisterInt()
            {
                Address = 0x10,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Глубина коррозии",
                NameRes = "",
                Measure = "мкм",
                MeasureRes = "",
                Size = 1,
                Description = "",
                MinValue = 0,
                MaxValue = 65535
            };
            ListHolding.Add(DeepKorr);

            IndicBITLeft = new RegisterFloat()
            {
                Address = 0x11,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Данные с индикатора тока БИТ (левый)",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "Bit_L",
                Scale = 0.01f,
                MinValue = -320f,
                MaxValue = 320f
            };
            ListHolding.Add(IndicBITLeft);

            IndicBITRight = new RegisterFloat()
            {
                Address = 0x12,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Данные с индикатора тока БИТ (правый)",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Size = 1,
                Description = "Bit_R",
                Scale = 0.01f,
                MinValue = -320f,
                MaxValue = 320f
            };
            ListHolding.Add(IndicBITRight);

            Register reserv = new RegisterInt()
            {
                Address = 0x13,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,

            };
            ListHolding.Add(reserv);
            
            reserv = new RegisterInt()
            {
                Address = 0x14,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,

            };
            ListHolding.Add(reserv);
            
            reserv = new RegisterInt()
            {
                Address = 0x15,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,

            };
            ListHolding.Add(reserv);
            
            reserv = new RegisterInt()
            {
                Address = 0x16,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,

            };
            ListHolding.Add(reserv);


            Temper2 = new RegisterFloat()
            {
                Address = 0x17,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Температурный датчик №2",
                NameRes = "",
                Measure = "°С",
                MeasureRes = "",
                Size = 1,
                Description = "",
                Scale = 0.1f,
                MinValue = -55f,
                MaxValue = 125f
            };
            ListHolding.Add(Temper2);


            // добавление суточного среднего значения
            dayArchive = new Archive()
            {
                time = "текущее",
                VoltageTRa = new RegisterFloat()
                {
                    Address = 0x1A1,
                    CodeFunc = ModbusFunc.HoldingRegister,
                    Name = "Напряжение труба/рельс",
                    NameRes = "",
                    Measure = "В",
                    MeasureRes = "",
                    Size = 1,
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -250f,
                    MaxValue = 250f
                },

                CurrTRa = new RegisterFloat()
                {
                    Address = 0x1A2,
                    CodeFunc = ModbusFunc.HoldingRegister,
                    Name = "Ток труба/рельс",
                    NameRes = "",
                    Measure = "А",
                    MeasureRes = "",
                    Size = 1,
                    Description = "",
                    Scale = 0.1f,
                    MinValue = -500f,
                    MaxValue = 500f
                },
                IndicBITLefta = new RegisterFloat()
                {
                    Address = 0x1A3,
                    CodeFunc = ModbusFunc.HoldingRegister,
                    Name = "Данные с индикатора тока БИТ (левый)",
                    NameRes = "",
                    Measure = "А",
                    MeasureRes = "",
                    Size = 1,
                    Description = "Bit_L",
                    Scale = 0.01f,
                    MinValue = -320f,
                    MaxValue = 320f
                },

                IndicBITRighta = new RegisterFloat()
                {
                    Address = 0x1A4,
                    CodeFunc = ModbusFunc.HoldingRegister,
                    Name = "Данные с индикатора тока БИТ (правый)",
                    NameRes = "",
                    Measure = "А",
                    MeasureRes = "",
                    Size = 1,
                    Description = "Bit_R",
                    Scale = 0.01f,
                    MinValue = -320f,
                    MaxValue = 320f
                }
            };

            //ListAll.AddRange(ListInput);
            //ListAll.AddRange(ListHolding);

            ushort address = 0x21;
            for(int i = 1; i <= cntArchive; ++i)
            {
                ushort locAddr = address;
                Archive arc = new Archive()
                {
                    time = $"{i-1:00}-{i:00}",

                    VoltageTRa = new RegisterFloat()
                    {
                        Address = locAddr++,
                        CodeFunc = ModbusFunc.HoldingRegister,
                        Name = "Напряжение труба/рельс",
                        NameRes = "",
                        Measure = "В",
                        MeasureRes = "",
                        Size = 1,
                        Description = "",
                        Scale = 0.01f,
                        MinValue = -250f,
                        MaxValue = 250f
                    },

                    CurrTRa = new RegisterFloat()
                    {
                        Address = locAddr++,
                        CodeFunc = ModbusFunc.HoldingRegister,
                        Name = "Ток труба/рельс",
                        NameRes = "",
                        Measure = "А",
                        MeasureRes = "",
                        Size = 1,
                        Description = "",
                        Scale = 0.1f,
                        MinValue = -500f,
                        MaxValue = 500f
                    },
                    IndicBITLefta = new RegisterFloat()
                    {
                        Address = locAddr++,
                        CodeFunc = ModbusFunc.HoldingRegister,
                        Name = "Данные с индикатора тока БИТ (левый)",
                        NameRes = "",
                        Measure = "А",
                        MeasureRes = "",
                        Size = 1,
                        Description = "Bit_L",
                        Scale = 0.01f,
                        MinValue = -320f,
                        MaxValue = 320f
                    },

                    IndicBITRighta = new RegisterFloat()
                    {
                        Address = locAddr,
                        CodeFunc = ModbusFunc.HoldingRegister,
                        Name = "Данные с индикатора тока БИТ (правый)",
                        NameRes = "",
                        Measure = "А",
                        MeasureRes = "",
                        Size = 1,
                        Description = "Bit_R",
                        Scale = 0.01f,
                        MinValue = -320f,
                        MaxValue = 320f
                    }
                };
                listArchive.Add(arc);
                //ListAll.Add(arc.VoltageTRa);
                //ListAll.Add(arc.CurrTRa);
                //ListAll.Add(arc.IndicBITLefta);
                //ListAll.Add(arc.IndicBITRighta);
                address += 0x10;
            }
            listArchive.Last().time = "сутки";

            //modb.slave.ListenAsync();
        }

        protected override void CheckListRegister()
        {
            CheckReg(ListInput);
            CheckReg(ListHolding);
        }


        protected override void SetAllRegister()
        {
            ListAll.AddRange(ListInput);
            ListAll.AddRange(ListHolding);

            foreach(var arc in listArchive)
            {
                ListAll.Add(arc.CurrTRa);
                ListAll.Add(arc.VoltageTRa);
                ListAll.Add(arc.CurrTRa);
                ListAll.Add(arc.IndicBITLefta);
                ListAll.Add(arc.IndicBITRighta);
            }
        }

        //protected override void GetRegisterData(ushort StartAddress, ModbusFunc CodeFunc, ReadOnlyCollection<ushort> listData)
        //{
        //    Register reg;
        //    int index = 0;
        //    ushort currAddress = StartAddress;

        //    for (int n = 0; n < listData.Count; n++, currAddress++)
        //    {
        //        reg = ListAll.FirstOrDefault(it => it.Address == currAddress && it.CodeFunc == CodeFunc);
        //        if (reg != null)
        //        {
        //            ushort[] data = new ushort[reg.Size];
        //            for (int i = 0; i < reg.Size; i++, index++)
        //            {
        //                data[i] = listData[index];
        //            }
        //            reg.SetResultValues(data);
        //        }
        //    }
        //}


        //public override void SetRegister(Register reg)
        //{
        //    ModbusDataCollection<ushort> listReg;
        //    ModbusDataCollection<bool> listRegBool;

        //    if (modbus.slave == null) return;

        //    switch (reg.CodeFunc)
        //    {
        //        case ModbusFunc.InputRegister:
        //            listReg = modbus.slave.DataStore.InputRegisters;
        //            listRegBool = null;
        //            break;
        //        case ModbusFunc.HoldingRegister:
        //            listReg = modbus.slave.DataStore.HoldingRegisters;
        //            listRegBool = null;
        //            break;
        //        case ModbusFunc.Coil:
        //            listRegBool = modbus.slave.DataStore.CoilDiscretes;
        //            listReg = null;
        //            break;
        //        case ModbusFunc.InputDiscrete:
        //            listRegBool = modbus.slave.DataStore.InputDiscretes;
        //            listReg = null;
        //            break;
        //        default:
        //            listRegBool = null;
        //            listReg = null;
        //            break;
        //    }

        //    ushort[] values = reg.SetOutput();

        //    if (listReg != null)
        //    {
        //        for (int i = 0; i < values.Length; i++)
        //            listReg[reg.Address + i + 1] = values[i];
        //    }

        //    if (listRegBool != null)
        //    {
        //        //for (int i = 0; i < values.Length; i++)
        //        //    listRegBool[reg.Address + i + 1] = values[i];
        //    }
        //}

        //protected override void GetRegisterData(IModbusMessage message)
        //{
        //    if(message is HoldingRegisterRegistersRequest messWrite)
        //    {
        //        Register reg;
        //        //var n = message.Data.ToArray<ushort>();
        //        int index = 0;
        //        ushort currAddress = messWrite.StartAddress;

        //        for (int n = 0; n < messWrite.NumberOfPoints; n++, currAddress++)
        //        {
        //            reg = ListAll.FirstOrDefault(it => it.Address == currAddress && (byte)it.CodeFunc == message.FunctionCode);
        //            if(reg != null)
        //            {
        //                ushort[] data = new ushort[reg.Size];
        //                for (int i = 0; i < reg.Size; i++, index++)
        //                {
        //                    data[i] = messWrite.Data[index];
        //                }
        //                reg.SetResultValues(data);
        //            }
        //        }
        //    }

        //    if (message is ReadHoldingInputRegistersRequest messRead)
        //    {
        //        //Register reg;
        //        ////var n = message.Data.ToArray<ushort>();
        //        //int index = 0;
        //        //ushort currAddress = messRead.StartAddress;

        //        //for (int n = 0; n < messRead.NumberOfPoints; n++, currAddress++)
        //        //{
        //        //    reg = ListAll.FirstOrDefault(it => it.Address == currAddress);
        //        //    ushort[] data = new ushort[reg.Size];
        //        //    for (int i = 0; i < reg.Size; i++, index++)
        //        //    {
        //        //        data[i] = messRead.  .Data[index];
        //        //    }
        //        //    reg.SetResultValues(data);
        //        //}

        //    }


    }
}
