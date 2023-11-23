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
using System.Threading;
using System.Globalization;
using System.Diagnostics;
using ConsoleControl.WPF;

namespace ServiceControl.ViewModel
{
    internal class MainWindowViewModel : Observable
    {

        #region Переменные класса
        private CultureInfo SelectedCilture = new CultureInfo("ru-RU");

        LogWindow winLog;
        INIManager iniFile;
        public Device device { get; set; }
        MbWork work;
        Device CurrentDevice;

        //public List<DeviceType> ListDeviceType { get; set; }
        


        private bool IsSelectTCP { get; set; }

        #endregion

        #region Экранные переменные

        private int _TimeOutCOM;
        public int TimeOutCOM { get => _TimeOutCOM; set { Set(ref _TimeOutCOM, value); } }
        
        public Visibility VisibleTCP => IsSelectTCP ? Visibility.Visible : Visibility.Hidden;

        public DeviceType _SelectDevice;
        public DeviceType SelectDevice { get => _SelectDevice; set { Set(ref _SelectDevice, value); } }

        private DevType _deviceType;
        public DevType deviceType { get => _deviceType; set { Set(ref _deviceType, value); } }

        private int _Slave;
        public int Slave { get => _Slave; set { Set(ref _Slave, value); } }

        private string _HostName = "COM3";// "localhost";
        public string HostName { get => _HostName; set { Set(ref _HostName, value); } }

        private int _Port;
        public int Port { get => _Port; set { Set(ref _Port, value); } }

        private bool IsRTU;

        private string _ComPort;
        public string ComPort { get => _ComPort; set { Set(ref _ComPort, value); } }

        //private string _StringConnect;
        //public string StringConnect { get => _StringConnect; set { Set(ref _StringConnect, value);  } }

        private UserControl _SControl { get; set; }
        public UserControl SControl { get => _SControl; set { _SControl = value; OnPropertyChanged(nameof(SControl)); } }

        private bool IsTimeOutStatus = false;
        //public bool IsTimeOutStatus
        //{
        //    get => _IsTimeOutStatus;
        //    set => Set(ref _IsTimeOutStatus, value);
        //}


        private bool IsConnected = false;
        //public bool IsConnected
        //{
        //    get => _IsConnected;
        //    set => Set(ref _IsConnected, value);
        //}

        private bool isWaiting = true;

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
            LoadFromIni();
            //ListDeviceType = new List<DeviceType>()
            //{
            //    new DeviceType() { Name = "ДЕШК.301411.131", deviceType = DevType.KS131},
            //    new DeviceType() { Name = "ДЕШК.301411.216", deviceType = DevType.KS216},
            //    new DeviceType() { Name = "ДЕШК.301411.356", deviceType = DevType.KS356},
            //    new DeviceType() { Name = "ДЕШК.301411.261", deviceType = DevType.KS261},
            //};
        }

        //--------------------------------------------------------------------------------------------
        // установка статуса соединения
        //--------------------------------------------------------------------------------------------
        public void SetStatusConnection(StatusConnect status)
        {
            switch(status)
            {
                case StatusConnect.Connected:
                    IsConnected = true;
                    isWaiting = false;
                    break;

                case StatusConnect.Disconnected:
                    IsTimeOutStatus= false;
                    IsConnected = false;
                    break;

                case StatusConnect.NotAnswer:
                    IsTimeOutStatus = true;
                    break;

                case StatusConnect.Answer:
                    IsTimeOutStatus = false;
                    break;

                case StatusConnect.Waiting:
                    isWaiting = true;
                    //IsConnected = false;
                    break;

            }

            SetStringConnection();
        }


        //--------------------------------------------------------------------------------------------
        // установка строки соединения
        //--------------------------------------------------------------------------------------------
        public void SetStringConnection()
        {
            if (IsTimeOutStatus)
            {
                ConnectedString = App.Current.Resources["NotAnswer"].ToString();
                ConnectedColor = Brushes.Red;
            }
            else if (isWaiting)
            {
                ConnectedString = "ожидание";
                ConnectedColor = Brushes.Red;
            }
            else if (IsConnected)
            {
                ConnectedString = App.Current.Resources["Connected"].ToString();
                ConnectedColor = Brushes.Green;
            }
            else
            {
                ConnectedString = App.Current.Resources["NotConnected"].ToString();
                ConnectedColor = Brushes.Red;
            }
        }

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

            bool.TryParse(iniFile.GetPrivateString("TCP", "RTU"), out IsRTU);

            var setting = new SettingConnectViewModel();
            SelectDevice = setting.ListDeviceType.FirstOrDefault(it => it.deviceType == deviceType);


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
            vm.IsOverRTU = IsRTU;

            if (win.ShowDialog() == true)
            {
                HostName = vm.HostTCP;
                Port = vm.PortTCP;
                IsRTU = vm.IsOverRTU;
                ComPort = vm.SelectedCOM;
                TimeOutCOM = vm.TimeOut;
                Slave = vm.Slave;
                IsSelectTCP = vm.IsSelectTCP;
                deviceType = vm.SelectedDevice.deviceType; /*(DevType)vm.DeviceIndex;*/
                SelectDevice = vm.SelectedDevice;

                iniFile.WritePrivateString("Main", "NumberDevice", Slave.ToString());
                string SelectConnect = vm.IsSelectTCP ? "tcp" : "com";
                iniFile.WritePrivateString("Main", "SelectConnect", SelectConnect);
                iniFile.WritePrivateString("Main", "Device", vm.DeviceIndex.ToString());

                iniFile.WritePrivateString("COM", "COMPort", ComPort);
                iniFile.WritePrivateString("COM", "Timeout", TimeOutCOM.ToString());

                iniFile.WritePrivateString("TCP", "Host", HostName);
                iniFile.WritePrivateString("TCP", "Port", Port.ToString());
                iniFile.WritePrivateString("TCP", "RTU", IsRTU.ToString());

                OnPropertyChanged(nameof(VisibleTCP));

                //FormatStringConnect();
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Кнопка Соединение
        //--------------------------------------------------------------------------------
        public ICommand ConnectCommand => new LambdaCommand(OnConnectCommandExecuted, CanConnectCommand);
        private bool CanConnectCommand(object p) => !IsConnected && SelectDevice != null;
        private void OnConnectCommandExecuted(object p)
        {
            bool res;

            if (IsSelectTCP)
                work = new MbWork(HostName, Port, Protocol.TCP, IsRTU);
            else
                work = new MbWork(ComPort, TimeOutCOM, Protocol.COM);


            if(SelectDevice.isSlave)
                res = work.CreateConnectSlave();
            else
                res = work.CreateConnect();

            if (res)
                SetStatusConnection(StatusConnect.Connected);
            else
            {
                SetStatusConnection(StatusConnect.Disconnected);
                return;
            }

            //if (!IsConnected) return;

            switch(SelectDevice.deviceType)
            {
                case DevType.KS131:
                    SControl = new KS131_UCView();
                    var vm131 = new KS131_UCViewModel(this, work, Slave);
                    SControl.DataContext = vm131;
                    CurrentDevice = vm131.device;
                    if (winLog != null)
                        (winLog.DataContext as LogWindowViewModel).StartLog(work.master);
                    CurrentDevice.ChangeLangRegister();
                    break;

                case DevType.KS216:
                    SControl = new KS216_UCView();
                    var vm216 = new KS216_UCViewModel(this, work, Slave);
                    SControl.DataContext = vm216;
                    CurrentDevice = vm216.device;
                    if(winLog != null)
                        (winLog.DataContext as LogWindowViewModel).StartLog(work.master);
                    CurrentDevice.ChangeLangRegister();
                    break;

                case DevType.KS356:
                    SControl = new KS356_UCView();
                    var vm356 = new KS356_UCViewModel(this, work, Slave);
                    SControl.DataContext = vm356;
                    CurrentDevice = vm356.device;
                    if (winLog != null)
                        (winLog.DataContext as LogWindowViewModel).StartLog(work.master);
                    CurrentDevice.ChangeLangRegister();
                    break;

                case DevType.KS261:
                    SControl = new KS261_UCView();
                    var vm261 = new KS261_UCViewModel(this, work, Slave);
                    SControl.DataContext = vm261;
                    CurrentDevice = vm261.device;
                    if (winLog != null)
                        (winLog.DataContext as LogWindowViewModel).StartLog(work.master);
                    CurrentDevice.ChangeLangRegister();
                    break;

                case DevType.BI_M_Slave:
                    SControl = new BI_M_UCView();
                    var vmBIM = new BI_M_UCViewModel(this, work, Slave);
                    SControl.DataContext = vmBIM;
                    CurrentDevice = vmBIM.device;
                    if (winLog != null)
                        (winLog.DataContext as LogWindowViewModel).StartLog(work.master);
                    CurrentDevice.ChangeLangRegister();
                    break;

                case DevType.KIP_M5:
                    SControl = new KIPM5_UCView();
                    var vmKIPM5 = new KIPM5_UCViewModel(this, work, Slave);
                    SControl.DataContext = vmKIPM5;
                    CurrentDevice = vmKIPM5.device;
                    if (winLog != null)
                        (winLog.DataContext as LogWindowViewModel).StartLog(work.master);
                    CurrentDevice.ChangeLangRegister();
                    break;

                case DevType.KSSM:
                    SControl = new KSSM_UCView();
                    var vmKSSM = new KSSM_UCViewModel(this, work, Slave);
                    SControl.DataContext = vmKSSM;
                    CurrentDevice = vmKSSM.device;
                    if (winLog != null)
                        (winLog.DataContext as LogWindowViewModel).StartLog(work.master);
                    CurrentDevice.ChangeLangRegister();
                    break;

                case DevType.KIP_LC:
                    SControl = new KIPLC_UCView();
                    var vmKIPLC = new KIPLC_UCViewModel(this, work, Slave);
                    SControl.DataContext = vmKIPLC;
                    CurrentDevice = vmKIPLC.device;
                    if (winLog != null)
                        (winLog.DataContext as LogWindowViewModel).StartLog(work.slave);
                    CurrentDevice.ChangeLangRegister();
                    break;

                case DevType.KIP_UDZ:
                    SControl = new KIPUDZ_UCView();
                    var vmKIPUDZ = new KIPUDZ_UCViewModel(this, work, Slave);
                    SControl.DataContext = vmKIPUDZ;
                    CurrentDevice = vmKIPUDZ.device;
                    if (winLog != null)
                        (winLog.DataContext as LogWindowViewModel).StartLog(work.slave);
                    CurrentDevice.ChangeLangRegister();
                    break;
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Кнопка Разъединить
        //--------------------------------------------------------------------------------
        public ICommand DisconnectCommand => new LambdaCommand(OnDisconnectCommandExecuted, CanDisconnectCommand);
        private bool CanDisconnectCommand(object p) => IsConnected || IsTimeOutStatus;
        private void OnDisconnectCommandExecuted(object p)
        {
            if(CurrentDevice is Device)
                (CurrentDevice as Device).Stop();
            Thread.Sleep(1000);
            work?.Disconnect();
            SControl = null;
            work = null;
            SetStatusConnection(StatusConnect.Disconnected);
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
            SetStringConnection();
        }

        //--------------------------------------------------------------------------------
        // Команда Журнал
        //--------------------------------------------------------------------------------
        public ICommand LogCommand => new LambdaCommand(OnLogCommandExecuted, CanLogCommand);
        private bool CanLogCommand(object p) => winLog == null /* && work != null*/;
        private void OnLogCommandExecuted(object p)
        {
            LogWindowViewModel vm = new LogWindowViewModel(work?.master);
            winLog = new LogWindow();
            if(work?.master != null)
                vm = new LogWindowViewModel(work?.master);
            if (work?.slave != null)
                vm = new LogWindowViewModel(work?.slave);
            //vm.listBox = winLog.lb;
            winLog.DataContext = vm;
            winLog.Closed += (sender, e) => { winLog = null; vm.Dispose(); };
            winLog.Show();
        }

        //--------------------------------------------------------------------------------
        // Команда О программе
        //--------------------------------------------------------------------------------
        public ICommand AboutCommand => new LambdaCommand(OnAboutCommandExecuted, CanAboutCommand);
        private bool CanAboutCommand(object p) => winLog == null;
        private void OnAboutCommandExecuted(object p)
        {
            AboutWindow win = new AboutWindow();
            win.NameVersion.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            win.Owner = App.Current.MainWindow;
            win.ShowDialog();
        }

        //--------------------------------------------------------------------------------
        // Команда Руководство
        //--------------------------------------------------------------------------------
        public ICommand ManualCommand => new LambdaCommand(OnManualCommandExecuted, CanManualCommand);
        private bool CanManualCommand(object p) => winLog == null;
        private void OnManualCommandExecuted(object p)
        {
            ManualWindow win = new ManualWindow();
            win.ShowDialog();
        }

        #endregion

    }
}
