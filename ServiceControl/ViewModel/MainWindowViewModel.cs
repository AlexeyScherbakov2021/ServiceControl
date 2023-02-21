﻿using ServiceControl.Infrastructure;
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
using System.Threading;
using System.Globalization;

namespace ServiceControl.ViewModel
{
    internal class MainWindowViewModel : Observable
    {

        #region Переменные класса
        private CultureInfo SelectedCilture = new CultureInfo("ru-RU");

        INIManager iniFile;
        public Device device { get; set; }
        MbWork work;
        Device CurrentDevice;

        public List<DeviceType> ListDeviceType { get; set; }
        


        private bool IsSelectTCP { get; set; }

        #endregion

        #region Экранные переменные

        private int _TimeOutCOM;
        public int TimeOutCOM { get => _TimeOutCOM; set { Set(ref _TimeOutCOM, value); } }
        
        public Visibility VisibleTCP => IsSelectTCP ? Visibility.Visible : Visibility.Hidden;

        private DevType _deviceType;
        public DevType deviceType { get => _deviceType; set { Set(ref _deviceType, value); } }

        private int _Slave;
        public int Slave { get => _Slave; set { Set(ref _Slave, value); } }

        private string _HostName = "COM3";// "localhost";
        public string HostName { get => _HostName; set { Set(ref _HostName, value); } }

        private int _Port;
        public int Port { get => _Port; set { Set(ref _Port, value); } }

        private string _ComPort;
        public string ComPort { get => _ComPort; set { Set(ref _ComPort, value); } }

        //private string _StringConnect;
        //public string StringConnect { get => _StringConnect; set { Set(ref _StringConnect, value);  } }

        private UserControl _SControl { get; set; }
        public UserControl SControl { get => _SControl; set { _SControl = value; OnPropertyChanged(nameof(SControl)); } }

        private bool _IsConnected = false;
        public bool IsConnected
        {
            get => _IsConnected;
            set
            {
                if (_IsConnected == value) return;
                _IsConnected = value;
                SetStatusConnection();
            }
        }

        private string _ConnectedString; // = "не подключено";
        public string ConnectedString { get => _ConnectedString; set { Set(ref _ConnectedString, value); } }

        private Brush _ConnectedColor = Brushes.Red;
        public Brush ConnectedColor { get => _ConnectedColor; set { Set(ref _ConnectedColor, value); } }

        #endregion


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public MainWindowViewModel()
        {
            //byte[] n = new byte[4] { 0x02, 0x03, 0x11, 0x44 };
            //string s = $"RX: {string.Join(", ", n.Select(it => $"{it:X2}"))}";

            LoadFromIni();
            //FormatStringConnect();

            ListDeviceType = new List<DeviceType>()
            {
                new DeviceType() { Name = "ДЕШК.301411.131", deviceType = DevType.KS131},
                new DeviceType() { Name = "ДЕШК.301411.216", deviceType = DevType.KS216},
                new DeviceType() { Name = "ДЕШК.301411.356", deviceType = DevType.KS356},
                new DeviceType() { Name = "ДЕШК.301411.261", deviceType = DevType.KS261},
            };
        }

        //--------------------------------------------------------------------------------------------
        // установка статуса соединения
        //--------------------------------------------------------------------------------------------
        private void SetStatusConnection()
        {
            ConnectedString = _IsConnected
                ? App.Current.Resources["Connected"].ToString()
                : App.Current.Resources["NotConnected"].ToString();
            ConnectedColor = _IsConnected ? Brushes.Green : Brushes.Red;
        }



        //--------------------------------------------------------------------------------------------
        // формирование строки подключеия
        //--------------------------------------------------------------------------------------------
        //private void FormatStringConnect()
        //{
        //    StringConnect = IsSelectTCP
        //        ? $"Тип устройства: {deviceType}   Адрес: {Slave}    IP адрес: {HostName}   Порт: {Port}"
        //        : $"Тип: {deviceType}   Адрес:{Slave}   Порт: {ComPort}   Таймаут: {TimeOutCOM}";
        //}


        //--------------------------------------------------------------------------------------------
        // загрузка параметров из INI файла
        //--------------------------------------------------------------------------------------------
        private void LoadFromIni()
        {
            string nameFile = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(nameFile);
            nameFile = path + "\\" + Path.GetFileNameWithoutExtension(nameFile) + ".ini";
            iniFile = new INIManager(nameFile);

            int.TryParse(iniFile.GetPrivateString("Main", "NumberDevice"), out int resInt);
            Slave = resInt == 0 ? 1 : resInt;

            string select = iniFile.GetPrivateString("Main", "SelectConnect");
            IsSelectTCP = select == "tcp";

            int.TryParse(iniFile.GetPrivateString("Main", "Device"), out resInt);
            deviceType = (DevType)resInt;

            ComPort = iniFile.GetPrivateString("COM", "COMPort");
            int.TryParse(iniFile.GetPrivateString("COM", "Timeout"), out resInt);
            TimeOutCOM = resInt == 0 ? 500 : resInt;

            HostName = iniFile.GetPrivateString("TCP", "Host");

            int.TryParse(iniFile.GetPrivateString("TCP", "Port"), out resInt);
            Port = resInt;


            string lang = iniFile.GetPrivateString("Main", "Lang");
            if (string.IsNullOrEmpty(lang)) 
                lang = SelectedCilture.Name;

            ChangeLanguage(lang);
            OnPropertyChanged(nameof(VisibleTCP));
        }

        //--------------------------------------------------------------------------------------------
        // изменение текущего язывка
        //--------------------------------------------------------------------------------------------
        private void ChangeLanguage(string cu)
        {
            var cult = new CultureInfo(cu);

            if (Equals(cult.Name, SelectedCilture.Name)) return;

            var dict = new ResourceDictionary() { Source = new Uri($"Resources/lang.{cult.Name}.xaml", UriKind.Relative) };
            if (dict == null) return;

            SelectedCilture = cult;

            ResourceDictionary oldDict = Application.Current.Resources.MergedDictionaries
                .Where(it => it.Source != null && it.Source.OriginalString.Contains("lang"))
                .Select(s => s)
                .First();

            Application.Current.Resources.MergedDictionaries.Remove(oldDict);
            Application.Current.Resources.MergedDictionaries.Add(dict);
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

                OnPropertyChanged(nameof(VisibleTCP));

                //FormatStringConnect();
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Кнопка Соединение
        //--------------------------------------------------------------------------------
        public ICommand ConnectCommand => new LambdaCommand(OnConnectCommandExecuted, CanConnectCommand);
        private bool CanConnectCommand(object p) => !IsConnected;
        private void OnConnectCommandExecuted(object p)
        {
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
                    var vm216 = new KS216_UCViewModel(this, work, Slave);
                    SControl.DataContext = vm216;
                    CurrentDevice = vm216.device;
                    CurrentDevice.ChangeLangRegister();
                    break;
                case DevType.KS356:
                    SControl = new KS356_UCView();
                    var vm356 = new KS356_UCViewModel(this, work, Slave);
                    SControl.DataContext = vm356;
                    CurrentDevice = vm356.device;
                    CurrentDevice.ChangeLangRegister();
                    break;
                case DevType.KS261:
                    SControl = new KS261_UCView();
                    var vm261 = new KS261_UCViewModel(this, work, Slave);
                    SControl.DataContext = vm261;
                    CurrentDevice = vm261.device;
                    CurrentDevice.ChangeLangRegister();
                    break;
            }



        }

        //--------------------------------------------------------------------------------
        // Команда Кнопка Разъединить
        //--------------------------------------------------------------------------------
        public ICommand DisconnectCommand => new LambdaCommand(OnDisconnectCommandExecuted, CanDisconnectCommand);
        private bool CanDisconnectCommand(object p) => IsConnected;
        private void OnDisconnectCommandExecuted(object p)
        {
            CurrentDevice.Stop();
            Thread.Sleep(1000);
            work.Disconnect();
            SControl = null;
            work = null;
            IsConnected = false;
        }

        //--------------------------------------------------------------------------------
        // Команда Сменить язык
        //--------------------------------------------------------------------------------
        public ICommand LangCommand => new LambdaCommand(OnLangCommandExecuted, CanLangCommand);
        private bool CanLangCommand(object p) => true;
        private void OnLangCommandExecuted(object p)
        {
            ChangeLanguage(p.ToString());
            iniFile.WritePrivateString("Main", "Lang", SelectedCilture.Name);
            CurrentDevice?.ChangeLangRegister();
            SetStatusConnection();
        }

        #endregion

    }
}
