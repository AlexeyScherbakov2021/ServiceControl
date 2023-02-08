using Modbus.Device;
using Modbus.Extensions.Enron;
using Modbus.IO;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ServiceControl.Modbus
{
    public enum ModbusFunc { None, CoilRead, Discrete, Holding, InputReg, CoilWrite, HoldingWrite };

    internal class MbWork
    {
        ModbusIpMaster master;

        public MbWork(int Port)
        {
            TcpClient tcp = new TcpClient("localhost", Port);
            master = ModbusIpMaster.CreateIp(tcp);
        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public double ReadRegisterInput(Register<ushort, double> reg, byte Slave)
        {
            ushort[] read = master.ReadInputRegisters(Slave, reg.Address, reg.Size);
            return reg.GetResult(read); 
        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public double ReadRegisterHolding(Register<ushort, double> reg, byte Slave)
        {
            ushort[] read = master.ReadHoldingRegisters(Slave, reg.Address, reg.Size);
            return reg.GetResult(read); 
        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public double ReadRegisterInput(Register<ushort, ushort> reg, byte Slave)
        {
            ushort[] read = master.ReadInputRegisters(Slave, reg.Address, reg.Size);
            return reg.GetResult(read); 
        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public double ReadRegisterHolding(Register<ushort, ushort> reg, byte Slave)
        {
            ushort[] read = master.ReadHoldingRegisters(Slave, reg.Address, reg.Size);
            return reg.GetResult(read); 
        }


        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public bool ReadRegisterCoil(Register<bool, bool> reg, byte Slave)
        {
            bool[] read = master.ReadCoils(Slave, reg.Address, reg.Size);
            return reg.GetResult(read);

        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public bool ReadRegisterDiscret(Register<bool, bool> reg, byte Slave)
        {
            bool[] read = master.ReadInputs(Slave, reg.Address, reg.Size);
            return reg.GetResult(read);
        }

        //----------------------------------------------------------------------------------------------
        // запись регистра
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(Register<ushort, double> reg, byte Slave)
        {
            ushort res = reg.SetUshort();
            master.WriteSingleRegister(Slave, reg.Address, res);
        }

    }
}
