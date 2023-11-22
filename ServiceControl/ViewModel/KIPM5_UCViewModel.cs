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
    internal class KIPM5_UCViewModel : Observable //, IDataErrorInfo
    {

        public class TwoRegister
        {
            public Register Register1 { get; set; }
            public Register Register2 { get; set; }
        }



        //private int CountTimerSetMode;

        private Visibility _IsAvarModeVisible = Visibility.Hidden;
        public Visibility IsAvarModeVisible { get => _IsAvarModeVisible; set { Set(ref _IsAvarModeVisible, value); } }

        public DeviceKIPM5 device { get; set; }

        public List<Register> ListInput { get; set; }
        public List<Register> ListInput2 { get; set; }

        //public List<Register> ListInputMS { get; set; }
        public List<TwoRegister> ListInputDK { get; set; }
        //public List<TwoRegister> ListInputAllDK { get; set; }
        public List<RegisterBool> ListStatus { get; set; }
        public RegisterBool isDoor { get; set; }
        public RegisterBool isDK1 { get; set; }
        public RegisterBool isDK2 { get; set; }
        public RegisterBool isDK3 { get; set; }
        public RegisterBool isUSIKP { get; set; }

        //public List<RegisterBool> ListCoil { get; set; }
        public List<TwoRegister> ListWriteControl { get; set; }
        public List<Register> ListService { get; set; }
        //public List<TwoRegister> ListModeNapr { get; set; }
        public List<Register> ListRealTime { get; set; }

#if !CLIENT
#endif


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KIPM5_UCViewModel()
        {
        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KIPM5_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {

            device = new DeviceKIPM5(mainViewModel, work, Slave);
            device.EndRead += OnReadFinish;
            device.EndStartRead += OnEndStartRead; 
            device.Start();

            ListRealTime = new List<Register>
            {
                device.RealTime
            };
            // добавление в список входных параметров
            ListInput = new List<Register>()
            {
                device.VoltPower, device.CurrOut,device.VoltOut,
                device.SummPot, device.PolPot, device.CurrPot,  device.VoltNaveden,
                device.FreqVoltNaveden, device.TemperBoard, 
                device.Power, device.CountPower, device.PeriodADC, device.UpLimitCurr,
                device.DownLimitCurr, device.UpLimitVolt, device.DownLimitVolt,
                device.UpLimitVoltSP, device.DownLimitVoltSP,device.OutUpLimitCurr,
                device.OutDownLimitCurr, device.OutUpLimitVolt, device.OutDownLimitVolt,
                device.OutUpLimitVoltSP,device.OutDownLimitVoltSP,device.SummPotRMS         };

            // добавление в список регистров управления
            ListWriteControl = new List<TwoRegister>() { 
                new TwoRegister() { Register1 = device.K_shunt, 
                    Register2 = device.K_shunt, },
                new TwoRegister() { Register1 = device.VoltControl, 
                    Register2 = device.VoltControl,},
            };

            // добавление в список целых регистров управления
#if !CLIENT
            ListInputDK = new List<TwoRegister>()
            {
                new TwoRegister() { Register1 = device.SpeedKorr, Register2 = device.DeepKorr },
            };


#endif



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
        private void OnReadFinish(object sender, EventArgs e)
        {

        }



#region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Отправить значение
        //--------------------------------------------------------------------------------
        public ICommand WriteValueCommand => new LambdaCommand(OnWriteValueCommandExecuted, CanWriteValueCommand);
        private bool CanWriteValueCommand(object p) => device != null;
        private void OnWriteValueCommandExecuted(object p)
        {
            if (p is Register reg)
            {
                try
                {
                    device.WriteRegister(reg);
                    if (reg.GetType() == typeof(RegisterFloat))
                    {
                        //(reg as RegisterFloat).Value = 0;
                    }
                }
                catch(TimeoutException)
                {

                }
            }
        }


#if !CLIENT
        //--------------------------------------------------------------------------------
        // Команда Установить текущее время
        //--------------------------------------------------------------------------------
        public ICommand WriteTimeCommand => new LambdaCommand(OnWriteTimeModeCommandExecuted, CanWriteTimeModeCommand);
        private bool CanWriteTimeModeCommand(object p) => device != null ;
        private void OnWriteTimeModeCommandExecuted(object p)
        {
            device.RealTime.RealTimeValue = DateTime.Now;
            device.WriteRegister(device.RealTime);
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
