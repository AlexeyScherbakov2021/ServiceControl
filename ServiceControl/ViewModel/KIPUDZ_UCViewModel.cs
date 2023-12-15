using ServiceControl.Commands;
using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using ServiceControl.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace ServiceControl.ViewModel
{
    internal class KIPUDZ_UCViewModel : Observable //, IDataErrorInfo
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private bool _isOpen;
        public bool isOpen { get => _isOpen; set { Set(ref _isOpen, value); } }

        private bool _isSumPot; 
        public bool isSumPot { get => _isSumPot; set { Set(ref _isSumPot, value); } }

        private bool _isPolPot;
        public bool isPolPot { get => _isPolPot; set { Set(ref _isPolPot, value); } }

        private bool _isCurrPol; 
        public bool isCurrPol { get => _isCurrPol; set { Set(ref _isCurrPol, value); } }

        private bool _isVoltageTR; 
        public bool isVoltageTR { get => _isVoltageTR; set { Set(ref _isVoltageTR, value); } }

        private bool _isCurrDrenazh;
        public bool isCurrDrenazh { get => _isCurrDrenazh; set { Set(ref _isCurrDrenazh, value); } }

        private bool _isResistTR; 
        public bool isResistTR { get => _isResistTR; set { Set(ref _isResistTR, value); } }

        private bool _isVoltage;
        public bool isVoltage { get => _isVoltage; set { Set(ref _isVoltage, value); } }

        private bool _isTemperatur; 
        public bool isTemperatur { get => _isTemperatur; set { Set(ref _isTemperatur, value); } }


        public bool isOpenSet { get; set; }

        public bool isSumPotSet { get; set; }

        public bool isPolPotSet { get; set; }

        public bool isCurrPolSet { get; set; }

        public bool isVoltageTRSet { get; set; }

        public bool isCurrDrenazhSet { get; set; }

        public bool isResistTRSet { get; set; }

        public bool isVoltageSet { get; set; }

        public bool isTemperaturSet { get; set; }


        //public class TwoRegister
        //{
        //    public Register Register1 { get; set; }
        //    public Register Register2 { get; set; }
        //}

        //private int CountTimerSetMode;

        //private Visibility _IsAvarModeVisible = Visibility.Hidden;
        //public Visibility IsAvarModeVisible { get => _IsAvarModeVisible; set { Set(ref _IsAvarModeVisible, value); } }

        public DeviceKIPUDZ device { get; set; }

        public List<Register> ListInput { get; set; }
        public List<TwoRegister> ListInputMM { get; set; }
        public List<Register> ListHolding { get; set; }

        public List<Register> ListRealTime { get; set; }


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KIPUDZ_UCViewModel()
        {
            
        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KIPUDZ_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {
            device = new DeviceKIPUDZ(mainViewModel, work, Slave);
            device.EndRead += OnReadFinish;

            device.RealTime.RealTimeValue = DateTime.Now;

            ListRealTime = new List<Register>
            {
                device.RealTime
            };
            // добавление в список входных параметров
            ListInput = new List<Register>()
            {
                device.WakeUp
            };

            ListHolding = new List<Register>()
            {
                device.Number, device.Voltage,
                device.SummPot, device.PolPot, device.VoltageInduct,  device.FreqInduct,
                device.CurrPol, device.ResistTR,
                device.Temper1, device.Temper2, device.SpeedKorr, device.DeepKorr,
                device.VoltageTR, device.CurrTR, device.IndicBITLeft,
                device.IndicBITRight
            };

            ListInputMM = new List<TwoRegister>()
            {
                new TwoRegister() { Register1 = device.PowerMin, Register2 = device.PowerMax },
                new TwoRegister() { Register1 = device.SummPotMin, Register2 = device.SummPotMax },
                new TwoRegister() { Register1 = device.PolPotMin, Register2 = device.PolPotMax },
                new TwoRegister() { Register1 = device.CurrPotMin, Register2 = device.CurrPotMax },
                new TwoRegister() { Register1 = device.VoltTRMin, Register2 = device.VoltTRMax },
                new TwoRegister() { Register1 = device.CurrTRMin, Register2 = device.CurrTRMax },
                new TwoRegister() { Register1 = device.ResistTRMin, Register2 = device.ResistTRMax },
                new TwoRegister() { Register1 = device.TemperMin, Register2 = device.TemperMax },
            };

            device.Start();

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }


        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            device.RealTime.RealTimeValue = DateTime.Now;
            device.SetRegister(device.RealTime);
            _timer.Start();
        }


        //--------------------------------------------------------------------------------
        // событие после чтения всех регистров при старте
        //--------------------------------------------------------------------------------
        //private void OnEndStartRead(object sender, EventArgs e)
        //{

        //    CommandManager.InvalidateRequerySuggested();
        //    //LastSetMode = device.SetMode.Value;
        //}

        //--------------------------------------------------------------------------------
        // событие после чтения всех регистров
        //--------------------------------------------------------------------------------
        private void OnReadFinish(object sender, EventArgs e)
        {
            isOpen = (device.Flags.Value &        0b0000000001) > 0;
            isSumPot = (device.Flags.Value &      0b0000000100) > 0;
            isPolPot = (device.Flags.Value &      0b0000001000) > 0;
            isCurrPol = (device.Flags.Value &     0b0000010000) > 0;
            isVoltageTR = (device.Flags.Value &   0b0000100000) > 0;
            isCurrDrenazh = (device.Flags.Value & 0b0001000000) > 0;
            isResistTR = (device.Flags.Value &    0b0010000000) > 0;
            isVoltage = (device.Flags.Value &     0b0100000000) > 0;
            isTemperatur = (device.Flags.Value &  0b1000000000) > 0;
        }



#region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Отправить значение
        //--------------------------------------------------------------------------------
        public ICommand WriteValueCommand => new LambdaCommand(OnWriteValueCommandExecuted, CanWriteValueCommand);
        private bool CanWriteValueCommand(object p) => device != null;
        private void OnWriteValueCommandExecuted(object p)
        {
            device.FlagsAlarm.Value =
                  (isOpenSet ?          0b0000000001 : 0)
                | (isSumPotSet ?        0b0000000100 : 0)
                | (isPolPotSet ?        0b0000001000 : 0)
                | (isCurrPolSet ?       0b0000010000 : 0)
                | (isVoltageTRSet ?     0b0000100000 : 0)
                | (isCurrDrenazhSet ?   0b0001000000 : 0)
                | (isResistTRSet ?      0b0010000000 : 0)
                | (isVoltageSet ?       0b0100000000 : 0)
                | (isTemperaturSet ?    0b1000000000 : 0);

            device.SetRegister(device.FlagsAlarm);

            foreach (var reg in ListInput)
                device.SetRegister(reg);
        }

        //--------------------------------------------------------------------------------
        // Команда Отправить значение
        //--------------------------------------------------------------------------------
        public ICommand WriteConfigCommand => new LambdaCommand(OnWriteConfigCommandExecuted, CanWriteConfigCommand);
        private bool CanWriteConfigCommand(object p) => device != null;
        private void OnWriteConfigCommandExecuted(object p)
        {
            ConfigKIPUDZ_Window win = new ConfigKIPUDZ_Window();

            win.ShowDialog();

        }
#endregion

    }
}
