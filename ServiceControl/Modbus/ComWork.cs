using ServiceControl.Based;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus
{
    internal class ComWork : IBasedProto
    {
        SerialPort com;
        public delegate void Function(string s);
        Function ReadString;

        public ComWork(string ComPort, int TimeOut)
        {
            com = new SerialPort(ComPort, 9600, Parity.None, 8, StopBits.One);
            com.ReadTimeout = TimeOut;
            com.DataReceived += Com_DataReceived;
        }


        public void setReadEvent(Function func)
        {
            ReadString = func;
        }

        private void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string s = com.ReadExisting();
            ReadString(s);
        }

        public void Send(string cmd)
        {
            //cmd += "\0";
            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;
            byte[] unicodeBytes = unicode.GetBytes(cmd);
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            com.Write(asciiBytes, 0, asciiBytes.Length);
        }

        public bool CreateConnect()
        {
            try
            {
                com.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            com.Close();
        }



    }
}
