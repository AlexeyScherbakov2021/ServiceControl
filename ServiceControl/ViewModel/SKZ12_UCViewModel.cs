using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ServiceControl.ViewModel
{
    internal class SKZ12_UCViewModel : Observable
    {
        public Device216 device { get; set; }
        DispatcherTimer timer;


        public SKZ12_UCViewModel()
        {

        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public SKZ12_UCViewModel(MbWork work, int Slave)
        {
            KIPData kipData = new KIPData();

            device = new Device216(work, Slave);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        //--------------------------------------------------------------------------------------------
        // Событие таймера
        //--------------------------------------------------------------------------------------------
        private void Timer_Tick(object sender, EventArgs e)
        {
            device.ReadRegisters(device.ListInput);
            device.ReadRegisters(device.ListHolding);
            device.ReadRegisters(device.ListInputShort);
            device.ReadRegisters(device.ListHoldingShort);
            device.ReadRegistersBool(device.ListDiscret);
            device.ReadRegistersBool(device.ListCoil);
            device.ReadRegisters(device.ListInputDK);

        }


    }
}
