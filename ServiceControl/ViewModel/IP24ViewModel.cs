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

namespace ServiceControl.ViewModel
{
    internal class IP24ViewModel : Observable
    {
        private readonly ComWork work;
        private Stopwatch sw = new Stopwatch();
        private bool IsStart = true;

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
            work = wrk;
            work.setReadEvent(Read);
        }

        public void Read(string readString)
        {
            long ms = sw.ElapsedMilliseconds;
            if (ms < 2500 && !IsStart)
                ResString += readString;
            else if( ms >= 2500)
            {
                ResString = readString;
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

                work.Send(cmd);
            }
        }

        #endregion
    }
}
