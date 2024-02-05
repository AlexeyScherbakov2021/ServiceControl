using ServiceControl.Commands;
using ServiceControl.Based;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ServiceControl.ViewModel
{
    internal class KS261_UCViewModel : Observable
    {
        public Device261 device { get; set; }
        //DispatcherTimer timer;

        public List<KIPData> ListKIP { get; set; }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS261_UCViewModel()
        {
        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS261_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {
            device = new Device261(mainViewModel, work, Slave);
            device.Start();

            ListKIP = new List<KIPData>();
            for(int i = 0; i < Device261.CountKIP; i++)
            {
                KIPData kip = new KIPData();
                kip.RegStatus = device.ListStatus[i];
                kip.RegCurrentPolyar = device.ListCurrentPol[i];
                kip.RegPolyarPot = device.ListPolPot[i];
                kip.RegSummPot = device.ListSummPot[i];
                kip.RegtResistDK1 = device.ListResistDK1[i];
                kip.RegtResistDK2 = device.ListResistDK2[i];
                kip.RegtResistDK3 = device.ListResistDK3[i];
                kip.RegProtectCurrent = device.ListProtectCurrent[i];
                kip.RegDeepCorr = device.ListDeepCorr[i];
                kip.RegSpeedCorr = device.ListSpeedCorr[i];
                ListKIP.Add(kip);
            }

        }

        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Установить текущее время
        //--------------------------------------------------------------------------------
        public ICommand WriteTimeCommand => new LambdaCommand(OnWriteTimeModeCommandExecuted, CanWriteTimeModeCommand);
        private bool CanWriteTimeModeCommand(object p) => device != null;
        private void OnWriteTimeModeCommandExecuted(object p)
        {
            device.RealTime.RealTimeValue = DateTime.Now;
            device.WriteRegister(device.RealTime);
        }

        #endregion
    }
}
