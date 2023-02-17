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
        public const int CountDK = 10;
        public const int CountMS = 12;
        public const int CountBI = 10;

        public RegisterFloat NaprSeti1;
        public RegisterFloat CountEE1;
        public RegisterFloat NaprSeti2;
        public RegisterFloat CountEE2;
        public RegisterFloat Temper;
        public RegisterFloat TimeWork;
        public RegisterFloat TimeProtect;
        public RegisterFloat CurrOutput;
        public RegisterFloat NaprOutput;
        public RegisterFloat ProtectPotenSumm;
        public RegisterFloat ProtectPotenPol;

        public RegisterMS[] MS;

        public RegisterFloat[] SpeedDK;
        public RegisterFloat[] DeepDK;
        public RegisterFloat[] SummPotBI;
        public RegisterFloat[] PolPotBI;
        public RegisterFloat[] CurrPolBI;
        public RegisterFloat[] OutNaprBI;
        public RegisterFloat[] OutCurrBI;
        public RegisterFloat[] NavNaprBI;
        public RegisterFloat[] FreqBI;
        public RegisterFloat[] TempBI;

        public RegisterBool IllegalAccess;
        public RegisterBool ControlMode;
        public RegisterBool Fault;
        public RegisterBool BreakCirc;
        public RegisterBool OnMS;
        public RegisterBool SpeedCorr1;
        public RegisterBool SpeedCorr2;
        public RegisterBool SpeedCorr3;
        public RegisterBool OnOffMS;
        public RegisterInt TempCoolerOn;
        public RegisterInt TempCoolerOff;
        public RegisterInt Year;
        public RegisterInt Number;
        public RegisterNapr4896 ModeNaprOutput;

        public RegisterRT RealTime { get; set; }
        //public RegisterInt WorkedTime;
        public RegisterInt ProtectTime;
        public RegisterInt ResistPlast1;
        public RegisterInt ResistPlast2;
        public RegisterInt ResistPlast3;
        RegisterFloat CurrPolyar;
        public RegisterInfo InfoReg { get; set; }


        public RegisterStab Stabil { get; set; }
        public RegisterFloat SetCurrOutput;
        public RegisterFloat SetSummPotOutput;
        public RegisterFloat SetPolPotOutput;
        public RegisterStab SetMode { get; set; }
        public RegisterFloat SetNaprOutput;

       List<Register> ListServices;
       List<Register> ListInput;
       List<RegisterBool> ListStatus;
       List<Register> ListWriteControl;
       List<RegisterBool> ListCoil;
       List<Register> ListBI; 
       List<Register> ListDop;

        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device356(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {

            // список входных регистров
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListInput = new List<Register>();

            NaprSeti1 = new RegisterFloat() { Address = 0x01, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Напряжение питающей сети 1 (основное)", Measure = "В", Description = "Uc1", Scale = 0.1f, MinValue = 0, MaxValue = 3000 };
            ListInput.Add(NaprSeti1);
            
            CountEE1 = new RegisterFloat() { Address = 0x02, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "Значение счетчика эл.энергии сети 1 (осн.)", Measure = "кВт*ч", Description = "Сч.ЭЭ.1", Scale = 0.1f, MinValue = 0, MaxValue = 9999999 };
            ListInput.Add(CountEE1);

            NaprSeti2 = new RegisterFloat() { Address = 0x04, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Напряжение питающей сети 2 (резервное)", Measure = "В", Description = "Uc2", Scale = 0.1f, MinValue = 0, MaxValue = 3000 };
            ListInput.Add(NaprSeti2);

            CountEE2 = new RegisterFloat() { Address = 0x05, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "Значение счетчика эл.энергии сети 1 (рез.)", Measure = "кВт*ч", Description = "Сч.ЭЭ.2", Scale = 0.1f, MinValue = 0, MaxValue = 3000 };
            ListInput.Add(CountEE2);

            Temper = new RegisterFloat() { Address = 0x07, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Температура в шкафу", Measure = "°С", Description = "Т°", Scale = 1, MinValue = 0, MaxValue = 3000 };
            ListInput.Add(Temper);

            TimeWork = new RegisterFloat() { Address = 0x08, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "Время наработки", Measure = "ч", Description = "СВН", Scale = 1, MinValue = 0, MaxValue = 3000 };
            ListInput.Add(TimeWork);

            TimeProtect = new RegisterFloat() { Address = 0x0A, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "Время защиты сооружения", Measure = "ч", Description = "СВЗ", Scale = 1, MinValue = 0, MaxValue = 3000 };
            ListInput.Add(TimeProtect);

            CurrOutput = new RegisterFloat() { Address = 0x0C, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Выходной ток", Measure = "А", Description = "Iвых", Scale = 0.01f, MinValue = 0, MaxValue = 3000 };
            ListInput.Add(CurrOutput);

            NaprOutput = new RegisterFloat() { Address = 0x0D, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Выходное напряжение", Measure = "В", Description = "Uвых", Scale = 0.01f, MinValue = 0, MaxValue = 3000 };
            ListInput.Add(NaprOutput);

            ProtectPotenSumm = new RegisterFloat() { Address = 0x0E, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Защитный потенциал суммарный", Measure = "В", Description = "Uсп", Scale = 0.01f, MinValue = 0, MaxValue = 3000 };
            ListInput.Add(ProtectPotenSumm);

            ProtectPotenPol = new RegisterFloat() { Address = 0x0F, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Защитный потенциал поляризационный", Measure = "В", Description = "Uпп", Scale = 0.01f, MinValue = 0, MaxValue = 3000 };
            ListInput.Add(ProtectPotenPol);

            Stabil = new RegisterStab() { Address = 0x10, CodeFunc = ModbusFunc.InputReg, Name = "Режим управления станцией", Description = "", MinValue = 0, MaxValue = 3 };
            ListInput.Add(Stabil);

            MS = new RegisterMS[CountMS];
            for (int i = 0; i < CountMS; i++)
            {
                MS[i] = new RegisterMS() { Address = (ushort)(0x11 + i), CodeFunc = ModbusFunc.InputReg, Name = $"Состояние модуля силового {i+1}", Description = $"ССМ{i+1}", MinValue = 0, MaxValue = 3 };
                ListInput.Add(MS[i]);
            }

            SpeedDK = new RegisterFloat[CountDK];
            DeepDK = new RegisterFloat[CountDK];
            for (int i = 0; i < CountDK; i++)
            {
                SpeedDK[i] = new RegisterFloat() { Address = (ushort)(0x1D + i * 2), CodeFunc = ModbusFunc.InputReg, Name = $"Скорость коррозии ИСК{i+1}", Measure = "мм в год", Description = $"СК_ИКП{i+1}", Scale = 0.001f, MinValue = 0, MaxValue = 65535 };
                ListInput.Add(SpeedDK[i]);
                DeepDK[i] = new RegisterFloat() { Address = (ushort)(0x1E + i * 2), CodeFunc = ModbusFunc.InputReg, Name = $"Глубина коррозии ИСК{i+1}", Measure = "мм", Description = $"ГК_ИКП{i+1}", Scale = 0.001f, MinValue = 0, MaxValue = 65535 };
                ListInput.Add(DeepDK[i]);

            }

            ListBI = new List<Register>();
            SummPotBI = new RegisterFloat[CountBI];
            PolPotBI = new RegisterFloat[CountBI];
            CurrPolBI = new RegisterFloat[CountBI];
            OutNaprBI = new RegisterFloat[CountBI];
            OutCurrBI = new RegisterFloat[CountBI];
            NavNaprBI = new RegisterFloat[CountBI];
            FreqBI = new RegisterFloat[CountBI];
            TempBI = new RegisterFloat[CountBI];

            for(int i = 0; i < CountBI; i++)
            {
                SummPotBI[i] = new RegisterFloat() { Address = (ushort)(0x51 + i * 8), CodeFunc = ModbusFunc.InputReg, Name = $"Суммарный потенциал БИ{i+1}", Measure = "В", Description = $"Uсп_БИ{i + 1}", Scale = 0.01f, MinValue = -500, MaxValue = 500 };
                ListBI.Add(SummPotBI[i]);
                PolPotBI[i] = new RegisterFloat() { Address = (ushort)(0x52 + i * 8), CodeFunc = ModbusFunc.InputReg, Name = $"Поляризац. потенциал БИ{i + 1}", Measure = "В", Description = $"Uпп_БИ{i + 1}", Scale = 0.01f, MinValue = -500, MaxValue = 500 };
                ListBI.Add(PolPotBI[i]);
                CurrPolBI[i] = new RegisterFloat() { Address = (ushort)(0x53 + i * 8), CodeFunc = ModbusFunc.InputReg, Name = $"Ток поляризации БИ{i + 1}", Measure = "мА", Description = $"Iпол_БИ{i + 1}", Scale = 0.01f, MinValue = -5000, MaxValue = 5000 };
                ListBI.Add(CurrPolBI[i]);
                OutNaprBI[i] = new RegisterFloat() { Address = (ushort)(0x54 + i * 8), CodeFunc = ModbusFunc.InputReg, Name = $"Выходное напряжение БИ{i + 1}", Measure = "В", Description = $"Uвых_БИ{i + 1}", Scale = 0.01f, MinValue = 0, MaxValue = 10000 };
                ListBI.Add(OutNaprBI[i]);
                OutCurrBI[i] = new RegisterFloat() { Address = (ushort)(0x55 + i * 8), CodeFunc = ModbusFunc.InputReg, Name = $"Выходное ток БИ{i + 1}", Measure = "А", Description = $"Iвых_БИ{i + 1}", Scale = 0.01f, MinValue = 0, MaxValue = 15000 };
                ListBI.Add(OutCurrBI[i]);
                NavNaprBI[i] = new RegisterFloat() { Address = (ushort)(0x56 + i * 8), CodeFunc = ModbusFunc.InputReg, Name = $"Наведенное напряжение БИ{i + 1}", Measure = "В", Description = $"Uнав_БИ{i + 1}", Scale = 0.01f, MinValue = 0, MaxValue = 10000 };
                ListBI.Add(NavNaprBI[i]);
                FreqBI[i] = new RegisterFloat() { Address = (ushort)(0x57 + i * 8), CodeFunc = ModbusFunc.InputReg, Name = $"Частота наведенного напряжения БИ{i + 1}", Measure = "Гц", Description = $"F_БИ{i + 1}", Scale = 1, MinValue = 0, MaxValue = 100 };
                ListBI.Add(FreqBI[i]);
                TempBI[i] = new RegisterFloat() { Address = (ushort)(0x58 + i * 8), CodeFunc = ModbusFunc.InputReg, Name = $"Температура БИ{i + 1}", Measure = "°С", Description = $"T_БИ{i + 1}", Scale = 1, MinValue = -45, MaxValue = 100 };
                ListBI.Add(TempBI[i]);
            }


            // список регистров статусов
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListStatus = new List<RegisterBool>();

            IllegalAccess = new RegisterBool() { Address = 0x01, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Несанкционированный доступ в шкаф", Description = "ТС1 (Дверь)", ResultText0 = "дверь закрыта", ResultText1 = "дверь открыта" };
            ListStatus.Add(IllegalAccess);
            ControlMode = new RegisterBool() { Address = 0x02, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Режим упр. станцией", Description = "ТС2 (ДУ)", ResultText0 = "местный", ResultText1 = "дистанционный" };
            ListStatus.Add(ControlMode);
            Fault = new RegisterBool() { Address = 0x03, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Неисправность станции", Description = "ТС3 (Неисправность СКЗ)", ResultText0 = "исправна (работа)", ResultText1 = "неисправна (авария)" };
            ListStatus.Add(Fault);
            BreakCirc = new RegisterBool() { Address = 0x04, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Обрыв изм. цепей", Description = "ТС4 (Обрыв ЭС/Т)", ResultText0 = "норма (нет обрыва)", ResultText1 = "неисправна (авария)" };
            ListStatus.Add(BreakCirc);
            OnMS = new RegisterBool() { Address = 0x05, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Включение группы осн. или рез. МС (СКЗ)", Description = "ТС5 (основные-резервные)", ResultText0 = "основные", ResultText1 = "резервные" };
            ListStatus.Add(OnMS);
            SpeedCorr1 = new RegisterBool() { Address = 0x06, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Индикатор скорости корр. 1", Description = "ТС6-1 (ДСК1)", ResultText0 = "разрыв", ResultText1 = "замкнут" };
            ListStatus.Add(SpeedCorr1);
            SpeedCorr2 = new RegisterBool() { Address = 0x07, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Индикатор скорости корр. 2", Description = "ТС6-2 (ДСК2)", ResultText0 = "разрыв", ResultText1 = "замкнут" };
            ListStatus.Add(SpeedCorr2);
            SpeedCorr3 = new RegisterBool() { Address = 0x08, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Индикатор скорости корр. 3", Description = "ТС6-3 (ДСК3)", ResultText0 = "разрыв", ResultText1 = "замкнут" };
            ListStatus.Add(SpeedCorr3);


            // список управляющих регистров
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListWriteControl = new List<Register>();

            SetCurrOutput = new RegisterFloat() { Address = 0x81, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "Задание выходного тока", Measure = "А", Description = "Iуст", Scale = 0.01f, MinValue = 0, MaxValue = 15000 };
            ListWriteControl.Add(SetCurrOutput);
            SetSummPotOutput = new RegisterFloat() { Address = 0x82, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "Задание суммарного потенциала", Measure = "В", Description = "Uпотс", Scale = 0.01f, MinValue = 0, MaxValue = 3000 };
            ListWriteControl.Add(SetSummPotOutput);
            SetPolPotOutput = new RegisterFloat() { Address = 0x83, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "Задание поляризационного потенциала", Measure = "В", Description = "Uпотп", Scale = 0.01f, MinValue = -500, MaxValue = 0 };
            ListWriteControl.Add(SetPolPotOutput);
            SetMode = new RegisterStab() { Address = 0x84, CodeFunc = ModbusFunc.Holding, Name = "Управление режимами стабилизации станции", Description = "Упр.", MinValue = 0, MaxValue = 3 };
            ListWriteControl.Add(SetMode);
            SetNaprOutput = new RegisterFloat() { Address = 0x85, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "Задание выходного напряжения", Measure = "В", Description = "Uуст", Scale = 0.01f, MinValue = 0, MaxValue = 10000 };
            ListWriteControl.Add(SetNaprOutput);

            //List<Register> ListWriteControl2 = new List<Register>();

            //RealTime = new RegisterRT() { Address = 0xC1, CodeFunc = ModbusFunc.Holding, Name = "Реальное время", Measure = "сек", Size = 4, Description = "РВ", MinValue = 0, MaxValue = 65535 };
            //ListWriteControl2.Add(RealTime);

            //TempCoolerOn = new RegisterInt() { Address = 0xC5, CodeFunc = ModbusFunc.Holding, Name = "Температура включения вентилятора", Measure = "°С", Description = "Твкл.вент.", MinValue = 0, MaxValue = 65535 };
            //ListWriteControl2.Add(TempCoolerOn);

            //TempCoolerOff = new RegisterInt() { Address = 0xC6, CodeFunc = ModbusFunc.Holding, Name = "Температура выключения вентилятора", Measure = "°С", Description = "Твыкл.вент.", MinValue = 0, MaxValue = 65535 };
            //ListWriteControl2.Add(TempCoolerOff);

            //WorkedTime = new RegisterInt() { Address = 0xC7, CodeFunc = ModbusFunc.Holding, Name = "Время наработки", Measure = "ч", Size = 2, Description = "СВН", MinValue = -596522, MaxValue = 596522 };
            //ListWriteControl2.Add(WorkedTime);

            //ProtectTime = new RegisterInt() { Address = 0xC9, CodeFunc = ModbusFunc.Holding, Name = "Время защиты сооружения", Measure = "ч", Size = 2, Description = "СВЗ", MinValue = -596522, MaxValue = 596522 };
            //ListWriteControl2.Add(ProtectTime);


            // список управляющих статусов
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListCoil = new List<RegisterBool>();
            OnOffMS = new RegisterBool() { Address = 0x81, CodeFunc = ModbusFunc.CoilRead, Size = 1, Name = "Дистанц.откл.вкл.модулей силовых", Description = "ТУ1 (ДО СМ)", ResultText0 = "выключить", ResultText1 = "включить" };
            ListCoil.Add(OnMS);


            // список сервисных регистров
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListServices = new List<Register>();
            RealTime = new RegisterRT() { Address = 0xC1, CodeFunc = ModbusFunc.Holding, Name = "Реальное время", Measure = "сек", Size = 4, Description = "РВ", MinValue = 0, MaxValue = 65535 };
            ListServices.Add(RealTime);
           
            TempCoolerOn = new RegisterInt() { Address = 0xC5, CodeFunc = ModbusFunc.Holding, Name = "Температура включения вентилятора", Measure = "°С", Description = "Твкл.вент.", MinValue = 0, MaxValue = 65535 };
            ListServices.Add(TempCoolerOn);
            
            TempCoolerOff = new RegisterInt() { Address = 0xC6, CodeFunc = ModbusFunc.Holding, Name = "Температура выключения вентилятора", Measure = "°С", Description = "Твыкл.вент.", MinValue = 0, MaxValue = 65535 };
            ListServices.Add(TempCoolerOff);

            var stab = new RegisterInt() { Address = 0xC7, CodeFunc = ModbusFunc.Holding, Name = "Заглушка", Size = 2, Description = "", MinValue = 0, MaxValue = 65535 };
            ListServices.Add(stab);

            ProtectTime = new RegisterInt() { Address = 0xC9, CodeFunc = ModbusFunc.Holding, Name = "Время защиты сооружения", Measure = "ч", Size = 2, Description = "СВЗ", MinValue = -596522, MaxValue = 596522 };
            ListServices.Add(ProtectTime);

            Year = new RegisterInt() { Address = 0xCB, CodeFunc = ModbusFunc.Holding, Name = "Год выпуска устройства", Description = "Год", MinValue = 0, MaxValue = 65535 };
            ListServices.Add(Year);
            
            Number = new RegisterInt() { Address = 0xCC, CodeFunc = ModbusFunc.Holding, Name = "Порядковый номер устройства", Description = "Номер", MinValue = 0, MaxValue = 65535 };
            ListServices.Add(Number);
            
            ModeNaprOutput = new RegisterNapr4896() { Address = 0xCD, CodeFunc = ModbusFunc.Holding, Name = "Режим выходного напряжения", Description = "Uрежим", MinValue = 0, MaxValue = 1 };
            ListServices.Add(ModeNaprOutput);

            ListDop = new List<Register>();
            ResistPlast1 = new RegisterInt() { Address = 0x45, CodeFunc = ModbusFunc.InputReg, Name = "Сопротивление пластины 1", Measure = "Ом", Description = "Rn1", MinValue = 0, MaxValue = 404 };
            ListDop.Add(ResistPlast1);
            ResistPlast2 = new RegisterInt() { Address = 0x46, CodeFunc = ModbusFunc.InputReg, Name = "Сопротивление пластины 2", Measure = "Ом", Description = "Rn2", MinValue = 0, MaxValue = 404 };
            ListDop.Add(ResistPlast2);
            ResistPlast3 = new RegisterInt() { Address = 0x47, CodeFunc = ModbusFunc.InputReg, Name = "Сопротивление пластины 3", Measure = "Ом", Description = "Rn3", MinValue = 0, MaxValue = 404 };
            ListDop.Add(ResistPlast3);
            CurrPolyar = new RegisterFloat() { Address = 0x48, CodeFunc = ModbusFunc.InputReg, Name = "Ток поляризации", Measure = "мА", Description = "Iпол", MinValue = -100, MaxValue = 100 };
            ListDop.Add(CurrPolyar);

            InfoReg = new RegisterInfo() { Name = "Информация" };

        }

        public override Task StartRequestValue()
        {
            ReadInfoRegister(InfoReg);
            ReadRegisters(ListWriteControl);
            ReadRegisters(ListCoil);

            ReadRegisters(ListInput);
            ReadRegisters(ListBI);
            ReadRegisters(ListStatus);
            ReadRegisters(ListDop);
            ReadRegisters(ListServices);
            return Task.CompletedTask;
        }


        public override Task RequestValue()
        {
            ReadRegisters(ListInput);
            ReadRegisters(ListBI);
            ReadRegisters(ListStatus);
            ReadRegisters(ListDop);
            ReadRegisters(ListServices);
            return Task.CompletedTask;
        }

        protected override void CheckListRegister()
        {
            CheckReg(ListInput);
            CheckReg(ListBI);
            CheckReg(ListStatus);
            CheckReg(ListDop);
            CheckReg(ListServices);

            CheckReg(ListWriteControl);
            CheckReg(ListCoil);
        }

    }
}
