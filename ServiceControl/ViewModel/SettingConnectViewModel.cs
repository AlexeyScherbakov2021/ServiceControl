using ServiceControl.Commands;
using ServiceControl.Infrastructure;
using ServiceControl.Modbus.Devices;
using ServiceControl.View;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace ServiceControl.ViewModel
{
    internal class SettingConnectViewModel : Observable
    {
        SettingConnect setWindow;


        #region Экранные переменные

        private string _HostTCP;
        public string HostTCP { get => _HostTCP; set { Set(ref _HostTCP, value); } }

        private int _PortTCP;
        public int PortTCP { get => _PortTCP; set { Set(ref _PortTCP, value); } }

        private int _TimeOut = 500;
        public int TimeOut { get => _TimeOut; set { Set(ref _TimeOut, value); } }

        private int _Slave = 1;
        public int Slave { get => _Slave; set { Set(ref _Slave, value); } }

        private bool _IsOverRTU;
        public bool IsOverRTU { get => _IsOverRTU; set { Set(ref _IsOverRTU, value); } }

        private DeviceType _SelectedDevice;
        public DeviceType SelectedDevice { get => _SelectedDevice; set { Set(ref _SelectedDevice, value); } }

        private int _DeviceIndex;
        public int DeviceIndex { get => _DeviceIndex; set { Set(ref _DeviceIndex, value); } }

        private bool _IsSelectTCP;
        public bool IsSelectTCP { get => _IsSelectTCP; set { Set(ref _IsSelectTCP, value); } }


        public List<DeviceType> ListDeviceType { get; set; } = new List<DeviceType>()
        {
                new DeviceType() { Name = "ДЕШК.301411.131 (KS131)", deviceType = DevType.KS131},
                new DeviceType() { Name = "ДЕШК.301411.216 (KS216)", deviceType = DevType.KS216},
                new DeviceType() { Name = "ДЕШК.301411.356 (KS356)", deviceType = DevType.KS356},
                new DeviceType() { Name = "ДЕШК.301411.261 (KS261)", deviceType = DevType.KS261},
                new DeviceType() { Name = "БИ-М slave (вн.изм.)", deviceType = DevType.BI_M_Slave},
        };

        public List<string> ListCOM { get; set; } // = new List<string>()
        //{
        //    "COM1", "COM2", "COM3", "COM4"
        //};
        private string _SelectedCOM;
        public string SelectedCOM { get => _SelectedCOM; set { Set(ref _SelectedCOM, value); } }

        #endregion

        public SettingConnectViewModel()
        {
        }

        public SettingConnectViewModel(SettingConnect win)
        {
            setWindow = win;
            ListCOM = SerialPort.GetPortNames().ToList();

        }

        #region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Настройка соединения
        //--------------------------------------------------------------------------------
        public ICommand OkCommand => new LambdaCommand(OnOkCommandExecuted);
        private void OnOkCommandExecuted(object p)
        {
            setWindow.DialogResult = true;
            setWindow.Close();
        }

        public ICommand CancelCommand => new LambdaCommand(OnCancelCommandExecuted);
        private void OnCancelCommandExecuted(object p)
        {
            setWindow.Close();
        }

        #endregion

    }
}
