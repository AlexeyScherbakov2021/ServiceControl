using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{

    internal class Device216 : Device
    {
        public List<DoubleRegister> ListInputDK { get; set; }

        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device216(MbWork modb, int slave) : base(modb, slave)
        {
            ListInput = new List<DoubleRegister>()
            {
                new DoubleRegister() { Address = 0x01, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Напряжение питающей сети 1 (основное)", Description = "Uc1", Scale = 0.1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x02, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "Значение счетчика эл.энергии сети 1 (осн.)", Description = "Сч.ЭЭ.1", Scale = 0.1, MinValue = 0, MaxValue = 9999999},
                new DoubleRegister() { Address = 0x04, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Напряжение питающей сети 2 (резервное)", Description = "Uc2", Scale = 0.1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x05, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "Значение счетчика эл.энергии сети 1 (рез.)", Description = "Сч.ЭЭ.2", Scale = 0.1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x07, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Температура в шкафу", Description = "Т°", Scale = 1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x08, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "Время наработки", Description = "СВН", Scale = 1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x0A, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "Время защиты сооружения", Description = "СВЗ", Scale = 1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x0C, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Выходной ток", Description = "Iвых", Scale = 0.01, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x0D, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Выходное напряжение", Description = "Uвых", Scale = 0.01, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x0E, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Защитный потенциал суммарный", Description = "Uсп", Scale = 0.01, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x0F, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Защитный потенциал поляризационный", Description = "Uпп", Scale = 0.01, MinValue = 0, MaxValue = 3000},

            };


            ListInputDK = new List<DoubleRegister>()
            {
                new DoubleRegister() { Address = 0x1D, CodeFunc = ModbusFunc.InputReg, Name = "Скорость коррозии ИСК1", Description = "СК_ИКП1", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x1E, CodeFunc = ModbusFunc.InputReg, Name = "Глубина коррозии ИСК1", Description = "ГК_ИКП1", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0xFE, CodeFunc = ModbusFunc.InputReg, Name = "Скорость коррозии ИСК2", Description = "СК_ИКП2", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x20, CodeFunc = ModbusFunc.InputReg, Name = "Глубина коррозии ИСК2", Description = "ГК_ИКП2", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x21, CodeFunc = ModbusFunc.InputReg, Name = "Скорость коррозии ИСК3", Description = "СК_ИКП3", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x22, CodeFunc = ModbusFunc.InputReg, Name = "Глубина коррозии ИСК3", Description = "ГК_ИКП3", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x23, CodeFunc = ModbusFunc.InputReg, Name = "Скорость коррозии ИСК4", Description = "СК_ИКП4", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x24, CodeFunc = ModbusFunc.InputReg, Name = "Глубина коррозии ИСК4", Description = "ГК_ИКП4", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x25, CodeFunc = ModbusFunc.InputReg, Name = "Скорость коррозии ИСК5", Description = "СК_ИКП5", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x26, CodeFunc = ModbusFunc.InputReg, Name = "Глубина коррозии ИСК5", Description = "ГК_ИКП5", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x27, CodeFunc = ModbusFunc.InputReg, Name = "Скорость коррозии ИСК6", Description = "СК_ИКП6", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x28, CodeFunc = ModbusFunc.InputReg, Name = "Глубина коррозии ИСК6", Description = "ГК_ИКП6", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x29, CodeFunc = ModbusFunc.InputReg, Name = "Скорость коррозии ИСК7", Description = "СК_ИКП7", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x2A, CodeFunc = ModbusFunc.InputReg, Name = "Глубина коррозии ИСК7", Description = "ГК_ИКП7", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x2B, CodeFunc = ModbusFunc.InputReg, Name = "Скорость коррозии ИСК8", Description = "СК_ИКП8", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x2C, CodeFunc = ModbusFunc.InputReg, Name = "Глубина коррозии ИСК8", Description = "ГК_ИКП8", Scale = 0.001, MinValue = 0, MaxValue = 65535},
            };

            ListInputShort = new List<UshortRegister>()
            {
                new RegisterStab() { Address = 0x10, CodeFunc = ModbusFunc.InputReg, Name = "Режим управления станцией", Description = "", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x11, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 1", Description = "ССМ1", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x12, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 2", Description = "ССМ2", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x13, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 3", Description = "ССМ3", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x14, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 4", Description = "ССМ4", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x15, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 5", Description = "ССМ5", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x16, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 6", Description = "ССМ6", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x17, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 7", Description = "ССМ7", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x18, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 8", Description = "ССМ8", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x19, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 9", Description = "ССМ9", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x1A, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 10", Description = "ССМ10", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x1B, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 11", Description = "ССМ11", MinValue = 0, MaxValue = 3},
                new RegisterMS() { Address = 0x1C, CodeFunc = ModbusFunc.InputReg, Name = "Состояние модуля силового 12", Description = "ССМ12", MinValue = 0, MaxValue = 3},
              

            };



            ListDiscret = new List<BoolRegister>()
            {
                new BoolRegister() { Address = 0x01, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Несанкционированный доступ в шкаф", Description = "ТС1 (Дверь)", ResultText0 = "дверь закрыта", ResultText1 = "дверь открыта"},
                new BoolRegister() { Address = 0x02, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Режи упр. станцией", Description = "ТС2 (ДУ)", ResultText0 = "местный", ResultText1 = "дистанционный"},
                new BoolRegister() { Address = 0x03, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Неисправность станции", Description = "ТС3 (Неисправность СКЗ)", ResultText0 = "исправна (работа)", ResultText1 = "неисправна (авария)"},
                new BoolRegister() { Address = 0x04, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Обрыв изм. цепей", Description = "ТС4 (Обрыв ЭС/Т)", ResultText0 = "норма (нет обрыва)", ResultText1 = "неисправна (авария)"},
                new BoolRegister() { Address = 0x05, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Включение группы осн. или рез. МС (СКЗ)", Description = "ТС5 (основные-резервные)", ResultText0 = "основнык", ResultText1 = "резервные"},
                new BoolRegister() { Address = 0x06, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Индикатор скорости корр. 1", Description = "ТС6-1 (ДСК1)", ResultText0 = "разрыв", ResultText1 = "замкнут"},
                new BoolRegister() { Address = 0x07, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Индикатор скорости корр. 2", Description = "ТС6-2 (ДСК2)", ResultText0 = "разрыв", ResultText1 = "замкнут"},
                new BoolRegister() { Address = 0x08, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Индикатор скорости корр. 3", Description = "ТС6-3 (ДСК3)", ResultText0 = "разрыв", ResultText1 = "замкнут"},
            };


            ListHolding = new List<DoubleRegister>()
            {
                new DoubleRegister() { Address = 0x81, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "Задание выходного тока", Description = "Iуст", Scale = 0.01, MinValue = 0, MaxValue = 15000},
                new DoubleRegister() { Address = 0x82, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "Задание суммарного потенциала", Description = "Uпотс", Scale = 0.01, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x83, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "Задание поляризационного потенциала", Description = "Uпотп", Scale = 0.01, MinValue = -500, MaxValue = 0},
                new DoubleRegister() { Address = 0x85, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "Задание выходного напряжения", Description = "Uуст", Scale = 0.01, MinValue = 0, MaxValue = 10000},
            };

            ListHoldingShort = new List<UshortRegister>()
            {
                new UshortRegister() { Address = 0x84, CodeFunc = ModbusFunc.Holding, Name = "Управление режимами стабилизации станции", Description = "Упр.", MinValue = 0, MaxValue = 3},
            };


            ListCoil = new List<BoolRegister>() 
            {
                new BoolRegister() { Address = 0x81, CodeFunc = ModbusFunc.CoilRead, Size = 1, Name = "Дистанц. откл. вкл. модулей силовых", Description = "ТУ1 (ДО СМ)", ResultText0 = "выключить", ResultText1 = "включить"},
            };
        }



    }
}
