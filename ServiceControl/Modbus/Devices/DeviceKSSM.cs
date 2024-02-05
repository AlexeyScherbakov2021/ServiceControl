using ServiceControl.Based;
using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using System.Xml.Linq;

namespace ServiceControl.Modbus.Devices
{

    internal class KIP_KSSM : Observable
    {
        public int Number { get; set; }
        public RegisterInt AddresBI { get; set; }                     // Адрес БИ
        public RegisterInt StatusFlags { get; set; }                  // Флаги состояния
        public RegisterRT TimeBI { get; set; }                        // Время БИ
        public RegisterInt PeriodRead { get; set; }                   // Период опроса
        public RegisterInt Shunt { get; set; }                        // Номинал шунта в Амперах
        public RegisterFloat TemperBoard { get; set; }                // Температура на плате
        public RegisterFloat CurrProtect { get; set; }                // Ток защиты
        public RegisterFloat VoltOut { get; set; }                    // Напряжение на выходе СКЗ
        public RegisterFloat CurrPol1 { get; set; }                   // Ток поляризации 1
        public RegisterFloat PolPot1 { get; set; }                    // Поляризационный потенциал 1
        public RegisterFloat SummPot1 { get; set; }                   // Суммарный потенциал 1
        public RegisterFloat VoltNaveden1 { get; set; }               // Наведенное напряжение 1
        public RegisterInt FreqVoltNaveden1 { get; set; }             // Частота наведенного напряжения 1
        public RegisterFloat CurrPol2 { get; set; }                   // Ток поляризации 2
        public RegisterFloat PolPot2 { get; set; }                    // Поляризационный потенциал 2
        public RegisterFloat SummPot2 { get; set; }                   // Суммарный потенциал 2
        public RegisterFloat VoltNaveden2 { get; set; }               // Наведенное напряжение 2
        public RegisterInt FreqVoltNaveden2 { get; set; }             // Частота наведенного напряжение 2
        public RegisterFloat VoltOtnos1 { get; set; }                 // Напряжение на токовом относе 1
        public RegisterFloat VoltOtnos2 { get; set; }                 // Напряжение на токовом относе 2
        public RegisterFloat BIT1 { get; set; }                       // Данные с индикатора тока БИТ 1
        public RegisterFloat BIT2 { get; set; }                       // Данные с индикатора тока БИТ 2
        public RegisterFloat VoltagePower { get; set; }               // Напряжение питания
        public RegisterInt SpeedKorr { get; set; }                    // Скорость коррозии
        public RegisterInt DeepKorr { get; set; }                     // Глубина коррозии

        private bool _isOpen;
        public bool isOpen { get => _isOpen; set { Set(ref _isOpen, value); } }
        
        private bool _isError;
        public bool isError { get => _isError; set { Set(ref _isError, value); } }
        
        private bool _isDK1;
        public bool isDK1 { get => _isDK1; set { Set(ref _isDK1, value); } }

        private bool _isDK2;
        public bool isDK2 { get => _isDK2; set { Set(ref _isDK2, value); } }

        private bool _isDK3;
        public bool isDK3 { get => _isDK3; set { Set(ref _isDK3, value); } }

        public ObservableCollection<Register> listInput { get; set; } = new ObservableCollection<Register>();
        public ObservableCollection<Register> listInput1 { get; set; } = new ObservableCollection<Register>();
        public ObservableCollection<Register> listInput2 { get; set; } = new ObservableCollection<Register>();
        public ObservableCollection<Register> listRT { get; set; } = new ObservableCollection<Register>();
    }


    //-----------------------------------------------------------------------------------------
    // 
    //-----------------------------------------------------------------------------------------
    internal class DeviceKSSM : DeviceSlave
    {
        //object lockitems = new object();

        //BindingOperations.EnableCollectionSynchronization(listInput, lockitems);

        public RegisterRT RealTime;                     // Текущее системное время
        public RegisterInt KIP_count;                   // Установленное колоичество КИП
        public RegisterInt KIP_find;                    // Количество найденных КИП
        public RegisterInt SD_check;                    // Свободный объем SD-карты
        public RegisterInt ElectricMeter1;              // Показаания эл.счетчика 1
        public RegisterInt ElectricMeter2;              // Показаания эл.счетчика 2
        public RegisterInt isDoor;                      // концевик двери

        //public RegisterRT RealTimeW;                    // Текущее системное время
        //public RegisterInt KIP_countW;                  // Установленное колоичество КИП
        //public RegisterInt KIP_findW;                   // Количество найденных КИП
        //public RegisterInt SD_checkW;                   // Свободный объем SD-карты
        //public RegisterInt ElectricMeter1W;             // Показаания эл.счетчика 1
        //public RegisterInt ElectricMeter2W;             // Показаания эл.счетчика 2
        //public RegisterInt isDoorW;                     // концевик двери

        public List<Register> ListHolding { get; set; } = new List<Register>();
        //public List<Register> ListWriteHolding = new List<Register>();


        //private List<KIP_KSSM> _listKIP = new List<KIP_KSSM>();
        public ObservableCollection<KIP_KSSM> listKIP { get; set; } = new ObservableCollection<KIP_KSSM>();


        //-----------------------------------------------------------------------------------------
        // 
        //-----------------------------------------------------------------------------------------
        public DeviceKSSM(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {
            RealTime = new RegisterRT()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Текущее системное время",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 2,
                Description = "",
            };
            ListHolding.Add(RealTime);



            KIP_count = new RegisterInt()
            {
                Address = 0x03,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Установленное количество КИП",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
            };
            ListHolding.Add(KIP_count);

            KIP_find = new RegisterInt()
            {
                Address = 0x04,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Количество найденных КИП",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
            };
            ListHolding.Add(KIP_find);

            SD_check = new RegisterInt()
            {
                Address = 0x05,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Свободный объем SD-карты",
                NameRes = "",
                Measure = "Мбайт",
                MeasureRes = "",
                Size = 1,
                Description = "",
            };
            ListHolding.Add(SD_check);

            ElectricMeter1 = new RegisterInt()
            {
                Address = 0x06,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Показаания эл.счетчика 1",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
            };
            ListHolding.Add(ElectricMeter1);

            ElectricMeter2 = new RegisterInt()
            {
                Address = 0x07,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Показаания эл.счетчика 2",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
            };
            ListHolding.Add(ElectricMeter2);

            isDoor = new RegisterInt()
            {
                Address = 0x08,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Концевик двери",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Size = 1,
                Description = "",
            };
            ListHolding.Add(isDoor);

            //RealTimeW = new RegisterRT()
            //{
            //    Address = 0x401,
            //    CodeFunc = ModbusFunc.HoldingRegister,
            //    Name = "Текущее системное время",
            //    NameRes = "",
            //    Measure = "",
            //    MeasureRes = "",
            //    Size = 2,
            //    Description = "",
            //};
            //ListWriteHolding.Add(RealTimeW);

            //KIP_countW = new RegisterInt()
            //{
            //    Address = 0x403,
            //    CodeFunc = ModbusFunc.HoldingRegister,
            //    Name = "Установленное колоичество КИП",
            //    NameRes = "",
            //    Measure = "",
            //    MeasureRes = "",
            //    Size = 1,
            //    Description = "",
            //};
            //ListWriteHolding.Add(KIP_countW);

            //KIP_findW = new RegisterInt()
            //{
            //    Address = 0x404,
            //    CodeFunc = ModbusFunc.HoldingRegister,
            //    Name = "Скорость коррозии",
            //    NameRes = "",
            //    Measure = "мкм/год",
            //    MeasureRes = "",
            //    Size = 1,
            //    Description = "",
            //};
            //ListWriteHolding.Add(KIP_findW);

            //SD_checkW = new RegisterInt()
            //{
            //    Address = 0x405,
            //    CodeFunc = ModbusFunc.HoldingRegister,
            //    Name = "Свободный объем SD-карты",
            //    NameRes = "",
            //    Measure = "Мбайт",
            //    MeasureRes = "",
            //    Size = 1,
            //    Description = "",
            //};
            //ListWriteHolding.Add(SD_checkW);

            //ElectricMeter1W = new RegisterInt()
            //{
            //    Address = 0x406,
            //    CodeFunc = ModbusFunc.HoldingRegister,
            //    Name = "Показаания эл.счетчика 1",
            //    NameRes = "",
            //    Measure = "кВт/ч",
            //    MeasureRes = "",
            //    Size = 1,
            //    Description = "",
            //};
            //ListWriteHolding.Add(ElectricMeter1W);

            //ElectricMeter2W = new RegisterInt()
            //{
            //    Address = 0x407,
            //    CodeFunc = ModbusFunc.HoldingRegister,
            //    Name = "Показаания эл.счетчика 2",
            //    NameRes = "",
            //    Measure = "кВт/ч",
            //    MeasureRes = "",
            //    Size = 1,
            //    Description = "",
            //};
            //ListWriteHolding.Add(ElectricMeter2W);

            //isDoorW = new RegisterInt()
            //{
            //    Address = 0x408,
            //    CodeFunc = ModbusFunc.HoldingRegister,
            //    Name = "Концевик двери",
            //    NameRes = "",
            //    Measure = "",
            //    MeasureRes = "",
            //    Size = 1,
            //    Description = "",
            //};
            //ListWriteHolding.Add(isDoorW);


        }

        //-----------------------------------------------------------------------------------------
        // 
        //-----------------------------------------------------------------------------------------
        private void initKIP(int count)
        {
            //Dispatcher.CurrentDispatcher.InvokeAsync(() => listKIP.Clear());

            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                listKIP.Clear();
            }));

            ushort startAddress = 0x01;
            ushort currAddress;

            for (int num = 1; num <= count; ++num)
            {
                KIP_KSSM kip = new KIP_KSSM();
                currAddress = startAddress;

                kip.Number = num;

                kip.AddresBI = new RegisterInt()
                {
                    Address = currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Адрес БИ",
                    NameRes = "",
                    Measure = "",
                    MeasureRes = "",
                    Description = "",
                };
                kip.listInput.Add(kip.AddresBI);
                kip.listInput1.Add(kip.AddresBI);

                kip.StatusFlags = new RegisterInt()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Флаги состояния",
                    NameRes = "",
                    Measure = "",
                    MeasureRes = "",
                    Description = "",
                };
                kip.listInput.Add(kip.StatusFlags);
                kip.listInput1.Add(kip.StatusFlags);


                kip.TimeBI = new RegisterRT()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Время БИ",
                    Size = 2,
                    NameRes = "",
                    Measure = "",
                    MeasureRes = "",
                    Description = "",
                };
                kip.listInput.Add(kip.TimeBI);
                kip.listRT.Add(kip.TimeBI);

                ++currAddress;

                kip.PeriodRead = new RegisterInt()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Период опроса",
                    Size = 1,
                    NameRes = "",
                    Measure = "",
                    MeasureRes = "",
                    Description = "",
                };
                kip.listInput.Add(kip.PeriodRead);
                kip.listInput2.Add(kip.PeriodRead);


                kip.Shunt = new RegisterInt()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Номинал шунта в Амперах",
                    Size = 1,
                    NameRes = "",
                    Measure = "А",
                    MeasureRes = "",
                    Description = "",
                };
                kip.listInput.Add(kip.Shunt);
                kip.listInput2.Add(kip.Shunt);


                kip.TemperBoard = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Температура на плате",
                    Size = 1,
                    NameRes = "",
                    Measure = "",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.1f,
                    MinValue = -55,
                    MaxValue = 125,
                };
                kip.listInput.Add(kip.TemperBoard);
                kip.listInput2.Add(kip.TemperBoard);


                kip.CurrProtect = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Ток защиты",
                    Size = 1,
                    NameRes = "",
                    Measure = "А",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -320,
                    MaxValue = 320,
                };
                kip.listInput.Add(kip.CurrProtect);
                kip.listInput2.Add(kip.CurrProtect);


                kip.VoltOut = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Напряжение на выходе СКЗ",
                    Size = 1,
                    NameRes = "",
                    Measure = "В",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -100,
                    MaxValue = 100,
                };
                kip.listInput.Add(kip.VoltOut);
                kip.listInput2.Add(kip.VoltOut);


                kip.CurrPol1 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Ток поляризации 1",
                    Size = 1,
                    NameRes = "",
                    Measure = "мА",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -50,
                    MaxValue = 50,
                };
                kip.listInput.Add(kip.CurrPol1);
                kip.listInput2.Add(kip.CurrPol1);


                kip.PolPot1 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Поляризационный потенциал 1",
                    Size = 1,
                    NameRes = "",
                    Measure = "В",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -10,
                    MaxValue = 10,
                };
                kip.listInput.Add(kip.PolPot1);
                kip.listInput2.Add(kip.PolPot1);


                kip.SummPot1 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Суммарный потенциал 1",
                    Size = 1,
                    NameRes = "",
                    Measure = "В",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -10,
                    MaxValue = 10,
                };
                kip.listInput.Add(kip.SummPot1);
                kip.listInput2.Add(kip.SummPot1);


                kip.VoltNaveden1 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Наведенное напряжение 1",
                    Size = 1,
                    NameRes = "",
                    Measure = "В",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = 0,
                    MaxValue = 150,
                };
                kip.listInput.Add(kip.VoltNaveden1);
                kip.listInput2.Add(kip.VoltNaveden1);


                kip.FreqVoltNaveden1 = new RegisterInt()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Частота наведенного напряжения 1",
                    Size = 1,
                    NameRes = "",
                    Measure = "Гц",
                    MeasureRes = "",
                    Description = "",
                };
                kip.listInput.Add(kip.FreqVoltNaveden1);
                kip.listInput2.Add(kip.FreqVoltNaveden1);


                kip.CurrPol2 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Ток поляризации 2",
                    Size = 1,
                    NameRes = "",
                    Measure = "мА",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -50,
                    MaxValue = 50,
                };
                kip.listInput.Add(kip.CurrPol2);
                kip.listInput2.Add(kip.CurrPol2);


                kip.PolPot2 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Поляризационный потенциал 2",
                    Size = 1,
                    NameRes = "",
                    Measure = "В",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -10,
                    MaxValue = 10,
                };
                kip.listInput.Add(kip.PolPot2);
                kip.listInput2.Add(kip.PolPot2);


                kip.SummPot2 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Суммарный потенциал 2",
                    Size = 1,
                    NameRes = "",
                    Measure = "В",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -10,
                    MaxValue = 10,
                };
                kip.listInput.Add(kip.SummPot2);
                kip.listInput2.Add(kip.SummPot2);


                kip.VoltNaveden2 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Наведенное напряжение 2",
                    Size = 1,
                    NameRes = "",
                    Measure = "В",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = 0,
                    MaxValue = 150,
                };
                kip.listInput.Add(kip.VoltNaveden2);
                kip.listInput2.Add(kip.VoltNaveden2);


                kip.FreqVoltNaveden2 = new RegisterInt()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Частота наведенного напряжения 2",
                    Size = 1,
                    NameRes = "",
                    Measure = "Гц",
                    MeasureRes = "",
                    Description = "",
                };
                kip.listInput.Add(kip.FreqVoltNaveden2);
                kip.listInput2.Add(kip.FreqVoltNaveden2);


                kip.VoltOtnos1 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Напряжение на токовом относе 1",
                    Size = 1,
                    NameRes = "",
                    Measure = "мВ",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -100,
                    MaxValue = 100,
                };
                kip.listInput.Add(kip.VoltOtnos1);
                kip.listInput2.Add(kip.VoltOtnos1);


                kip.VoltOtnos2 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Напряжение на токовом относе 2",
                    Size = 1,
                    NameRes = "",
                    Measure = "мВ",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -100,
                    MaxValue = 100,
                };
                kip.listInput.Add(kip.VoltOtnos2);
                kip.listInput2.Add(kip.VoltOtnos2);


                kip.BIT1 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Данные с индикатора тока БИТ 1",
                    Size = 1,
                    NameRes = "",
                    Measure = "А",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -320,
                    MaxValue = 320,
                };
                kip.listInput.Add(kip.BIT1);
                kip.listInput2.Add(kip.BIT1);


                kip.BIT2 = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Данные с индикатора тока БИТ 2",
                    Size = 1,
                    NameRes = "",
                    Measure = "А",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.01f,
                    MinValue = -320,
                    MaxValue = 320,
                };
                kip.listInput.Add(kip.BIT2);
                kip.listInput2.Add(kip.BIT2);


                kip.VoltagePower = new RegisterFloat()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Напряжение питания",
                    Size = 1,
                    NameRes = "",
                    Measure = "В",
                    MeasureRes = "",
                    Description = "",
                    Scale = 0.001f,
                    MinValue = 0,
                    MaxValue = 65535,
                };
                kip.listInput.Add(kip.VoltagePower);
                kip.listInput2.Add(kip.VoltagePower);


                kip.SpeedKorr = new RegisterInt()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Напряжение питания",
                    Size = 1,
                    NameRes = "",
                    Measure = "мкм/год",
                    MeasureRes = "",
                    Description = "",
                };
                kip.listInput.Add(kip.SpeedKorr);
                kip.listInput2.Add(kip.SpeedKorr);


                kip.DeepKorr = new RegisterInt()
                {
                    Address = ++currAddress,
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = "Напряжение питания",
                    Size = 1,
                    NameRes = "",
                    Measure = "мкм",
                    MeasureRes = "",
                    Description = "",
                };
                kip.listInput.Add(kip.DeepKorr);
                kip.listInput2.Add(kip.DeepKorr);


                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    listKIP.Add(kip);
                }));
                //Dispatcher.CurrentDispatcher.InvokeAsync(() => listKIP.Add(kip));

                startAddress += 0x20;
            }

        }


        private void setFlags(KIP_KSSM kip)
        {
            kip.isOpen = (kip.StatusFlags.Value & 1) == 1;
            kip.isError = (kip.StatusFlags.Value & 16) == 16;
            kip.isDK1 = (kip.StatusFlags.Value & 2) == 2;
            kip.isDK2 = (kip.StatusFlags.Value & 4) == 4;
            kip.isDK3 = (kip.StatusFlags.Value & 8) == 8;
        }

        //-----------------------------------------------------------------------------------------
        // 
        //-----------------------------------------------------------------------------------------
        public override Task RequestValue()
        {
            ReadRegisters(ListHolding);
            //ReadRegisters(ListWriteHolding);

            if(KIP_find.Value != listKIP.Count)
                initKIP(KIP_find.Value ?? 0);

            foreach (var kip in listKIP)
            {
                ReadRegisters(kip.listInput);
                ReadRegisters(kip.listInput2);
                setFlags(kip);
            }
            //listKIP[2].isOpen = true;
            //listKIP[5].isError = true;
            //listKIP[17].isDK1 = true;
            //listKIP[22].isDK2 = true;
            //listKIP[29].isDK3 = true;

            return Task.CompletedTask;
        }


        //-----------------------------------------------------------------------------------------
        // 
        //-----------------------------------------------------------------------------------------
        public override Task StartRequestValue()
        {
            ReadRegisters(ListHolding);
            //ReadRegisters(ListWriteHolding);
            //KIP_find.Value = 32;

            initKIP(KIP_find.Value ?? 0);
            foreach (var kip in listKIP)
            {
                ReadRegisters(kip.listInput);
                ReadRegisters(kip.listInput2);
                setFlags(kip);
            }
            return Task.CompletedTask;
        }

        //-----------------------------------------------------------------------------------------
        // 
        //-----------------------------------------------------------------------------------------
        protected override void CheckListRegister()
        {
            CheckReg(ListHolding);
            //CheckReg(ListWriteHolding);
        }
    }
}
