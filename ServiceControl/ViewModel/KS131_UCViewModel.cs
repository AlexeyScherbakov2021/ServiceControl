using ServiceControl.Commands;
using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ServiceControl.ViewModel
{
    internal class KS131_UCViewModel : Observable
    {
        public class TwoRegister
        {
            public Register Reg1 { get; set; }
            public Register Reg2 { get; set; }
            public RezhStab stab { get; set; }
        }


        private int? LastSetMode;
        private int CountTimerSetMode;

        public Device131 device { get; set; }

        public List<TwoRegister> ListCtrlReg { get; set; }
        public List<Register> ListReg { get; set; }
        public List<Register> ListKIP { get; set; }
        public List<RegisterStatus131> ListStatus { get; set; }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS131_UCViewModel()
        {

        }

        public KS131_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {
            device = new Device131(mainViewModel, work, Slave);
            device.EndRead += OnReadFinish;


            ListCtrlReg = new List<TwoRegister>()
            {
                new TwoRegister { Reg1 = device.CurrOutput, Reg2 = device.SetCurrOutput, stab = RezhStab.StabCurrent},
                new TwoRegister { Reg1 = device.VoltOutput, Reg2 = device.SetVoltageOutput, stab = RezhStab.StabNapr},
                new TwoRegister { Reg1 = device.Potencial, Reg2 = device.SetPotOutput, stab = RezhStab.StabSummPot},
                new TwoRegister { Reg1 = device.PolPotencial, Reg2 = device.SetPotOutput, stab = RezhStab.StabPolPot},
            };

            ListReg = new List<Register>() 
            {
                device.TimeProtect, device.NaprSeti, device.Temper
            };

            ListKIP = new List<Register>();
            ListKIP.AddRange(device.ListKIP);


            device.Start();
        }

        //--------------------------------------------------------------------------------
        // событие после чтения всех регистров
        //--------------------------------------------------------------------------------
        private void OnReadFinish(object sender, EventArgs e)
        {
            if (CountTimerSetMode == 0)
            {
                //IsAvarModeVisible = device.Stabil.Value == LastSetMode
                //    ? Visibility.Hidden
                //    : Visibility.Visible;

                OnPropertyChanged(nameof(device.Mode));
            }
            else
                --CountTimerSetMode;
        }



        #region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Отправить значение
        //--------------------------------------------------------------------------------
        public ICommand WriteValueCommand => new LambdaCommand(OnWriteValueCommandExecuted, CanWriteValueCommand);
        private bool CanWriteValueCommand(object p) => device != null && device.Mode.distMode == DistMode.Distance;
        private void OnWriteValueCommandExecuted(object p)
        {
            if (p is Register reg)
            {
                try
                {
                    device.WriteRegister(reg);
                }
                catch (TimeoutException)
                {

                }
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Установить режим стабилизации
        //--------------------------------------------------------------------------------
        public ICommand WriteStabCommand => new LambdaCommand(OnWriteStabCommandExecuted, CanWriteStabCommand);
        private bool CanWriteStabCommand(object p) => device != null && device.Mode.distMode == DistMode.Distance;
        private void OnWriteStabCommandExecuted(object p)
        {
            if (p is RezhStab mode)
            {
                switch(mode)
                {
                    case RezhStab.StabCurrent:
                        device.Mode.Value = 3;
                        device.Mode.Mode = ModeStab.Current;
                        break;
                    case RezhStab.StabNapr:
                        device.Mode.Value = 4;
                        device.Mode.Mode = ModeStab.Voltage;
                        break;
                    case RezhStab.StabSummPot:
                        device.Mode.Value = 7;
                        device.Mode.Mode = ModeStab.SummPot;
                        break;
                    case RezhStab.StabPolPot:
                        device.Mode.Value = 5;
                        device.Mode.Mode = ModeStab.PolPot;
                        break;
                }

                //device.Mode.Value = (int)mode;
                device.WriteRegister(device.Mode);
                LastSetMode = (int)mode;
                //LastSetMode = (reg as RegisterInt).Value;
                CountTimerSetMode = 3;
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Установить текущее время
        //--------------------------------------------------------------------------------
        public ICommand WriteTimeCommand => new LambdaCommand(OnWriteTimeModeCommandExecuted, CanWriteTimeModeCommand);
        private bool CanWriteTimeModeCommand(object p) => device != null && device.Mode.distMode == DistMode.Distance;
        private void OnWriteTimeModeCommandExecuted(object p)
        {
            device.RealTime.RealTimeValue = DateTime.Now;
            device.WriteRegister(device.RealTime);
        }


        #endregion

    }
}
