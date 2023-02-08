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

namespace ServiceControl.ViewModel
{
    internal class MainWindowViewModel : Observable
    {
        public Device device { get; set; }
        DispatcherTimer timer;

        public bool IsCurrentStab
        {
            get => device?.ListInputShort[0].Value == (ushort)RezhStab.StabCurrent;
            set
            {
                device.ListInputShort[0].Value = value ? (ushort)RezhStab.StabCurrent : (ushort)0;
                OnPropertyChanged(nameof(IsCurrentStab));
            }
        }
            
        public bool IsSummPotStab
        {
            get => device?.ListInputShort[0].Value == (ushort)RezhStab.StabSummPot;
            set
            {
                device.ListInputShort[0].Value = value ? (ushort)RezhStab.StabSummPot : (ushort)0;
                OnPropertyChanged(nameof(IsSummPotStab));
            }
        }

        public bool IsPolPotStab
        {
            get => device?.ListInputShort[0].Value == (ushort)RezhStab.StabPolPot;
            set
            {
                device.ListInputShort[0].Value = value ? (ushort)RezhStab.StabPolPot : (ushort)0;
                OnPropertyChanged(nameof(IsPolPotStab));
            }
        }

        public bool IsNaprStab
        {
            get => device?.ListInputShort[0].Value == (ushort)RezhStab.StabNapr;
            set
            {
                device.ListInputShort[0].Value = value ? (ushort)RezhStab.StabNapr : (ushort)0;
                OnPropertyChanged(nameof(IsNaprStab));
            }
        }




        public MainWindowViewModel()
        {
            MbWork work = new MbWork(8800);

            device = new Device216(work, 1);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            device.ReadRegisters(device.ListInput);
            device.ReadRegisters(device.ListHolding);
            device.ReadRegisters(device.ListInputShort);
            device.ReadRegisters(device.ListHoldingShort);
            device.ReadRegistersBool(device.ListDiscret);
            device.ReadRegistersBool(device.ListCoil);

            OnPropertyChanged(nameof(IsCurrentStab));
            OnPropertyChanged(nameof(IsSummPotStab));
            OnPropertyChanged(nameof(IsNaprStab));

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
        // Команда Кнопка Отменить
        //--------------------------------------------------------------------------------
        public ICommand CancelCommand => new LambdaCommand(OnCancelCommandExecuted, CanCancelCommand);
        private bool CanCancelCommand(object p) => true;
        private void OnCancelCommandExecuted(object p)
        {

        }

        #endregion


    }
}
