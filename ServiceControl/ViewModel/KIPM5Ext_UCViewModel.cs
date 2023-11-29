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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ServiceControl.ViewModel
{
    internal class KIPM5Ext_UCViewModel : Observable //, IDataErrorInfo
    {

        public class ThreeRegister
        {
            public Register Register1 { get; set; }
            public Register Register2 { get; set; }
            public Register Register3 { get; set; }
        }

        //private int CountTimerSetMode;

        //private Visibility _IsAvarModeVisible = Visibility.Hidden;
        //public Visibility IsAvarModeVisible { get => _IsAvarModeVisible; set { Set(ref _IsAvarModeVisible, value); } }

        public DeviceKIPM5Ext device { get; set; }

        public List<Register> ListHolding { get; set; }
        public List<ThreeRegister> ListHolding2 { get; set; }

        //public List<RegisterBool> ListStatus { get; set; }
        //public RegisterBool isDoor { get; set; }
        //public RegisterBool isDK1 { get; set; }
        //public RegisterBool isDK2 { get; set; }
        //public RegisterBool isDK3 { get; set; }
        //public RegisterBool isUSIKP { get; set; }


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KIPM5Ext_UCViewModel()
        {
        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KIPM5Ext_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {

            device = new DeviceKIPM5Ext(mainViewModel, work, Slave);
            //device.EndRead += OnReadFinish;
            device.EndStartRead += OnEndStartRead; 

            // добавление в список входных параметров
            ListHolding = new List<Register>()
            {
                device.Address, device.Flags, device.CurrOut, device.VoltOut, 
            };

            // добавление в список целых регистров управления
            ListHolding2 = new List<ThreeRegister>()
            {
                new ThreeRegister() { Register1 = device.SummPotRMS, Register2 = device.SummPotRMS2, Register3 = device.SummPotRMS3 },
                new ThreeRegister() { Register1 = device.SummPot, Register2 = device.SummPot2, Register3 = device.SummPot3 },
                new ThreeRegister() { Register1 = device.PolPot, Register2 = device.PolPot2, Register3 = device.PolPot3 },
                new ThreeRegister() { Register1 = device.CurrPot, Register2 = device.CurrPot2, Register3 = device.CurrPot3 },
                new ThreeRegister() { Register1 = device.VoltNaveden, Register2 = device.VoltNaveden2, Register3 = device.VoltNaveden3 },
                new ThreeRegister() { Register1 = device.FreqVoltNaveden, Register2 = device.FreqVoltNaveden2, Register3 = device.FreqVoltNaveden3 },
            };

            device.Start();

        }


        //--------------------------------------------------------------------------------
        // событие после чтения всех регистров при старте
        //--------------------------------------------------------------------------------
        private void OnEndStartRead(object sender, EventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
            //LastSetMode = device.SetMode.Value;
        }

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
        public ICommand WriteValueCommand => new LambdaCommand(OnWriteValueCommandExecuted, CanWriteValueCommand);
        private bool CanWriteValueCommand(object p) => device != null;
        private void OnWriteValueCommandExecuted(object p)
        {
            //if (p is Register reg)
            //{
            //    try
            //    {
            //        device.WriteRegister(reg);
            //        if (reg.GetType() == typeof(RegisterFloat))
            //        {
            //            //(reg as RegisterFloat).Value = 0;
            //        }
            //    }
            //    catch(TimeoutException)
            //    {

            //    }
            //}
        }


#if !CLIENT
        //--------------------------------------------------------------------------------
        // Команда Установить текущее время
        //--------------------------------------------------------------------------------
        public ICommand WriteTimeCommand => new LambdaCommand(OnWriteTimeModeCommandExecuted, CanWriteTimeModeCommand);
        private bool CanWriteTimeModeCommand(object p) => device != null ;
        private void OnWriteTimeModeCommandExecuted(object p)
        {
            //device.RealTime.RealTimeValue = DateTime.Now;
            //device.WriteRegister(device.RealTime);
        }


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
