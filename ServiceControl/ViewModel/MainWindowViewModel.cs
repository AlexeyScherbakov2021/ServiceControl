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

namespace ServiceControl.ViewModel
{
    internal class MainWindowViewModel : Observable
    {



        #region Переменные класса

        public Device device { get; set; }
        DispatcherTimer timer;

        #endregion

        #region Экранные переменные

        private UserControl _SControl { get; set; }
        public UserControl SControl { get => _SControl; set { _SControl = value; OnPropertyChanged(nameof(SControl)); } }


        public int Slave { get; set; } = 1;

        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 8800;
        public string ComPort { get; set; }

        private bool _IsConnected = false;
        public bool IsConnected
        {
            get => _IsConnected;
            set
            {
                if (_IsConnected == value) return;
                _IsConnected = value;
                ConnectedString = _IsConnected ? "соединено" : "нет соединения";
                ConnectedColor = _IsConnected ? Brushes.Green : Brushes.Red;
            }
        }

        private string _ConnectedString = "нет соединения";
        public string ConnectedString { get => _ConnectedString; set { Set(ref _ConnectedString, value); } }

        private Brush _ConnectedColor = Brushes.Red;
        public Brush ConnectedColor { get => _ConnectedColor; set { Set(ref _ConnectedColor, value); } }

        #endregion


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public MainWindowViewModel()
        {
        }



        #region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Кнопка ОК
        //--------------------------------------------------------------------------------
        public ICommand OkCommand => new LambdaCommand(OnOkCommandExecuted, CanOkCommand);
        private bool CanOkCommand(object p) => true;
        private void OnOkCommandExecuted(object p)
        {
            device.WriteRegister(device.ListHolding[0]);
        }
        //--------------------------------------------------------------------------------
        // Команда Кнопка Соединение
        //--------------------------------------------------------------------------------
        public ICommand ConnectCommand => new LambdaCommand(OnConnectCommandExecuted, CanConnectCommand);
        private bool CanConnectCommand(object p) => !IsConnected;
        private void OnConnectCommandExecuted(object p)
        {
            MbWork work = new MbWork(HostName, Port);

            IsConnected = work.CreateConnect();

            if (!IsConnected) return;

            SControl = new SKZ12_UCView();
            SControl.DataContext = new SKZ12_UCViewModel(work, Slave);


        }

        #endregion


    }
}
