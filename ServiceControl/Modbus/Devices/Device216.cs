using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    public enum RezhStab : ushort { StabCurrent, StabSummPot, StabPolPot, StabNapr };

    internal class Device216 : Device
    {


        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device216(MbWork modb, byte slave) : base(modb, slave)
        {
            ListInput = new List<DoubleRegister>()
            {
                new DoubleRegister() { Address = 0x01, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Напряжение питающей сети 1 (основное)", Description = "Uc1", Scale = 0.1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x02, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "Значение счетчика эл.энергии сети 1 (осн.)", Description = "Сч.ЭЭ.1", Scale = 0.1, MinValue = 0, MaxValue = 9999999},
                new DoubleRegister() { Address = 0x04, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "", Description = "", Scale = 0.1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x05, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "", Description = "", Scale = 0.1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x07, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "", Description = "", Scale = 1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x08, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "", Description = "", Scale = 1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x0A, CodeFunc = ModbusFunc.InputReg, Size = 2, Name = "", Description = "", Scale = 1, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x0C, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Выходной ток", Description = "", Scale = 0.01, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x0D, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Выходное напряжение", Description = "", Scale = 0.01, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x0E, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "Защитный потенциал суммарный", Description = "", Scale = 0.01, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x0F, CodeFunc = ModbusFunc.InputReg, Size = 1, Name = "", Description = "", Scale = 0.01, MinValue = 0, MaxValue = 3000},

                new DoubleRegister() { Address = 0x1D, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x1E, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0xFE, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x20, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x21, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x22, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x23, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x24, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x25, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x26, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x27, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x28, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x29, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x2A, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x2B, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
                new DoubleRegister() { Address = 0x2C, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", Scale = 0.001, MinValue = 0, MaxValue = 65535},
            };

            ListInputShort = new List<UshortRegister>()
            {
                new UshortRegister() { Address = 0x10, CodeFunc = ModbusFunc.InputReg, Name = "Режим управления станцией", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x11, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x12, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x13, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x14, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x15, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x16, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x17, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x18, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x19, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x1A, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x1B, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
                new UshortRegister() { Address = 0x1C, CodeFunc = ModbusFunc.InputReg, Name = "", Description = "", MinValue = 0, MaxValue = 3},
              

            };



            ListDiscret = new List<BoolRegister>()
            {
                new BoolRegister() { Address = 0x01, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "Несанкционированный доступ в шкаф", Description = "ТС1 (Дверь)", ResultText0 = "дверь закрыта", ResultText1 = "дверь открыта"},
                new BoolRegister() { Address = 0x02, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "", Description = "", ResultText0 = "местный", ResultText1 = "дистанционный"},
                new BoolRegister() { Address = 0x03, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "", Description = "", ResultText0 = "исправна (работа)", ResultText1 = "неисправна (авария)"},
                new BoolRegister() { Address = 0x04, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "", Description = "", ResultText0 = "норма (нет обрыва)", ResultText1 = "неисправна (авария)"},
                new BoolRegister() { Address = 0x05, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "", Description = "", ResultText0 = "основнык", ResultText1 = "резервные"},
                new BoolRegister() { Address = 0x06, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "", Description = "", ResultText0 = "разрыв", ResultText1 = "замкнут"},
                new BoolRegister() { Address = 0x07, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "", Description = "", ResultText0 = "разрыв", ResultText1 = "замкнут"},
                new BoolRegister() { Address = 0x08, CodeFunc = ModbusFunc.Discrete, Size = 1, Name = "", Description = "", ResultText0 = "разрыв", ResultText1 = "замкнут"},
            };


            ListHolding = new List<DoubleRegister>()
            {
                new DoubleRegister() { Address = 0x81, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "Задание выходного тока", Description = "Iуст", Scale = 0.01, MinValue = 0, MaxValue = 15000},
                new DoubleRegister() { Address = 0x82, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "", Description = "", Scale = 0.01, MinValue = 0, MaxValue = 3000},
                new DoubleRegister() { Address = 0x83, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "", Description = "", Scale = 0.01, MinValue = -500, MaxValue = 0},
                new DoubleRegister() { Address = 0x85, CodeFunc = ModbusFunc.Holding, Size = 1, Name = "", Description = "", Scale = 0.01, MinValue = 0, MaxValue = 10000},
            };

            ListHoldingShort = new List<UshortRegister>()
            {
                new UshortRegister() { Address = 0x84, CodeFunc = ModbusFunc.Holding, Name = "", Description = "", MinValue = 0, MaxValue = 3},
            };


            ListCoil = new List<BoolRegister>() 
            {
                new BoolRegister() { Address = 0x81, CodeFunc = ModbusFunc.CoilRead, Size = 1, Name = "Дистанц. откл. вкл. модулей силовых", Description = "ТУ1 (ДО СМ)", ResultText0 = "выключить", ResultText1 = "включить"},
            };
        }



    }
}
