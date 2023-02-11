using Modbus.Device;
using Modbus.Extensions.Enron;
using Modbus.IO;
using Modbus.Message;
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


        public bool CreateConnect()
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
                    //com.ReadTimeout = 1000;
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
        public ushort[] ReadRegisterInput(ushort Address, ushort Size, byte Slave)
        {
            try
            {
                ushort[] read = master.ReadInputRegisters(Slave, Address, Size);
                return read;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public ushort[] ReadRegisterHolding(ushort Address, ushort Size, byte Slave)
        {
            try
            {
                ushort[] read = master.ReadHoldingRegisters(Slave, Address, Size);
                return read;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        //public void ReadRegister(ushort Address, ushort Size, byte Slave)
        //{
            //    ushort[] read = master.ReadInputRegisters(Slave, Address, Size);
            //    return read;
            //reg.SetResultValues(read); 
        //}

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        //public ushort[] ReadRegisterHolding(ushort Address, ushort Size, byte Slave)
        //{
        //    ushort[] read = master.ReadHoldingRegisters(Slave, Address, Size);
        //    return read;
        //    //return reg.SetResultValues(read); 
        //}


        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public bool[] ReadRegisterCoil(ushort Address, ushort Size, byte Slave)
        {
            bool[] read = master.ReadCoils(Slave, Address, Size);
            return read; 

        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public bool[] ReadRegisterDiscret(ushort Address, ushort Size, byte Slave)
        {
            bool[] read = master.ReadInputs(Slave, Address, Size);
            return read;
        }

        //----------------------------------------------------------------------------------------------
        // запись регистра
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(ushort Address, ushort val, byte Slave)
        {
            master.WriteSingleRegister(Slave, Address, val);
        }

        public void WriteRegister(ushort Address, ushort[] val, byte Slave)
        {
            master.WriteMultipleRegisters(Slave, Address, val);
        }

        //----------------------------------------------------------------------------------------------
        // запись регистра
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(ushort Address, bool val, byte Slave)
        {
            master.WriteSingleCoil(Slave, Address, val);
        }

    }
}
