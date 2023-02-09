using Modbus.Device;
using Modbus.Extensions.Enron;
using Modbus.IO;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace ServiceControl.Modbus
{
    public enum ModbusFunc { None, CoilRead, Discrete, Holding, InputReg, CoilWrite, HoldingWrite };

    internal class MbWork
    {
        ModbusMaster master;
        private string Host;
        private int Port;
        private string ComPort;

        public MbWork(string host,  int port)
        {
            Host = host;
            Port = port;

        }

        public MbWork(string comPort)
        {
            ComPort = comPort;
        }


        public  bool CreateConnect()
        {
            try
            {
                if (Port != 0)
                {
                    TcpClient tcp = new TcpClient(Host, Port);
                    master = ModbusIpMaster.CreateIp(tcp);
                }
                else
                {
                    //string[] ports = SerialPort.GetPortNames();
                    SerialPort com = new SerialPort(ComPort, 9600, Parity.None, 8, StopBits.One);
                    com.Open();
                    if (!com.IsOpen)
                        return false;

                    master = ModbusSerialMaster.CreateRtu(com);
                }

                return true;
            }
            catch(Exception )
            {
                return false;
            }

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

        //----------------------------------------------------------------------------------------------
        // запись регистра
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(Register<ushort, ushort> reg, byte Slave)
        {
            ushort res = reg.SetUshort();
            master.WriteSingleRegister(Slave, reg.Address, res);
        }

    }
}
