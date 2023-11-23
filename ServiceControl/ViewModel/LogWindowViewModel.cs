using Modbus.Device;
using ServiceControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace ServiceControl.ViewModel
{
    internal class LogWindowViewModel : Observable,  IDisposable
    {

        public ConsoleControl.WPF.ConsoleControl conCtrl { get; set; }

        ModbusDevice modbusDevice;
        //ModbusSlave slave;
        //public ListBox listBox;
        private bool _IsPause;
        public bool IsPause
        {
            get => _IsPause;
            set
            {
                if (_IsPause == value || modbusDevice.Transport == null) return;
                _IsPause = value;
                if (_IsPause)
                {
                    if(modbusDevice != null)
                        modbusDevice.Transport.EventLogEvent -= Transport_EventLogEvent;
                    //if(slave != null)
                    //    slave.Transport.EventLogEvent -= Transport_EventLogEvent;
                }
                else
                {
                    if (modbusDevice != null)
                        modbusDevice.Transport.EventLogEvent += Transport_EventLogEvent;
                    //if (slave != null)
                    //    slave.Transport.EventLogEvent += Transport_EventLogEvent;
                }
            }
        }   

        public ObservableCollection<string> ListString { get; set; }


        public LogWindowViewModel()
        {

        }

        public LogWindowViewModel(ModbusDevice mas)
        {
            conCtrl = new ConsoleControl.WPF.ConsoleControl();
            conCtrl.StartProcess("procNew", "7");

            ListString = new ObservableCollection<string>();
            //object lockitems = new object();
            //BindingOperations.EnableCollectionSynchronization(ListString, lockitems);

            modbusDevice = mas;
            if(modbusDevice != null)
                modbusDevice.Transport.EventLogEvent += Transport_EventLogEvent;

        }

        //public LogWindowViewModel(ModbusSlave slav)
        //{
        //    conCtrl = new ConsoleControl.WPF.ConsoleControl();
        //    conCtrl.StartProcess("procNew", "7");

        //    ListString = new ObservableCollection<string>();
        //    //object lockitems = new object();
        //    //BindingOperations.EnableCollectionSynchronization(ListString, lockitems);

        //    slave = slav;
        //    if (slave != null)
        //        slave.Transport.EventLogEvent += Transport_EventLogEvent;

        //}


        public void Dispose()
        {
            conCtrl.StopProcess();

            if (modbusDevice != null && modbusDevice.Transport != null)
                modbusDevice.Transport.EventLogEvent -= Transport_EventLogEvent;

            //if (slave != null && slave.Transport != null)
            //    slave.Transport.EventLogEvent -= Transport_EventLogEvent;
        }

        private void Transport_EventLogEvent(string header, byte[] message)
        {
            Color color = Colors.White;

            string s = header + $": {string.Join(", ", message.Select(it => $"{it:X2}"))}";

            if(header == "RX") color = Color.FromRgb(160, 255, 130);
            if (header == "TX") color = Color.FromRgb(0,255, 255);

            App.Current.Dispatcher.Invoke(() =>
            {
                ListString.Add(s);
                conCtrl.WriteOutput(s, color);
                conCtrl.WriteOutput("\n", color);

            });
        }

        public void StartLog(ModbusDevice mas)
        {
            modbusDevice = mas;
            modbusDevice.Transport.EventLogEvent += Transport_EventLogEvent;
        }

        //public void StartLog(ModbusSlave slav)
        //{
        //    slave = slav;
        //    slave.Transport.EventLogEvent += Transport_EventLogEvent;
        //}

    }
}
