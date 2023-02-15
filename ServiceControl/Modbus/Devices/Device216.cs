using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ServiceControl.Modbus.Registers
{

    internal class Device216 : Device
    {
        public const int CountDK = 8;
        public const int CountMS = 12;


        public RegisterFloat NaprSeti1;
        public RegisterFloat CountEE1;
        public RegisterFloat NaprSeti2;
        public RegisterFloat CountEE2;
        public RegisterInt Temper;
        public RegisterInt TimeWork;
        public RegisterInt TimeProtect;
        public RegisterInt TimeWorkWrite;
        public RegisterInt TimeProtectWrite;
        public RegisterFloat CurrOutput;
        public RegisterFloat NaprOutput;
        public RegisterFloat ProtectPotenSumm;
        public RegisterFloat ProtectPotenPol;

        public RegisterMS[] MS;
        public RegisterFloat[] SpeedDK;
        public RegisterFloat[] DeepDK;


        public RegisterBool IllegalAccess;
        public RegisterBool ControlMode;
        public RegisterBool Fault;
        public RegisterBool BreakCirc;
        public RegisterBool OnMS;
        public RegisterBool SpeedCorr1;
        public RegisterBool SpeedCorr2;
        public RegisterBool SpeedCorr3;
        public RegisterBool OnOffMS { get; set; }
        public RegisterInt TempCoolerOnWrite;
        public RegisterInt TempCoolerOn;
        public RegisterInt TempCoolerOffWrite;
        public RegisterInt TempCoolerOff;
        public RegisterNapr4896 ModeNaprOutput { get; set; }
        public RegisterNapr4896 ModeNaprOutputWrite { get; set; }
        public RegisterRT RealTime { get; set; }
        public RegisterRT RealTimeWrite { get; set; }

        public RegisterStab Stabil { get; set; }
        public RegisterFloat SetCurrOutput;
        public RegisterFloat SetSummPotOutput;
        public RegisterFloat SetPolPotOutput;
        public RegisterStab SetMode { get; set; }
        public RegisterFloat SetNaprOutput;
        public RegisterInt ResistPlast1;
        public RegisterInt ResistPlast2;
        public RegisterInt ResistPlast3;
        public RegisterFloat CurrPolyar;

        public RegisterInfo InfoReg { get; set; }

        List<Register> ListInput;
        List<RegisterBool> ListStatus;
        List<Register> ListWriteControl;
        List<Register> ListWriteControl2;
        //List<RegisterBool> ListCoil;
        List<Register> ListServices;
        //List<Register> ListServices2;
        List<Register> ListDop;

        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device216(MbWork modb, int slave) : base(modb, slave)
        {
            // список входных регистров
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListInput = new List<Register>();

            NaprSeti1 = new RegisterFloat() { Address = 0x01, CodeFunc = ModbusFunc.InputReg, Size = 1, 
                Name = "Напряжение питающей сети 1 (основное)", Measure = "В", Description = "Uc1", 
                Scale = 0.1f, MinValue = 0, MaxValue = 300 };
            ListInput.Add(NaprSeti1);

            CountEE1 = new RegisterFloat() { Address = 0x02, CodeFunc = ModbusFunc.InputReg, Size = 2, 
                Name = "Значение счетчика эл.энергии сети 1 (осн.)", Measure = "кВт*ч", Description = "Сч.ЭЭ.1", 
                Scale = 0.1f, MinValue = 0, MaxValue = 999999.9f };
            ListInput.Add(CountEE1);

            NaprSeti2 = new RegisterFloat() { Address = 0x04, CodeFunc = ModbusFunc.InputReg, Size = 1, 
                Name = "Напряжение питающей сети 2 (резервное)", Measure = "В", Description = "Uc2", 
                Scale = 0.1f, MinValue = 0, MaxValue = 300 };
            ListInput.Add(NaprSeti2);

            CountEE2 = new RegisterFloat() { Address = 0x05, CodeFunc = ModbusFunc.InputReg, Size = 2, 
                Name = "Значение счетчика эл.энергии сети 1 (рез.)", Measure = "кВт*ч", Description = "Сч.ЭЭ.2", 
                Scale = 0.1f, MinValue = 0, MaxValue = 999999.9f };
            ListInput.Add(CountEE2);

            Temper = new RegisterInt() { Address = 0x07, CodeFunc = ModbusFunc.InputReg, Size = 1, 
                Name = "Температура в шкафу", Measure = "°С", Description = "Т°", 
                MinValue = -45, MaxValue = 100 };
            ListInput.Add(Temper);

            TimeWork = new RegisterInt() { Address = 0x08, CodeFunc = ModbusFunc.InputReg, Size = 2, 
                Name = "Время наработки", Measure = "ч", Description = "СВН", 
                MinValue = 0, MaxValue = 999999 };
            ListInput.Add(TimeWork);

            TimeProtect = new RegisterInt() { Address = 0x0A, CodeFunc = ModbusFunc.InputReg, Size = 2, 
                Name = "Время защиты сооружения", Measure = "ч", Description = "СВЗ", 
                MinValue = 0, MaxValue = 999999 };
            ListInput.Add(TimeProtect);

            CurrOutput = new RegisterFloat() { Address = 0x0C, CodeFunc = ModbusFunc.InputReg, Size = 1, 
                Name = "Выходной ток", Measure = "А", Description = "Iвых", 
                Scale = 0.01f, MinValue = 0, MaxValue = 150 };
            ListInput.Add(CurrOutput);

            NaprOutput = new RegisterFloat() { Address = 0x0D, CodeFunc = ModbusFunc.InputReg, Size = 1, 
                Name = "Выходное напряжение", Measure = "В", Description = "Uвых", 
                Scale = 0.01f, MinValue = 0, MaxValue = 100 };
            ListInput.Add(NaprOutput);

            ProtectPotenSumm = new RegisterFloat() { Address = 0x0E, CodeFunc = ModbusFunc.InputReg, Size = 1, 
                Name = "Защитный потенциал суммарный", Measure = "В", Description = "Uсп", 
                Scale = 0.01f, MinValue = -5, MaxValue = 5 };
            ListInput.Add(ProtectPotenSumm);

            ProtectPotenPol = new RegisterFloat() { Address = 0x0F, CodeFunc = ModbusFunc.InputReg, Size = 1, 
                Name = "Защитный потенциал поляризационный", Measure = "В", Description = "Uпп", 
                Scale = 0.01f, MinValue = -5, MaxValue = 5 };
            ListInput.Add(ProtectPotenPol);

            Stabil = new RegisterStab() { Address = 0x10, CodeFunc = ModbusFunc.InputReg, 
                Name = "Режим управления станцией", Description = "", MinValue = 0, MaxValue = 3 };
            ListInput.Add(Stabil);

            MS = new RegisterMS[CountMS];
            for (int i = 0; i < CountMS; i++)
            {
                MS[i] = new RegisterMS() { Number = i + 1, Address = (ushort)(0x11 + i), CodeFunc = ModbusFunc.InputReg, 
                    Name = $"Состояние модуля силового {i + 1}", Description = $"ССМ{i + 1}", MinValue = 0, MaxValue = 3 };
                ListInput.Add(MS[i]);
            }

            SpeedDK = new RegisterFloat[CountDK];
            DeepDK = new RegisterFloat[CountDK];
            for (int i = 0; i < CountDK; i++)
            {
                SpeedDK[i] = new RegisterFloat() { Address = (ushort)(0x1D + i * 2), CodeFunc = ModbusFunc.InputReg, 
                    Name = $"Скорость коррозии ИКП{i + 1}", Measure = "мм в год", Description = $"СК_ИКП{i + 1}", 
                    Scale = 0.001f, MinValue = 0, MaxValue = 65.535f, Number = i + 1 };
                ListInput.Add(SpeedDK[i]);

                DeepDK[i] = new RegisterFloat() { Address = (ushort)(0x1E + i * 2), CodeFunc = ModbusFunc.InputReg, 
                    Name = $"Глубина коррозии ИКП{i + 1}", Measure = "мм", Description = $"ГК_ИКП{i + 1}", 
                    Scale = 0.001f, MinValue = 0, MaxValue = 65.535f, Number = i + 1 };
                ListInput.Add(DeepDK[i]);

            }

            // список регистров статусов
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListStatus = new List<RegisterBool>();

            IllegalAccess = new RegisterBool() { Address = 0x01, CodeFunc = ModbusFunc.Discrete, Size = 1, 
                Name = "Несанкционированный доступ в шкаф", Description = "ТС1 (Дверь)", 
                ResultText0 = "дверь закрыта", ResultText1 = "дверь открыта", IsCorrectValue = false };
            ListStatus.Add(IllegalAccess);
            ControlMode = new RegisterBool() { Address = 0x02, CodeFunc = ModbusFunc.Discrete, Size = 1, 
                Name = "Режим упр. станцией", Description = "ТС2 (ДУ)", 
                ResultText0 = "местный", ResultText1 = "дистанционный", IsCorrectValue = true };
            ListStatus.Add(ControlMode);
            Fault = new RegisterBool() { Address = 0x03, CodeFunc = ModbusFunc.Discrete, Size = 1, 
                Name = "Неисправность станции", Description = "ТС3 (Неисправность СКЗ)", 
                ResultText0 = "исправна (работа)", ResultText1 = "неисправна (авария)", IsCorrectValue = false};
            ListStatus.Add(Fault);
            BreakCirc = new RegisterBool() { Address = 0x04, CodeFunc = ModbusFunc.Discrete, Size = 1, 
                Name = "Обрыв изм. цепей", Description = "ТС4 (Обрыв ЭС/Т)", 
                ResultText0 = "норма (нет обрыва)", ResultText1 = "неисправна (авария)", IsCorrectValue = false };
            ListStatus.Add(BreakCirc);
            OnMS = new RegisterBool() { Address = 0x05, CodeFunc = ModbusFunc.Discrete, Size = 1, 
                Name = "Включение группы осн. или рез. МС (СКЗ)", Description = "ТС5 (основные-резервные)", 
                ResultText0 = "основные", ResultText1 = "резервные", IsCorrectValue = false };
            ListStatus.Add(OnMS);
            SpeedCorr1 = new RegisterBool() { Address = 0x06, CodeFunc = ModbusFunc.Discrete, Size = 1, 
                Name = "Индикатор скорости корр. 1", Description = "ТС6-1 (ДСК1)", 
                ResultText0 = "разрыв", ResultText1 = "замкнут", IsCorrectValue = true };
            ListStatus.Add(SpeedCorr1);
            SpeedCorr2 = new RegisterBool() { Address = 0x07, CodeFunc = ModbusFunc.Discrete, Size = 1, 
                Name = "Индикатор скорости корр. 2", Description = "ТС6-2 (ДСК2)", 
                ResultText0 = "разрыв", ResultText1 = "замкнут", IsCorrectValue = true };
            ListStatus.Add(SpeedCorr2);
            SpeedCorr3 = new RegisterBool() { Address = 0x08, CodeFunc = ModbusFunc.Discrete, Size = 1, 
                Name = "Индикатор скорости корр. 3", Description = "ТС6-3 (ДСК3)", 
                ResultText0 = "разрыв", ResultText1 = "замкнут", IsCorrectValue = true };
            ListStatus.Add(SpeedCorr3);



            // список управляющих регистров
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListWriteControl = new List<Register>();

            SetCurrOutput = new RegisterFloat() { Address = 0x81, CodeFunc = ModbusFunc.Holding, Size = 1, 
                Name = "Задание выходного тока", Measure = "А", Description = "Iуст", 
                Scale = 0.01f, MinValue = 0, MaxValue = 150 };
            ListWriteControl.Add(SetCurrOutput);

            SetSummPotOutput = new RegisterFloat() { Address = 0x82, CodeFunc = ModbusFunc.Holding, Size = 1, 
                Name = "Задание суммарного потенциала", Measure = "В", Description = "Uпотс", 
                Scale = 0.01f, MinValue = -5, MaxValue = 0 };
            ListWriteControl.Add(SetSummPotOutput);

            SetPolPotOutput = new RegisterFloat() { Address = 0x83, CodeFunc = ModbusFunc.Holding, Size = 1, 
                Name = "Задание поляризационного потенциала", Measure = "В", Description = "Uпотп", 
                Scale = 0.01f, MinValue = -5, MaxValue = 0 };
            ListWriteControl.Add(SetPolPotOutput);

            SetMode = new RegisterStab() { Address = 0x84, CodeFunc = ModbusFunc.Holding, 
                Name = "Управление режимами стабилизации станции", Description = "Упр.", MinValue = 0, MaxValue = 3 };
            ListWriteControl.Add(SetMode);

            SetNaprOutput = new RegisterFloat() { Address = 0x85, CodeFunc = ModbusFunc.Holding, Size = 1, 
                Name = "Задание выходного напряжения", Measure = "В", Description = "Uуст", 
                Scale = 0.01f, MinValue = 0, MaxValue = 100 };
            ListWriteControl.Add(SetNaprOutput);


            ListWriteControl2 = new List<Register>();
            // Отдельные регистры для записи
            RealTimeWrite = new RegisterRT()
            {
                Address = 0xC1,
                CodeFunc = ModbusFunc.Holding,
                Name = "Реальное время",
                Measure = "сек",
                Size = 4,
                Description = "РВ",
                MinValue = 0,
                MaxValue = int.MaxValue
            };
            ListWriteControl2.Add(RealTimeWrite);


            TempCoolerOnWrite = new RegisterInt()
            {
                Address = 0xC5,
                CodeFunc = ModbusFunc.Holding,
                Name = "Температура включения вентилятора",
                Measure = "°С",
                Description = "Твкл.вент.",
                MinValue = -32769,
                MaxValue = 32678
            };
            ListWriteControl2.Add(TempCoolerOnWrite);

            TempCoolerOffWrite = new RegisterInt()
            {
                Address = 0xC6,
                CodeFunc = ModbusFunc.Holding,
                Name = "Температура выключения вентилятора",
                Measure = "°С",
                Description = "Твыкл.вент.",
                MinValue = -32769,
                MaxValue = 32768
            };
            ListWriteControl2.Add(TempCoolerOffWrite);



            TimeWorkWrite = new RegisterInt()
            {
                Address = 0xC7,
                CodeFunc = ModbusFunc.InputReg,
                Size = 2,
                Name = "Время наработки",
                Measure = "ч",
                Description = "СВН",
                MinValue = 0,
                MaxValue = 999999
            };


            TimeProtectWrite = new RegisterInt()
            {
                Address = 0xC9,
                CodeFunc = ModbusFunc.InputReg,
                Size = 2,
                Name = "Время защиты сооружения",
                Measure = "ч",
                Description = "СВЗ",
                MinValue = 0,
                MaxValue = 999999
            };


            ModeNaprOutputWrite = new RegisterNapr4896()
            {
                Address = 0xCD,
                CodeFunc = ModbusFunc.Holding,
                Name = "Режим выходного напряжения",
                Description = "Uрежим",
                Measure = "В",
                MinValue = 0,
                MaxValue = 1
            };


            // список управляющих статусов
            //--------------------------------------------------------------------------------------------------------------------------------------
            //ListCoil = new List<RegisterBool>();
            OnOffMS = new RegisterBool() { Address = 0x81, CodeFunc = ModbusFunc.CoilRead, Size = 1, 
                Name = "Дистанц.откл.вкл.модулей силовых", Description = "ТУ1 (ДО СМ)", 
                ResultText0 = "выключен", ResultText1 = "включен" };
            //ListCoil.Add(OnMS);



            // список сервисных регистров
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListServices = new List<Register>();

            RealTime = new RegisterRT()
            {
                Address = 0xC1,
                CodeFunc = ModbusFunc.Holding,
                Name = "Реальное время",
                Measure = "сек",
                Size = 4,
                Description = "РВ",
                MinValue = 0,
                MaxValue = int.MaxValue
            };
            ListServices.Add(RealTime);

            TempCoolerOn = new RegisterInt() { Address = 0xC5, CodeFunc = ModbusFunc.Holding, 
                Name = "Температура включения вентилятора", Measure = "°С", Description = "Твкл.вент.", 
                MinValue = -32769, MaxValue = 32678 };
            ListServices.Add(TempCoolerOn);

            TempCoolerOff = new RegisterInt() { Address = 0xC6, CodeFunc = ModbusFunc.Holding, 
                Name = "Температура выключения вентилятора", Measure = "°С", Description = "Твыкл.вент.", 
                MinValue = -32769, MaxValue = 32768 };
            ListServices.Add(TempCoolerOff);

            //var stab = new RegisterInt() { Address = 0xC7, CodeFunc = ModbusFunc.Holding, Name = "Заглушка", Size = 2, Description = "", MinValue = 0, MaxValue = 65535 };
            //ListServices.Add(stab);

            //ProtectTime = new RegisterInt() { Address = 0xC9, CodeFunc = ModbusFunc.Holding, Name = "Время защиты сооружения", Measure = "ч", Size = 2, Description = "СВЗ", MinValue = -596522, MaxValue = 596522 };
            //ListServices.Add(ProtectTime);


            // список сервисных регистров 2
            //--------------------------------------------------------------------------------------------------------------------------------------
            //ListServices2 = new List<Register>();
            //Year = new RegisterInt() { Address = 0xCB, CodeFunc = ModbusFunc.InputReg, Name = "Год выпуска устройства", Description = "Год", MinValue = 0, MaxValue = 65535 };
            //ListServices2.Add(Year);

            //Number = new RegisterInt() { Address = 0xCC, CodeFunc = ModbusFunc.InputReg, Name = "Порядковый номер устройства", Description = "Номер", MinValue = 0, MaxValue = 65535 };
            //ListServices2.Add(Number);

            ModeNaprOutput = new RegisterNapr4896() { Address = 0xCD, CodeFunc = ModbusFunc.Holding, 
                Name = "Режим выходного напряжения", Description = "Uрежим", Measure = "В", MinValue = 0, MaxValue = 1 };
            //ListServices2.Add(ModeNaprOutput);


            ListDop = new List<Register>();
            //RealTime = new RegisterRT()
            //{
            //    Address = 0x41,
            //    CodeFunc = ModbusFunc.InputReg,
            //    Name = "Реальное время",
            //    Measure = "сек",
            //    Size = 4,
            //    Description = "РВ",
            //    MinValue = 0,
            //    MaxValue = int.MaxValue
            //};
            //ListDop.Add(RealTime);

            ResistPlast1 = new RegisterInt() { Address = 0x45, CodeFunc = ModbusFunc.InputReg, 
                Name = "Сопротивление пластины 1", Measure = "Ом", Description = "Rn1", MinValue = 0, MaxValue = 404 };
            ListDop.Add(ResistPlast1);
            ResistPlast2 = new RegisterInt() { Address = 0x46, CodeFunc = ModbusFunc.InputReg, 
                Name = "Сопротивление пластины 2", Measure = "Ом", Description = "Rn2", MinValue = 0, MaxValue = 404 };
            ListDop.Add(ResistPlast2);
            ResistPlast3 = new RegisterInt() { Address = 0x47, CodeFunc = ModbusFunc.InputReg, 
                Name = "Сопротивление пластины 3", Measure = "Ом", Description = "Rn3", MinValue = 0, MaxValue = 404 };
            ListDop.Add(ResistPlast3);
            CurrPolyar = new RegisterFloat() { Address = 0x48, CodeFunc = ModbusFunc.InputReg, Scale = 0.1f,
                Name = "Ток поляризации", Measure = "мА", Description = "Iпол", MinValue = -10, MaxValue = 10 };
            ListDop.Add(CurrPolyar);

            InfoReg = new RegisterInfo() { Name = "Информация" };

            //CheckListRegister();

            //StartRequestValue();
        }

        //-------------------------------------------------------------------------------------------
        // 
        //-------------------------------------------------------------------------------------------
        public override Task StartRequestValue()
        {
            ReadInfoRegister(InfoReg);
            ReadRegisters(ListWriteControl);
            ReadRegisters(ListWriteControl2);
            ReadRegister(OnOffMS);
            ReadRegister(SetMode);
            ReadRegister(ModeNaprOutputWrite);

            ReadRegisters(ListInput);
            //MS[1].Value = (int)StatusMS.Off;
            //MS[3].Value = (int)StatusMS.Off;
            //MS[5].Value = (int)StatusMS.Avar;
            //MS[6].Value = (int)StatusMS.Absent;
            ReadRegisters(ListStatus);
            ReadRegisters(ListServices);
            ReadRegister(ModeNaprOutput);
            ReadRegisters(ListDop);

            return Task.CompletedTask;

        }


        //-------------------------------------------------------------------------------------------
        // 
        //-------------------------------------------------------------------------------------------
        public override Task RequestValue()
        {

            ReadRegisters(ListInput);

            //MS[1].Value = (int)StatusMS.Off;
            //MS[3].Value = (int)StatusMS.Off;
            //MS[5].Value = (int)StatusMS.Avar;
            //MS[6].Value = (int)StatusMS.Absent;

            //SpeedDK[0].Value = 2.334f;
            //DeepDK[0].Value = 4.674f;
            //SpeedDK[1].Value = 16.334f;
            //DeepDK[1].Value = 65.500f;

            ReadRegisters(ListStatus);
            ReadRegister(OnOffMS);
            ReadRegisters(ListServices);
            ReadRegisters(ListDop);
            ReadRegister(ModeNaprOutput);

            return Task.CompletedTask;
        }

        //-------------------------------------------------------------------------------------------
        // проверка последовательноти регистров в списках
        //-------------------------------------------------------------------------------------------
        protected override void CheckListRegister()
        {
            CheckReg(ListInput);
            CheckReg(ListStatus);
            CheckReg(ListWriteControl);
            CheckReg(ListWriteControl2);
            CheckReg(ListServices);
            //CheckReg(ListCoil);
        }
    }
}
