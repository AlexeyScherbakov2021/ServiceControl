using ServiceControl.Based;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace ServiceControl.Modbus
{
    internal class ComWork : IBasedProto
    {
        SerialPort com;
        public delegate void Function(string s);
        Function ReadString;

        public delegate void dFailEvent();
        dFailEvent ComFailEvent;

        public delegate void dGoodEvent();
        dGoodEvent ComGoodEvent;

        private readonly DispatcherTimer timer = new DispatcherTimer();

        private bool isConnect = false;



        public ComWork(string ComPort, int TimeOut)
        {
            com = new SerialPort(ComPort, 9600, Parity.None, 8, StopBits.One);
            com.ReadTimeout = TimeOut;
            com.DataReceived += Com_DataReceived;
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
        }

        public void SetEventConnection(dFailEvent funcFail)
        {
            ComFailEvent = funcFail;
        }      



        private void Timer_Tick(object sender, EventArgs e)
        {
            if (com.IsOpen)
            {
                //if(!isConnect)
                //    ComGoodEvent();
            }
            else
            {
                if (isConnect && ComFailEvent != null)
                {
                    //isConnect = false;
                    ComFailEvent();
                }
            }
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
            //cmd += " ;";
            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;
            byte[] unicodeBytes = unicode.GetBytes(cmd);
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            com.Write(asciiBytes, 0, asciiBytes.Length);
            //Thread.Sleep(500);

            Mouse.OverrideCursor = Cursors.Wait;
            while (com.IsOpen)
            { }
            Thread.Sleep(1000);
            com.Open();
            Mouse.OverrideCursor = null;
        }


        public bool CreateConnect()
        {
            try
            {
                com.Open();
                isConnect = true;
                timer.Start();
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
            isConnect = false;
        }



    }
}
