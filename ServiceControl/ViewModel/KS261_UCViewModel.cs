using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public KS261_UCViewModel(MbWork work, int Slave)
        {
            device = new Device261(work, Slave);
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



            //timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(0, 0, 1);
            //timer.Tick += Timer_Tick;
            //timer.Start();
        }

        //--------------------------------------------------------------------------------------------
        // Событие таймера
        //--------------------------------------------------------------------------------------------
        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    device.ReadRegisters(device.ListStatus);
        //    device.ReadRegisters(device.ListCurrentPol);
        //    device.ReadRegisters(device.ListPolPot);
        //    device.ReadRegisters(device.ListSummPot);
        //    device.ReadRegisters(device.ListResistDK1);
        //    device.ReadRegisters(device.ListResistDK2);
        //    device.ReadRegisters(device.ListResistDK3);
        //    device.ReadRegisters(device.ListProtectCurrent);
        //    device.ReadRegisters(device.ListDeepCorr);
        //    device.ReadRegisters(device.ListSpeedCorr);
        //}
    }
}
