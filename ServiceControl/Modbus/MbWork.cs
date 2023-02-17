using Modbus.Device;
using Modbus.Extensions.Enron;
using Modbus.IO;
using Modbus.Message;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace ServiceControl.Modbus
{
    public enum ModbusFunc { None, CoilRead, Discrete, Holding, InputReg };
    public enum Protocol { COM, TCP};

    internal class MbWork
    {
        ModbusMaster master;
        TcpClient tcp;
        SerialPort com;
        private string Host;
        private int Port;
        private string ComPort;
        private int TimeOut = 1000;

        public MbWork(string connect, int val, Protocol proto)
        {
            if (proto == Protocol.TCP)
            {
                Host = connect;
                Port = val;
            }
            else
            {
                ComPort = connect;
                TimeOut = val;
            }
        }

        //public MbWork(string comPort)
        //{
        //    ComPort = comPort;
        //}


        public bool CreateConnect()
        {
            try
            {
                if (Port != 0)
                {
                    tcp = new TcpClient();
                    tcp.Connect(Host, Port);
                    if (!tcp.Connected)
                        return false;

                    master = ModbusIpMaster.CreateIp(tcp);
                }
                else
                {
                    //string[] ports = SerialPort.GetPortNames();
                    com = new SerialPort(ComPort, 9600, Parity.None, 8, StopBits.One);
                    if(TimeOut != 0)
                        com.ReadTimeout = TimeOut;

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

        public void Disconnect()
        {
            if (Port != 0)
            {
                tcp.Close();
                tcp.Dispose();
            }
            else
            {
                com.Close();
                com.Dispose();
            }

            master.Dispose();
            master = null;
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
            catch(TimeoutException te)
            {
                throw te;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public ushort[] ReadInfoRegister(ushort Size, byte Slave)
        {
            try
            {
                ushort[] read = master.ReadInfoRegisters(Slave, Size);
                return read;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }


        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public bool[] ReadRegisterCoil(ushort Address, ushort Size, byte Slave)
        {
            try
            {
                bool[] read = master.ReadCoils(Slave, Address, Size);
                return read;
            }
            catch (TimeoutException te)
            {
                throw te;
                //return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }

        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public bool[] ReadRegisterDiscret(ushort Address, ushort Size, byte Slave)
        {
            try
            {
                bool[] read = master.ReadInputs(Slave, Address, Size);
                return read;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }

        }

        //----------------------------------------------------------------------------------------------
        // запись регистра
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(ushort Address, ushort val, byte Slave)
        {
            try
            {
                master.WriteSingleRegister(Slave, Address, val);
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

            }
        }

        public void WriteRegister(ushort Address, ushort[] val, byte Slave)
        {
            try
            {
                master.WriteMultipleRegisters(Slave, Address, val);
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
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
