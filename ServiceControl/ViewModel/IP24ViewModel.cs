using ServiceControl.Based;
using ServiceControl.Commands;
using ServiceControl.Modbus;
using ServiceControl.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ServiceControl.Modbus.Devices;

namespace ServiceControl.ViewModel
{
    internal class IP24ViewModel : Observable
    {
        private readonly MainWindowViewModel mainWindow;
        private readonly ComWork work;
        private Stopwatch sw = new Stopwatch();
        private bool IsStart = true;
        private string TempString;

        private string _resString;
        public string ResString
        {
            get => _resString;
            set
            {
                Set(ref _resString, value);
            }
        }

        public bool IsPause { get; set; }

        public int BU { get; set; }
        public int Imax { get; set; }
        public int Umax { get; set; }


        public IP24ViewModel()
        {
            
        }

        public IP24ViewModel(MainWindowViewModel mainViewModel, ComWork wrk)
        {
            mainWindow = mainViewModel;
            work = wrk;
            work.setReadEvent(Read);
            work.SetEventConnection(EventFailConnection);
            sw.Start();
        }

        public void EventFailConnection()
        {
            if (!work.CreateConnect())
            {
                mainWindow.SetStatusConnection(StatusConnect.Disconnected);
            }
            else
            {
                sw.Restart();
                IsStart = true;
                mainWindow.SetStatusConnection(StatusConnect.Connected);
            }

        }


        public void Read(string readString)
        {
            if (IsPause)
            {
                IsStart = true;
                sw.Restart();
                return;
            }

            long ms = sw.ElapsedMilliseconds;

            if (ms < 2500 && !IsStart)
                TempString += readString;
            else if( ms >= 2500)
            {
                ResString = TempString;
                TempString = readString;
                IsStart = false;
            }
            sw.Restart();
        }

        #region Команды
        //--------------------------------------------------------------------------------
        // Команда Отправить
        //--------------------------------------------------------------------------------
        public ICommand SendCommand => new LambdaCommand(OnSendCommandExecuted, CanSendCommand);
        private bool CanSendCommand(object p) => true;
        private void OnSendCommandExecuted(object p)
        {
            if(p is string s)
            {
                string cmd;

                switch(s)
                {
                    case "BU":
                        cmd = $"BU_Address={BU}";
                        break;
                    case "Imax":
                        cmd = $"I_max={Imax}";
                        break;
                    case "Umax":
                        cmd = $"U_max={Umax}";
                        break;
                    default:
                        return;
                }

                IsStart = true;
                work.Send(cmd);
                //IsStart = true;
            }
        }

        #endregion
    }
}
