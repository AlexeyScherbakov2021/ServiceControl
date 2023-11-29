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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ServiceControl.ViewModel
{
    internal class KIPLC_UCViewModel : Observable //, IDataErrorInfo
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        //public class TwoRegister
        //{
        //    public Register Register1 { get; set; }
        //    public Register Register2 { get; set; }
        //}

        public DeviceKIPLC device { get; set; }

        public List<Register> ListInput { get; set; }
        //public List<Register> ListInput2 { get; set; }

        public List<Register> ListRealTime { get; set; }


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KIPLC_UCViewModel()
        {
        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KIPLC_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {

            device = new DeviceKIPLC(mainViewModel, work, Slave);
            //device.EndRead += OnReadFinish;
            //device.EndStartRead += OnEndStartRead;
            //device.Start();

            ListRealTime = new List<Register>
            {
                device.RealTime
            };
            // добавление в список входных параметров
            ListInput = new List<Register>()
            {
                device.NumberPacket, device.VoltPower,device.SummPot1,
                device.PolPot1, device.CurrPot1, device.VoltNaveden1,  device.FreqVoltNaveden1,
                device.KoefSummPot1, device.KoefPolPot1,
                device.KoCurrPot1, device.AddressBIM,
                device.AddressBIMChange, device.SetID_BI,
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
        //private void OnReadFinish(object sender, EventArgs e)
        //{

        //}



        #region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Отправить значение
        //--------------------------------------------------------------------------------
        //public ICommand WriteValueCommand => new LambdaCommand(OnWriteValueCommandExecuted, CanWriteValueCommand);
        //private bool CanWriteValueCommand(object p) => device != null;
        //private void OnWriteValueCommandExecuted(object p)
        //{
        //    if (p is Register reg)
        //    {
        //        try
        //        {
        //            device.WriteRegister(reg);
        //            if (reg.GetType() == typeof(RegisterFloat))
        //            {
        //                //(reg as RegisterFloat).Value = 0;
        //            }
        //        }
        //        catch (TimeoutException)
        //        {

        //        }
        //    }
        //}


#if !CLIENT
        //--------------------------------------------------------------------------------
        // Команда Установить текущее время
        //--------------------------------------------------------------------------------
        //public ICommand WriteTimeCommand => new LambdaCommand(OnWriteTimeModeCommandExecuted, CanWriteTimeModeCommand);
        //private bool CanWriteTimeModeCommand(object p) => device != null ;
        //private void OnWriteTimeModeCommandExecuted(object p)
        //{
        //    device.RealTime.RealTimeValue = DateTime.Now;
        //    device.WriteRegister(device.RealTime);
        //}


#endif
        //--------------------------------------------------------------------------------
        // Команда Записать в регистр Bool
        //--------------------------------------------------------------------------------
        //public ICommand OnOffCommand => new LambdaCommand(OnOnOffCommandExecuted);
        //private void WriteValueBoolCommand(object p)
        //{
        //    RegisterBool reg = p as RegisterBool;
        //    device.WriteRegister(reg);
        //}


        #endregion



    }
}
