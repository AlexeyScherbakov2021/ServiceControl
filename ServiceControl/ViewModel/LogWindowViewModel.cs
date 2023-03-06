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

        ModbusMaster master;
        public ListBox listBox;
        private bool _IsPause;
        public bool IsPause
        {
            get => _IsPause;
            set
            {
                if (_IsPause == value || master.Transport == null) return;
                _IsPause = value;
                if(_IsPause)
                    master.Transport.EventLogEvent -= Transport_EventLogEvent;
                else
                    master.Transport.EventLogEvent += Transport_EventLogEvent;
            }
        }   

        public ObservableCollection<string> ListString { get; set; }


        public LogWindowViewModel()
        {

        }

        public LogWindowViewModel(ModbusMaster mas)
        {
            conCtrl = new ConsoleControl.WPF.ConsoleControl();
            conCtrl.StartProcess("procNew", "7");

            ListString = new ObservableCollection<string>();
            //object lockitems = new object();
            //BindingOperations.EnableCollectionSynchronization(ListString, lockitems);

            master = mas;
            if(master != null)
                master.Transport.EventLogEvent += Transport_EventLogEvent;

        }

        public void Dispose()
        {
            conCtrl.StopProcess();

            if (master != null && master.Transport != null)
                master.Transport.EventLogEvent -= Transport_EventLogEvent;

        }

        private void Transport_EventLogEvent(string header, byte[] message)
        {
            string s = header + $": {string.Join(", ", message.Select(it => $"{it:X2}"))}";
            App.Current.Dispatcher.Invoke(() =>
            {
                ListString.Add(s);
                conCtrl.WriteOutput(s, Colors.Red);
                conCtrl.WriteOutput("\n\r", Colors.Red);

                //listBox.ScrollIntoView(s);
            });
        }

        public void StartLog(ModbusMaster mas)
        {
            master = mas;
            master.Transport.EventLogEvent += Transport_EventLogEvent;
        }
    }
}
