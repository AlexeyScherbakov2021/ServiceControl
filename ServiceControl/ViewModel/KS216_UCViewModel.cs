﻿using ServiceControl.Commands;
using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using ServiceControl.View;
using System;
using System.Collections.Generic;
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
    internal class KS216_UCViewModel : Observable
    {
        public class TwoRegister
        {
            public Register Register1 { get; set; }
            public Register Register2 { get; set; }
            public RegisterBool Register3 { get; set; }
        }

        //private MainWindowViewModel mainVM;

        private bool IsProcessOnOff = false;

        private int CountTimerSetMode;
        private int? LastSetMode;

        private Visibility _IsAvarModeVisible = Visibility.Hidden;
        public Visibility IsAvarModeVisible { get => _IsAvarModeVisible; set { Set(ref _IsAvarModeVisible, value); } }

        public Device216 device { get; set; }

        public List<Register> ListInput { get; set; }
        public List<Register> ListInputMS { get; set; }
        public List<TwoRegister> ListInputDK { get; set; }
        public List<RegisterBool> ListStatus { get; set; }
        public List<RegisterBool> ListCoil { get; set; }
        public List<TwoRegister> ListWriteControl { get; set; }
        public List<Register> ListService { get; set; }
        public List<TwoRegister> ListDK3 { get; set; }

        public string DeviceInfo =>
            "Версия платы " + device?.InfoReg.VersionDev
            + "; Верстия ПО " + device?.InfoReg.VersionPO
            + "; Год " + device?.InfoReg.Year
            + "; № " + device?.InfoReg.NumberDev;

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS216_UCViewModel()
        {
        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS216_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {
            //mainVM = mainViewModel;
            device = new Device216(mainViewModel, work, Slave);
            //device.InfoReg.PropertyChanged += InfoReg_PropertyChanged;

            device.EndRead += OnReadFinish;
            device.EndStartRead += OnEndStartRead; 

            device.Start();

            ListInput = new List<Register>()
            {
                device.NaprSeti1, device.NaprSeti2, device.CountEE1, device.CountEE2, device.Temper, device.CurrPolyar
            };

            ListInputMS = new List<Register>();
            for (int i = 0; i < Device216.CountMS; i++)
                ListInputMS.Add(device.MS[i]);


            ListInputDK = new List<TwoRegister>();
            for (int i = 0; i < Device216.CountDK; i++)
            {
                ListInputDK.Add( new TwoRegister() { Register1 = device.SpeedDK[i], Register2 = device.DeepDK[i] });
                //ListInputDK.Add(device.DeepDK[i]);
            }

            ListStatus = new List<RegisterBool>() { device.IllegalAccess, device.DistanceMode, device.Fault, 
                device.BreakCirc, device.OnMS };

            ListWriteControl = new List<TwoRegister>() { 
                new TwoRegister() { Register1 = device.CurrOutput, Register2 = device.SetCurrOutput },
                new TwoRegister() { Register1 = device.ProtectPotenSumm, Register2 = device.SetSummPotOutput },
                new TwoRegister() { Register1 = device.ProtectPotenPol, Register2 = device.SetPolPotOutput },
                new TwoRegister() { Register1 = device.NaprOutput, Register2 = device.SetNaprOutput },
                new TwoRegister() { Register1 = device.TempCoolerOn, Register2 = device.TempCoolerOnWrite },
                new TwoRegister() { Register1 = device.TempCoolerOff, Register2 = device.TempCoolerOffWrite },
                new TwoRegister() { Register1 = device.TimeWork, Register2 = device.TimeWorkWrite },
                new TwoRegister() { Register1 = device.TimeProtect, Register2 = device.TimeProtectWrite },
            };

            ListCoil = new List<RegisterBool>() { device.OnOffMS };

            ListDK3 = new List<TwoRegister>()
            {
                new TwoRegister() { Register1 = device.ResistPlast1, Register3 = device.SpeedCorr1 },
                new TwoRegister() { Register1 = device.ResistPlast2, Register3 = device.SpeedCorr2 },
                new TwoRegister() { Register1 = device.ResistPlast3, Register3 = device.SpeedCorr3 },
            };
        }

        private void OnEndStartRead(object sender, EventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
            LastSetMode = device.SetMode.Value;
        }

        private void OnReadFinish(object sender, EventArgs e)
        {
            if (CountTimerSetMode == 0)
            {
                IsAvarModeVisible = device.Stabil.Value == LastSetMode
                    ? Visibility.Hidden
                    : Visibility.Visible;
            }
            else
                --CountTimerSetMode;
        }


        //private async void CheckAvarMode()
        //{
        //    await Task.Delay(3000);
        //    IsAvarModeVisible = device.Stabil.Value == LastSetMode
        //    ? Visibility.Hidden
        //    : Visibility.Visible;
        //}


        //private void InfoReg_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    OnPropertyChanged(nameof(DeviceInfo));
        //}

        #region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Отправить значение
        //--------------------------------------------------------------------------------
        public ICommand WriteValueCommand => new LambdaCommand(OnWriteValueCommandExecuted, CanWriteValueCommand);
        private bool CanWriteValueCommand(object p) => device != null && device.DistanceMode.ValueBool;
        private void OnWriteValueCommandExecuted(object p)
        {
            if (p is Register reg)
            {
                try
                {
                    device.WriteRegister(reg);
                }
                catch(TimeoutException)
                {

                }

                // была установка режима стабилизации
                if(reg.Address == 0x84)
                {
                    LastSetMode = (reg as RegisterInt).Value;
                    CountTimerSetMode = 3;
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Установить текущее время
        //--------------------------------------------------------------------------------
        public ICommand WriteTimeCommand => new LambdaCommand(OnWriteTimeModeCommandExecuted, CanWriteTimeModeCommand);
        private bool CanWriteTimeModeCommand(object p) => true;
        private void OnWriteTimeModeCommandExecuted(object p)
        {
            device.RealTimeWrite.RealTimeValue = DateTime.Now;
            device.WriteRegister(device.RealTimeWrite);
        }

        //--------------------------------------------------------------------------------
        // Команда Записать в регистр Bool
        //--------------------------------------------------------------------------------
        //public ICommand OnOffCommand => new LambdaCommand(OnOnOffCommandExecuted);
        private void WriteValueBoolCommand(object p)
        {
            RegisterBool reg = p as RegisterBool;
            device.WriteRegister(reg);
        }

        //--------------------------------------------------------------------------------
        // Команда Вкл - откл силовых модулей
        //--------------------------------------------------------------------------------
        public ICommand OnOffCommand => new LambdaCommand(WriteOnOffMSCommand, CanOnOfCommand);
        private bool CanOnOfCommand(object p) => !IsProcessOnOff && device != null && device.DistanceMode.ValueBool;
        private async void WriteOnOffMSCommand(object p)
        {
            IsProcessOnOff = true;
            device.OnOffMSWrite.ValueBool ^= true;
            device.WriteRegister(device.OnOffMSWrite);
            await Task.Delay(10000);
            IsProcessOnOff = false;
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion



    }
}
