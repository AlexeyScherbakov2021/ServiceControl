using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal abstract class Device
    {
        public string Name;
        public List<DoubleRegister> ListInput { get; set; }
        public List<DoubleRegister> ListHolding { get; set; }
        public List<UshortRegister> ListInputShort { get; set; }
        public List<UshortRegister> ListHoldingShort { get; set; }
        public List<BoolRegister> ListCoil { get; set; }
        public List<BoolRegister> ListDiscret { get; set; }
        public byte Slave;
        protected MbWork modbus;

        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device(MbWork modb, int slave)
        {
            modbus = modb;
            Slave = (byte)slave;
        }


        //----------------------------------------------------------------------------------------------
        // чтение списка регистров
        //----------------------------------------------------------------------------------------------
        public void ReadRegisters(IEnumerable<Register<ushort, double>> listReg)
        {
            foreach (var item in listReg)
            {
                if (item.CodeFunc == ModbusFunc.Holding)
                    modbus.ReadRegisterHolding(item, Slave);
                if (item.CodeFunc == ModbusFunc.InputReg)
                    modbus.ReadRegisterInput(item, Slave);
            }
        }

        //----------------------------------------------------------------------------------------------
        // чтение списка регистров
        //----------------------------------------------------------------------------------------------
        public void ReadRegisters(IEnumerable<Register<ushort, ushort>> listReg)
        {
            foreach (var item in listReg)
            {
                if (item.CodeFunc == ModbusFunc.Holding)
                    modbus.ReadRegisterHolding(item, Slave);
                if (item.CodeFunc == ModbusFunc.InputReg)
                    modbus.ReadRegisterInput(item, Slave);
            }
        }

        //----------------------------------------------------------------------------------------------
        // чтение списка регистров
        //----------------------------------------------------------------------------------------------
        public void ReadRegistersBool(IEnumerable<Register<bool, bool>> listReg)
        {
            foreach (var item in listReg)
            {
                if(item.CodeFunc == ModbusFunc.CoilRead)
                    modbus.ReadRegisterCoil(item, Slave);
                if(item.CodeFunc == ModbusFunc.Discrete)
                    modbus.ReadRegisterDiscret(item, Slave);
            }
        }

        //----------------------------------------------------------------------------------------------
        // запись списка регистров
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(Register<ushort, double> Reg)
        {
            modbus.WriteRegister(Reg, Slave);
        }

    }
}
