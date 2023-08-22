//using ServiceControl.Infrastructure;
using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interop;

namespace ServiceControl.Modbus.Devices
{
    internal class Device356 : Device
    {
        //public const int CountDK = 10;
        public const int CountMS = 12;
        public const int CountBI = 10;
        public bool IsOldVersion = true;

        public RegisterFloat NaprSeti1;
        public RegisterFloat CountEE1;
        public RegisterFloat NaprSeti2;
        public RegisterFloat CountEE2;
        public RegisterFloat Temper;
        public RegisterInt TimeWork;
        public RegisterInt TimeProtect;
        public RegisterFloat CurrOutput;
        public RegisterFloat NaprOutput;
        public RegisterFloat ProtectPotenSumm;
        public RegisterFloat ProtectPotenPol;

        public RegisterMS[] MS;
        public RegisterFloat[] SpeedDK;
        public RegisterFloat[] DeepDK;
        public RegisterFloat[] BI_SummPot;
        public RegisterFloat[] BI_PolPot;
        public RegisterFloat[] BI_CurrPol;
        public RegisterFloat[] BI_OutVoltage;
        public RegisterFloat[] BI_OutCurrent;
        public RegisterFloat[] BI_IndVoltage;
        public RegisterInt[] BI_FreqVoltage;
        public RegisterFloat[] BI_Temper;


        public RegisterBool IllegalAccess;
        public RegisterBool DistanceMode { get; set; }
        public RegisterBool Fault;
        public RegisterBool BreakCirc;
        public RegisterBool OnMS;
        public RegisterBool SpeedCorr1;
        public RegisterBool SpeedCorr2;
        public RegisterBool SpeedCorr3;
        public RegisterBool OnOffMS { get; set; }
        public RegisterBool OnOffMSWrite { get; set; }

        public RegisterStab Stabil { get; set; }
        public RegisterFloat SetCurrOutput;
        public RegisterFloat SetSummPotOutput;
        public RegisterFloat SetPolPotOutput;
        public RegisterStab SetMode { get; set; }
        public RegisterFloat SetNaprOutput;

        public RegisterInfo InfoReg { get; set; }

        List<Register> ListInput;
        List<Register> ListInputBI;
        List<RegisterBool> ListStatus;
        List<Register> ListWriteControl;

#if !CLIENT
        public RegisterNapr4896 ModeNaprOutput { get; set; }
        public RegisterNapr4896 ModeNaprOutputWrite { get; set; }
        public RegisterRT RealTime { get; set; }
        public RegisterRT RealTimeWrite { get; set; }
        public RegisterInt TempCoolerOnWrite;
        public RegisterInt TempCoolerOffWrite;
        public RegisterInt ResistPlast1;
        public RegisterInt ResistPlast2;
        public RegisterInt ResistPlast3;
        public RegisterFloat CurrPolyar;
        public RegisterInt TimeWorkWrite;
        public RegisterInt TimeProtectWrite;
        public RegisterInt TempCoolerOn;
        public RegisterInt TempCoolerOff;

        List<Register> ListWriteControl2;
        List<Register> ListServices;
        List<Register> ListDop;
#endif


        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device356(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {
            InfoReg = new RegisterInfo() { Name = "Информация", NameRes = "" };
            ReadInfoRegister(InfoReg);

            // список входных регистров
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListInput = new List<Register>();

            NaprSeti1 = new RegisterFloat()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Напряжение сети 1",
                NameRes = "VoltCircuit1",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uc1",
                Scale = 0.1f,
                MinValue = 0,
                MaxValue = 300
            };
            ListInput.Add(NaprSeti1);

            CountEE1 = new RegisterFloat()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Счетчик э/э сети 1",
                NameRes = "ValueCounterEE1",
                Measure = "кВт*ч",
                MeasureRes = "KVT",
                Description = "Сч.ЭЭ.1",
                Scale = 0.1f,
                MinValue = 0,
                MaxValue = 999999.9f
            };
            ListInput.Add(CountEE1);

            NaprSeti2 = new RegisterFloat()
            {
                Address = 0x04,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Напряжение сети 2",
                NameRes = "VoltCircuit2",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uc2",
                Scale = 0.1f,
                MinValue = 0,
                MaxValue = 300
            };
            ListInput.Add(NaprSeti2);

            CountEE2 = new RegisterFloat()
            {
                Address = 0x05,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Счетчик э/э сети 2",
                NameRes = "ValueCounterEE2",
                Measure = "кВт*ч",
                MeasureRes = "KVT",
                Description = "Сч.ЭЭ.2",
                Scale = 0.1f,
                MinValue = 0,
                MaxValue = 999999.9f
            };
            ListInput.Add(CountEE2);

            Temper = new RegisterFloat()
            {
                Address = 0x07,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Температура в шкафу",
                Measure = "°С",
                NameRes = "InnnerTemper",
                Description = "°C",
                MinValue = -45,
                MaxValue = 100
            };
            ListInput.Add(Temper);

            TimeWork = new RegisterInt()
            {
                Address = 0x08,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Время наработки",
                NameRes = "TimeWork",
                Measure = "ч",
                MeasureRes = "Hour",
                Description = "СВН",
                MinValue = 0,
                MaxValue = 999999
            };
            ListInput.Add(TimeWork);

            TimeProtect = new RegisterInt()
            {
                Address = 0x0A,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Время защиты сооружения",
                NameRes = "TimeProtect",
                Measure = "ч",
                MeasureRes = "Hour",
                Description = "СВЗ",
                MinValue = 0,
                MaxValue = 999999
            }; 
            ListInput.Add(TimeProtect);

            CurrOutput = new RegisterFloat()
            {
                Address = 0x0C,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Выходной ток",
                NameRes = "OutCur",
                Measure = "A",
                Description = "Iвых",
                Scale = 0.01f,
                MinValue = 0,
                MaxValue = 150
            };
            ListInput.Add(CurrOutput);

            NaprOutput = new RegisterFloat()
            {
                Address = 0x0D,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Выходное напряжение",
                NameRes = "OutNapr",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uвых",
                Scale = 0.01f,
                MinValue = 0,
                MaxValue = 100
            };
            ListInput.Add(NaprOutput);

            ProtectPotenSumm = new RegisterFloat()
            {
                Address = 0x0E,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Защитный потенциал",
                NameRes = "SummPot",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uсп",
                Scale = 0.01f,
                MinValue = -5,
                MaxValue = 5
            };
            ListInput.Add(ProtectPotenSumm);

            ProtectPotenPol = new RegisterFloat()
            {
                Address = 0x0F,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 1,
                Name = "Поляризационный потенциал",
                NameRes = "PolPot",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uпп",
                Scale = 0.01f,
                MinValue = -5,
                MaxValue = 5
            };
            ListInput.Add(ProtectPotenPol);

            Stabil = new RegisterStab()
            {
                Address = 0x10,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Режим управления станцией",
                NameRes = "ModeControlStation",
                Description = "",
                MinValue = 0,
                MaxValue = 3
            };
            ListInput.Add(Stabil);

            MS = new RegisterMS[CountMS];
            for (int i = 0; i < CountMS; i++)
            {
                MS[i] = new RegisterMS()
                {
                    Number = i + 1,
                    Address = (ushort)(0x11 + i),
                    CodeFunc = ModbusFunc.InputRegister,
                    Name = $"Состояние модуля силового {i + 1}",
                    NameRes = "PowerModuleStatus",
                    Description = $"ССМ{i + 1}",
                    MinValue = 0,
                    MaxValue = 3
                };
                ListInput.Add(MS[i]);
            }

            int.TryParse(InfoReg.VersionPO, out int version);
            int.TryParse(InfoReg.Year,  out int year);

            if ((year >= 2020 && version <= 3) || (year >= 2023 && version >= 4))
            {
                IsOldVersion = false;
                SpeedDK = new RegisterFloat[CountBI];
                DeepDK = new RegisterFloat[CountBI];
                BI_SummPot = new RegisterFloat[CountBI];
                BI_PolPot = new RegisterFloat[CountBI];
                BI_CurrPol = new RegisterFloat[CountBI];
                BI_OutVoltage = new RegisterFloat[CountBI];
                BI_OutCurrent = new RegisterFloat[CountBI];
                BI_IndVoltage = new RegisterFloat[CountBI];
                BI_FreqVoltage = new RegisterInt[CountBI];
                BI_Temper = new RegisterFloat[CountBI];


                int adr = 0;
                for (int i = 0; i < CountBI; i++, adr += 2)
                {
                    SpeedDK[i] = new RegisterFloat()
                    {
                        Address = (ushort)(0x1D + adr),
                        CodeFunc = ModbusFunc.InputRegister,
                        Name = $"Скорость коррозии {i + 1}",
                        NameRes = "SpeedCorrDK",
                        Measure = "мм/год",
                        MeasureRes = "MMYEAR",
                        Description = $"СК_ИКП{i + 1}",
                        Scale = 0.001f,
                        MinValue = 0,
                        MaxValue = 65.535f,
                        Number = i + 1
                    };
                    ListInput.Add(SpeedDK[i]);

                    DeepDK[i] = new RegisterFloat()
                    {
                        Address = (ushort)(0x1E + adr),
                        CodeFunc = ModbusFunc.InputRegister,
                        Name = $"Глубина коррозии ИКП {i + 1}",
                        NameRes = "DeepCorrDK",
                        Measure = "мм",
                        MeasureRes = "MM",
                        Description = $"ГК_ИКП{i + 1}",
                        Scale = 0.001f,
                        MinValue = 0,
                        MaxValue = 65.535f,
                        Number = i + 1
                    };
                    ListInput.Add(DeepDK[i]);

                }

                ListInputBI = new List<Register>();
                adr = 0;
                for (int i = 0; i < CountBI; i++, adr += 8)
                {
                    BI_SummPot[i] = new RegisterFloat()
                    {
                        Address = (ushort)(0x51 + adr),
                        CodeFunc = ModbusFunc.InputRegister,
                        Name = $"Сумм. пот. БИ{i + 1}",
                        NameRes = "SummPot",
                        Measure = "В",
                        MeasureRes = "Volt",
                        Description = $"Uсп_БИ{i + 1}",
                        Scale = 0.01f,
                        MinValue = -5,
                        MaxValue = 5,
                        Number = i + 1
                    };
                    ListInputBI.Add(BI_SummPot[i]);

                    BI_PolPot[i] = new RegisterFloat()
                    {
                        Address = (ushort)(0x52 + adr),
                        CodeFunc = ModbusFunc.InputRegister,
                        Name = $"Пол. пот. БИ{i + 1}",
                        NameRes = "PolPot",
                        Measure = "В",
                        MeasureRes = "Volt",
                        Description = $"Uпп_БИ{i + 1}",
                        Scale = 0.01f,
                        MinValue = -5,
                        MaxValue = 5,
                        Number = i + 1
                    };
                    ListInputBI.Add(BI_PolPot[i]);

                    BI_CurrPol[i] = new RegisterFloat()
                    {
                        Address = (ushort)(0x53 + adr),
                        CodeFunc = ModbusFunc.InputRegister,
                        Name = $"Ток полр. БИ{i + 1}",
                        NameRes = "CurrPol",
                        Measure = "мА",
                        MeasureRes = "MAmp",
                        Description = $"Iпол_БИ{i + 1}",
                        Scale = 0.01f,
                        MinValue = -5,
                        MaxValue = 5,
                        Number = i + 1
                    };
                    ListInputBI.Add(BI_CurrPol[i]);

                    BI_OutVoltage[i] = new RegisterFloat()
                    {
                        Address = (ushort)(0x54 + adr),
                        CodeFunc = ModbusFunc.InputRegister,
                        Name = $"Вых напр. БИ{i + 1}",
                        NameRes = "OutNapr",
                        Measure = "В",
                        MeasureRes = "Volt",
                        Description = $"Uвых_БИ{i + 1}",
                        Scale = 0.01f,
                        MinValue = 0,
                        MaxValue = 100,
                        Number = i + 1
                    };
                    ListInputBI.Add(BI_OutVoltage[i]);

                    BI_OutCurrent[i] = new RegisterFloat()
                    {
                        Address = (ushort)(0x55 + adr),
                        CodeFunc = ModbusFunc.InputRegister,
                        Name = $"Вых напр. БИ{i + 1}",
                        NameRes = "OutCur",
                        Measure = "А",
                        MeasureRes = "Amp",
                        Description = $"Iвых_БИ{i + 1}",
                        Scale = 0.01f,
                        MinValue = 0,
                        MaxValue = 150,
                        Number = i + 1
                    };
                    ListInputBI.Add(BI_OutCurrent[i]);

                    BI_IndVoltage[i] = new RegisterFloat()
                    {
                        Address = (ushort)(0x56 + adr),
                        CodeFunc = ModbusFunc.InputRegister,
                        Name = $"Наведенное напр. БИ{i + 1}",
                        NameRes = "IndVoltage",
                        Measure = "В",
                        MeasureRes = "Volt",
                        Description = $"Uнав_БИ{i + 1}",
                        Scale = 0.01f,
                        MinValue = 0,
                        MaxValue = 100,
                        Number = i + 1
                    };
                    ListInputBI.Add(BI_IndVoltage[i]);

                    BI_FreqVoltage[i] = new RegisterInt()
                    {
                        Address = (ushort)(0x57 + adr),
                        CodeFunc = ModbusFunc.InputRegister,
                        Name = $"Частота нав. напр. БИ{i + 1}",
                        NameRes = "FreqIndVoltage",
                        Measure = "Гц",
                        MeasureRes = "Hertz",
                        Description = $"F_БИ{i + 1}",
                        MinValue = 0,
                        MaxValue = 100,
                        Number = i + 1
                    };
                    ListInputBI.Add(BI_FreqVoltage[i]);

                    BI_Temper[i] = new RegisterFloat()
                    {
                        Address = (ushort)(0x58 + adr),
                        CodeFunc = ModbusFunc.InputRegister,
                        Name = $"Температура БИ{i + 1}",
                        NameRes = "Temper",
                        Measure = "°C",
                        Scale = 0.01f,
                        Description = $"T_БИ{i + 1}",
                        MinValue = -45,
                        MaxValue = 100,
                        Number = i + 1
                    };
                    ListInputBI.Add(BI_Temper[i]);

                }
            }

            // список регистров статусов
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListStatus = new List<RegisterBool>();

            IllegalAccess = new RegisterBool()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.InputDiscrete,
                Size = 1,
                Name = "Дверь шкафа",
                NameRes = "Door",
                Description = "ТС1 (Дверь)",
                ResultText0 = "дверь закрыта",
                ResultText1 = "дверь открыта",
                ResultText0Res = "CloseDoor",
                ResultText1Res = "OpenDoor",
                IsCorrectValue = false
            };
            ListStatus.Add(IllegalAccess);
            DistanceMode = new RegisterBool()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.InputDiscrete,
                Size = 1,
                Name = "Режим упр. станцией",
                NameRes = "DistControl",
                Description = "ТС2 (ДУ)",
                ResultText0 = "местный",
                ResultText1 = "дистанционный",
                ResultText0Res = "Local",
                ResultText1Res = "Remote",
                IsCorrectValue = true
            };
            ListStatus.Add(DistanceMode);
            Fault = new RegisterBool()
            {
                Address = 0x03,
                CodeFunc = ModbusFunc.InputDiscrete,
                Size = 1,
                Name = "Неисправность станции",
                NameRes = "FailStation",
                Description = "ТС3 (Неисправность СКЗ)",
                ResultText0 = "норма",
                ResultText1 = "авария",
                ResultText0Res = "Norm",
                ResultText1Res = "Avar",
                IsCorrectValue = false
            };
            ListStatus.Add(Fault);
            BreakCirc = new RegisterBool()
            {
                Address = 0x04,
                CodeFunc = ModbusFunc.InputDiscrete,
                Size = 1,
                Name = "Обрыв изм. цепей",
                NameRes = "BreakCirc",
                Description = "ТС4 (Обрыв ЭС/Т)",
                ResultText0 = "норма",
                ResultText1 = "авария",
                ResultText0Res = "Norm",
                ResultText1Res = "Avar",
                IsCorrectValue = false
            };
            ListStatus.Add(BreakCirc);
            OnMS = new RegisterBool()
            {
                Address = 0x05,
                CodeFunc = ModbusFunc.InputDiscrete,
                Size = 1,
                Name = "Силовые модули",
                NameRes = "PowerModul",
                Description = "ТС5 (основные-резервные)",
                ResultText0 = "основные",
                ResultText1 = "резервные",
                ResultText0Res = "Main",
                ResultText1Res = "Second",
                IsCorrectValue = false
            };
            ListStatus.Add(OnMS);

            SpeedCorr1 = new RegisterBool()
            {
                Address = 0x06,
                CodeFunc = ModbusFunc.InputDiscrete,
                Size = 1,
                Name = "Индикатор скорости корр. 1",
                NameRes = "SensorpeedCorr",
                Description = "ТС6-1 (ДСК1)",
                ResultText0 = "разрыв",
                ResultText1 = "замкнут",
                ResultText0Res = "Break",
                ResultText1Res = "Contact",
                IsCorrectValue = true
            };
            ListStatus.Add(SpeedCorr1);
            SpeedCorr2 = new RegisterBool()
            {
                Address = 0x07,
                CodeFunc = ModbusFunc.InputDiscrete,
                Size = 1,
                Name = "Индикатор скорости корр. 2",
                NameRes = "SensorpeedCorr",
                Description = "ТС6-2 (ДСК2)",
                ResultText0 = "разрыв",
                ResultText1 = "замкнут",
                ResultText0Res = "Break",
                ResultText1Res = "Contact",
                IsCorrectValue = true
            };
            ListStatus.Add(SpeedCorr2);
            SpeedCorr3 = new RegisterBool()
            {
                Address = 0x08,
                CodeFunc = ModbusFunc.InputDiscrete,
                Size = 1,
                Name = "Индикатор скорости корр. 3",
                NameRes = "SensorpeedCorr",
                Description = "ТС6-3 (ДСК3)",
                ResultText0 = "разрыв",
                ResultText1 = "замкнут",
                ResultText0Res = "Break",
                ResultText1Res = "Contact",
                IsCorrectValue = true
            };
            ListStatus.Add(SpeedCorr3);


            // список управляющих регистров
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListWriteControl = new List<Register>();

            SetCurrOutput = new RegisterFloat()
            {
                Address = 0x81,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Задание выходного тока",
                NameRes = "SetCurrent",
                Measure = "A",
                Description = "Iуст",
                Scale = 0.01f,
                MinValue = 0,
                MaxValue = 150
            };
            ListWriteControl.Add(SetCurrOutput);

            SetSummPotOutput = new RegisterFloat()
            {
                Address = 0x82,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Задание суммарного потенциала",
                NameRes = "SetSummPot",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uпотс",
                Scale = 0.01f,
                MinValue = -5,
                MaxValue = 0
            };
            ListWriteControl.Add(SetSummPotOutput);

            SetPolPotOutput = new RegisterFloat()
            {
                Address = 0x83,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Задание поляризационного потенциала",
                NameRes = "",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uпотп",
                Scale = 0.01f,
                MinValue = -5,
                MaxValue = 0
            };
            ListWriteControl.Add(SetPolPotOutput);

            SetMode = new RegisterStab()
            {
                Address = 0x84,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Управление режимами стабилизации станции",
                NameRes = "ControlModeStab",
                Description = "Упр.",
                MinValue = 0,
                MaxValue = 3
            };
            ListWriteControl.Add(SetMode);

            SetNaprOutput = new RegisterFloat()
            {
                Address = 0x85,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Задание выходного напряжения",
                NameRes = "SetVoltage",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uуст",
                Scale = 0.01f,
                MinValue = 0,
                MaxValue = 100
            };
            ListWriteControl.Add(SetNaprOutput);

#if !CLIENT
            ListWriteControl2 = new List<Register>();
            // Отдельные регистры для записи
            RealTimeWrite = new RegisterRT()
            {
                Address = 0xC1,
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
            ListWriteControl2.Add(RealTimeWrite);


            TempCoolerOnWrite = new RegisterInt()
            {
                Address = 0xC5,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Температура включения вентилятора",
                NameRes = "TempOnCooler",
                Measure = "°С",
                Description = "Твкл.вент.",
                MinValue = -32769,
                MaxValue = 32678
            };
            ListWriteControl2.Add(TempCoolerOnWrite);

            TempCoolerOffWrite = new RegisterInt()
            {
                Address = 0xC6,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Температура выключения вентилятора",
                NameRes = "TempOffCooler",
                Measure = "°С",
                Description = "Твыкл.вент.",
                MinValue = -32769,
                MaxValue = 32768
            };
            ListWriteControl2.Add(TempCoolerOffWrite);

            TimeWorkWrite = new RegisterInt()
            {
                Address = 0xC7,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Время наработки",
                NameRes = "TimeWork",
                Measure = "ч",
                MeasureRes = "Hour",
                Description = "СВН",
                MinValue = 0,
                MaxValue = 999999
            };


            TimeProtectWrite = new RegisterInt()
            {
                Address = 0xC9,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Время защиты сооружения",
                NameRes = "TimeProtect",
                Measure = "ч",
                MeasureRes = "Hour",
                Description = "СВЗ",
                MinValue = 0,
                MaxValue = 999999
            };


            ModeNaprOutputWrite = new RegisterNapr4896()
            {
                Address = 0xCD,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Режим работы СКЗ (48/96)",
                NameRes = "ModeVoltage",
                Description = "Uрежим",
                Measure = "В",
                MeasureRes = "Volt",
                MinValue = 0,
                MaxValue = 1
            };

            // список сервисных регистров
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListServices = new List<Register>();

            RealTime = new RegisterRT()
            {
                Address = 0xC1,
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
            ListServices.Add(RealTime);

            TempCoolerOn = new RegisterInt()
            {
                Address = 0xC5,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Температура включения вентилятора",
                NameRes = "TempOnCooler",
                Measure = "°C",
                Description = "Твкл.вент.",
                MinValue = -32769,
                MaxValue = 32678
            };
            ListServices.Add(TempCoolerOn);

            TempCoolerOff = new RegisterInt()
            {
                Address = 0xC6,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Температура выключения вентилятора",
                NameRes = "TempOffCooler",
                Measure = "°C",
                Description = "Твыкл.вент.",
                MinValue = -32769,
                MaxValue = 32768
            };
            ListServices.Add(TempCoolerOff);

            ModeNaprOutput = new RegisterNapr4896()
            {
                Address = 0xCD,
                CodeFunc = ModbusFunc.HoldingRegister,
                Name = "Режим работы СКЗ (48/96)",
                NameRes = "ModeVoltage",
                Measure = "В",
                MeasureRes = "Volt",
                Description = "Uрежим",
                MinValue = 0,
                MaxValue = 1
            };


            ListDop = new List<Register>();

            ResistPlast1 = new RegisterInt()
            {
                Address = 0x45,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Сопротивление пластины 1",
                NameRes = "ResistPast1",
                Measure = "Ом",
                MeasureRes = "Ohm",
                Description = "Rn1",
                MinValue = 0,
                MaxValue = 1404
            };
            ListDop.Add(ResistPlast1);
            ResistPlast2 = new RegisterInt()
            {
                Address = 0x46,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Сопротивление пластины 2",
                NameRes = "ResistPast2",
                Measure = "Ом",
                MeasureRes = "Ohm",
                Description = "Rn2",
                MinValue = 0,
                MaxValue = 1404
            };
            ListDop.Add(ResistPlast2);
            ResistPlast3 = new RegisterInt()
            {
                Address = 0x47,
                CodeFunc = ModbusFunc.InputRegister,
                Name = "Сопротивление пластины 3",
                NameRes = "ResistPast3",
                Measure = "Ом",
                MeasureRes = "Ohm",
                Description = "Rn3",
                MinValue = 0,
                MaxValue = 1404
            };
            ListDop.Add(ResistPlast3);
            CurrPolyar = new RegisterFloat()
            {
                Address = 0x48,
                CodeFunc = ModbusFunc.InputRegister,
                Scale = 0.1f,
                Name = "Ток поляризации",
                NameRes = "CurrentPolyar",
                Measure = "мА",
                MeasureRes = "MAmp",
                Description = "Iпол",
                MinValue = -10,
                MaxValue = 10
            };
            ListDop.Add(CurrPolyar);


#endif

            // список управляющих статусов
            //--------------------------------------------------------------------------------------------------------------------------------------
            //ListCoil = new List<RegisterBool>();
            OnOffMS = new RegisterBool()
            {
                Address = 0x81,
                CodeFunc = ModbusFunc.Coil,
                Size = 1,
                Name = "Дистанц.откл.вкл.модулей силовых",
                NameRes = "RemoteOnOffPower",
                Description = "ТУ1 (ДО СМ)",
                ResultText0 = "выключен",
                ResultText1 = "включен",
                ResultText0Res = "Off",
                ResultText1Res = "On",
            };

            OnOffMSWrite = new RegisterBool()
            {
                Address = 0x81,
                CodeFunc = ModbusFunc.Coil,
                Size = 1,
                Name = "Дистанц.откл.вкл.модулей силовых",
                NameRes = "RemoteOnOffPower",
                Description = "ТУ1 (ДО СМ)",
                ResultText0 = "выключен",
                ResultText1 = "включен",
                ResultText0Res = "Off",
                ResultText1Res = "On",
            };


        }

        //-------------------------------------------------------------------------------------------
        // 
        //-------------------------------------------------------------------------------------------
        public override Task StartRequestValue()
        {
            ReadInfoRegister(InfoReg);
            ReadRegisters(ListWriteControl);
#if !CLIENT
            ReadRegisters(ListWriteControl2);
            ReadRegister(ModeNaprOutputWrite);
#endif
            ReadRegister(OnOffMS);
            ReadRegister(OnOffMSWrite);
            ReadRegister(SetMode);

            ReadRegisters(ListInput);
            ReadRegisters(ListInputBI);
            ReadRegisters(ListStatus);
#if !CLIENT
            ReadRegisters(ListServices);
            ReadRegister(ModeNaprOutput);
            ReadRegisters(ListDop);
#endif
            return Task.CompletedTask;

        }


        //-------------------------------------------------------------------------------------------
        // 
        //-------------------------------------------------------------------------------------------
        public override Task RequestValue()
        {
            ReadRegisters(ListInputBI);
            ReadRegisters(ListStatus);
            ReadRegister(OnOffMS);
#if !CLIENT
            ReadRegisters(ListServices);
            ReadRegisters(ListDop);
            ReadRegister(ModeNaprOutput);
#endif
            ReadRegisters(ListInput);
            return Task.CompletedTask;
        }

        //-------------------------------------------------------------------------------------------
        // проверка последовательноти регистров в списках
        //-------------------------------------------------------------------------------------------
        protected override void CheckListRegister()
        {
            CheckReg(ListInput);
            CheckReg(ListInputBI);
            CheckReg(ListStatus);
            CheckReg(ListWriteControl);
#if !CLIENT
            CheckReg(ListWriteControl2);
            CheckReg(ListServices);
#endif
        }


        //-------------------------------------------------------------------------------------------
        // Изменине языка для регистров
        //-------------------------------------------------------------------------------------------
        public override void ChangeLangRegister()
        {
            ListInput.ForEach(n => n.SetLanguage());
            ListInputBI?.ForEach(n => n.SetLanguage());
            ListStatus.ForEach(n => n.SetLanguage());
            ListWriteControl.ForEach(n => n.SetLanguage());
#if !CLIENT
            ListServices.ForEach(n => n.SetLanguage());
            ListDop.ForEach(n => n.SetLanguage());
            ListWriteControl2.ForEach(n => n.SetLanguage());
            ModeNaprOutput.SetLanguage();
            ModeNaprOutputWrite.SetLanguage();
#endif
            InfoReg.SetLanguage();
            OnOffMS.SetLanguage();
            OnOffMSWrite.SetLanguage();
            SetMode.SetLanguage();

        }

    }
}
