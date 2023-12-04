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
                new DeviceType() { Name = "KS 131", deviceType = DevType.KS131, isSlave = false},
                new DeviceType() { Name = "KS 216", deviceType = DevType.KS216, isSlave = false},
                new DeviceType() { Name = "KS 356", deviceType = DevType.KS356},
                new DeviceType() { Name = "KS 261", deviceType = DevType.KS261, isSlave = false},
                new DeviceType() { Name = "БИ-М master", deviceType = DevType.BI_M_Master, isSlave = true},
                new DeviceType() { Name = "БИ-М slave (вн.изм.)", deviceType = DevType.BI_M_Slave, isSlave = false},
                new DeviceType() { Name = "КИП-М5", deviceType = DevType.KIP_M5, isSlave = false},
                new DeviceType() { Name = "КИП-М5 (мод.расш.)", deviceType = DevType.KIP_M5Ext, isSlave = false},
                new DeviceType() { Name = "КССМ(Н)", deviceType = DevType.KSSM, isSlave = false},
                new DeviceType() { Name = "КИП-LC", deviceType = DevType.KIP_LC, isSlave = true},
                new DeviceType() { Name = "КИП-М(УДЗ)", deviceType = DevType.KIP_UDZ, isSlave = true},
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
