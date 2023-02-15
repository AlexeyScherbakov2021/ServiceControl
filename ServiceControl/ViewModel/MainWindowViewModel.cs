using ServiceControl.Infrastructure;
using ServiceControl.Modbus.Registers;
using ServiceControl.Modbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceControl.Commands;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Controls;
using ServiceControl.View;
using ServiceControl.Modbus.Devices;
using System.IO;
using System.Windows;
using System.Reflection;

namespace ServiceControl.ViewModel
{
    internal class MainWindowViewModel : Observable
    {

        #region Переменные класса

        INIManager iniFile;
        public Device device { get; set; }
        //DispatcherTimer timer;

        public List<DeviceType> ListDeviceType { get; set; }
        private DevType deviceType { get; set; }
        private int TimeOutCOM { get; set; }
        private bool IsSelectTCP { get; set; }


        #endregion

        #region Экранные переменные

        private string _StringConnect;
        public string StringConnect { get => _StringConnect; set { Set(ref _StringConnect, value);  } }

        private UserControl _SControl { get; set; }
        public UserControl SControl { get => _SControl; set { _SControl = value; OnPropertyChanged(nameof(SControl)); } }


        public int Slave { get; set; } = 1;

        public string HostName { get; set; } = "COM3";// "localhost";
        public int Port { get; set; } = 0;
        public string ComPort { get; set; }

        private bool _IsConnected = false;
        public bool IsConnected
        {
            get => _IsConnected;
            set
            {
                if (_IsConnected == value) return;
                _IsConnected = value;
                ConnectedString = _IsConnected ? "подключено" : "не подключено";
                ConnectedColor = _IsConnected ? Brushes.Green : Brushes.Red;
            }
        }

        private string _ConnectedString = "не подключено";
        public string ConnectedString { get => _ConnectedString; set { Set(ref _ConnectedString, value); } }

        private Brush _ConnectedColor = Brushes.Red;
        public Brush ConnectedColor { get => _ConnectedColor; set { Set(ref _ConnectedColor, value); } }

        #endregion


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public MainWindowViewModel()
        {
            byte[] n = new byte[4] { 0x02, 0x03, 0x11, 0x44 };

            //var s2 = n.Select(it => $"{it:X}").ToList();
            string s = $"RX: {string.Join(", ", n.Select(it => $"{it:X2}"))}";

            LoadFromIni();
            FormatStringConnect();

            ListDeviceType = new List<DeviceType>()
            {
                new DeviceType() { Name = "ДЕШК.301411.131", deviceType = DevType.KS131},
                new DeviceType() { Name = "ДЕШК.301411.216", deviceType = DevType.KS216},
                new DeviceType() { Name = "ДЕШК.301411.356", deviceType = DevType.KS356},
                new DeviceType() { Name = "ДЕШК.301411.261", deviceType = DevType.KS261},
            };
        }


        private void FormatStringConnect()
        {
            StringConnect = IsSelectTCP
                ? $"Тип устройства: {deviceType}   Адрес: {Slave}    IP адрес: {HostName}   Порт: {Port}"
                : $"Тип: {deviceType}   Адрес:{Slave}   Порт: {ComPort}   Таймаут: {TimeOutCOM}";
        }

        private void LoadFromIni()
        {
            string nameFile = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(nameFile);
            nameFile = path + "\\" + Path.GetFileNameWithoutExtension(nameFile) + ".ini";
            iniFile = new INIManager(nameFile);

            int.TryParse(iniFile.GetPrivateString("Main", "NumberDevice"), out int resInt);
            Slave = resInt;

            string select = iniFile.GetPrivateString("Main", "SelectConnect");
            IsSelectTCP = select == "tcp";

            int.TryParse(iniFile.GetPrivateString("Main", "Device"), out resInt);
            deviceType = (DevType)resInt;

            ComPort = iniFile.GetPrivateString("COM", "COMPort");
            int.TryParse(iniFile.GetPrivateString("COM", "Timeout"), out resInt);
            TimeOutCOM = resInt;

            HostName = iniFile.GetPrivateString("TCP", "Host");

            int.TryParse(iniFile.GetPrivateString("TCP", "Port"), out resInt);
            Port = resInt;

        }



        #region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Настройка соединения
        //--------------------------------------------------------------------------------
        public ICommand SetConnCommand => new LambdaCommand(OnSetConnCommandExecuted, CanSetConnCommand);
        private bool CanSetConnCommand(object p) => true;
        private void OnSetConnCommandExecuted(object p)
        {
            SettingConnect win = new SettingConnect();
            SettingConnectViewModel vm = new SettingConnectViewModel(win);
            win.DataContext = vm;

            vm.Slave = Slave;
            vm.IsSelectTCP = IsSelectTCP;
            vm.DeviceIndex = (int)deviceType;
            vm.SelectedCOM = ComPort;
            vm.TimeOut= TimeOutCOM;
            vm.HostTCP = HostName;
            vm.PortTCP = Port;

            if (win.ShowDialog() == true)
            {
                HostName = vm.HostTCP;
                Port = vm.PortTCP;
                ComPort = vm.SelectedCOM;
                TimeOutCOM = vm.TimeOut;
                Slave = vm.Slave;
                IsSelectTCP = vm.IsSelectTCP;
                deviceType = (DevType)vm.DeviceIndex;

                iniFile.WritePrivateString("Main", "NumberDevice", Slave.ToString());
                string SelectConnect = vm.IsSelectTCP ? "tcp" : "com";
                iniFile.WritePrivateString("Main", "SelectConnect", SelectConnect);
                iniFile.WritePrivateString("Main", "Device", vm.DeviceIndex.ToString());

                iniFile.WritePrivateString("COM", "COMPort", ComPort);
                iniFile.WritePrivateString("COM", "Timeout", TimeOutCOM.ToString());

                iniFile.WritePrivateString("TCP", "Host", HostName);
                iniFile.WritePrivateString("TCP", "Port", Port.ToString());

                FormatStringConnect();
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Кнопка Соединение
        //--------------------------------------------------------------------------------
        public ICommand ConnectCommand => new LambdaCommand(OnConnectCommandExecuted, CanConnectCommand);
        private bool CanConnectCommand(object p) => !IsConnected;
        private void OnConnectCommandExecuted(object p)
        {
            MbWork work;

            if (IsSelectTCP)
                work = new MbWork(HostName, Port, Protocol.TCP);
            else
                work = new MbWork(ComPort, TimeOutCOM, Protocol.COM);

            IsConnected = work.CreateConnect();

            if (!IsConnected) return;

            switch(deviceType)
            {
                case DevType.KS131:
                    break;
                case DevType.KS216:
                    SControl = new KS216_UCView();
                    SControl.DataContext = new KS216_UCViewModel(work, Slave);
                    break;
                case DevType.KS356:
                    SControl = new KS356_UCView();
                    SControl.DataContext = new KS356_UCViewModel(work, Slave);

                    break;
                case DevType.KS261:
                    SControl = new KS261_UCView();
                    SControl.DataContext = new KS261_UCViewModel(work, Slave);
                    break;
            }



        }

        #endregion


    }
}
