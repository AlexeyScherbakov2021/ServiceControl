using ServiceControl.Commands;
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
            public RezhStab stab { get; set; }
        }

        //private MainWindowViewModel mainVM;

        private bool IsProcessOnOff = false;

        private int CountTimerSetMode;
        private int? LastSetMode;

        private Visibility _IsAvarModeVisible = Visibility.Hidden;
        public Visibility IsAvarModeVisible { get => _IsAvarModeVisible; set { Set(ref _IsAvarModeVisible, value); } }

        public Device216 device { get; set; }

        public List<Register> ListInput { get; set; }
        public List<Register> ListInput2 { get; set; }

        public List<Register> ListInputMS { get; set; }
        public List<TwoRegister> ListInputDK { get; set; }
        public List<TwoRegister> ListInputAllDK { get; set; }
        public List<RegisterBool> ListStatus { get; set; }
        public List<RegisterBool> ListCoil { get; set; }
        public List<TwoRegister> ListWriteControl { get; set; }
        public List<Register> ListService { get; set; }
        public List<TwoRegister> ListModeNapr { get; set; }
        public List<Register> ListRealTime { get; set; }

#if !CLIENT
        public List<TwoRegister> ListWriteControl2 { get; set; }
        public List<TwoRegister> ListDK3 { get; set; }
#endif

        //public string DeviceInfo =>
        //    "Версия платы " + device?.InfoReg.VersionDev
        //    + "; Верстия ПО " + device?.InfoReg.VersionPO
        //    + "; Год " + device?.InfoReg.Year
        //    + "; № " + device?.InfoReg.NumberDev;

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

            // добавление в список входных параметров
            ListInput = new List<Register>()
            {
                device.NaprSeti1, device.CountEE1, device.NaprSeti2, device.CountEE2
            };

            ListInput2 = new List<Register>()
            {
#if CLIENT
                device.TimeWork, device.TimeProtect,
#endif
                device.Temper,
#if !CLIENT
                device.CurrPolyar
#endif
            };

            // добавление в список силовых модулей
            ListInputMS = new List<Register>();
            for (int i = 0; i < Device216.CountMS; i+=2)
            {
                ListInputMS.Add(device.MS[i]);
            }
            for (int i = 1; i < Device216.CountMS; i+=2)
            {
                ListInputMS.Add(device.MS[i]);
            }

            // добавление в список датчиков коррозии
            ListInputDK = new List<TwoRegister>();
            for (int i = 0; i < 1/*Device216.CountDK*/; i++)
            {
                ListInputDK.Add( new TwoRegister() { Register1 = device.SpeedDK[i], Register2 = device.DeepDK[i] });
            }

            // добавление в список датчиков коррозии
            ListInputAllDK = new List<TwoRegister>();
            for (int i = 1; i < Device216.CountDK; i++)
            {
                ListInputAllDK.Add( new TwoRegister() { Register1 = device.SpeedDK[i], Register2 = device.DeepDK[i] });
            }

            // добавление в список статусов
            ListStatus = new List<RegisterBool>() { device.IllegalAccess, device.DistanceMode, device.Fault, 
                device.BreakCirc, device.OnMS };

            // добавление в список регистров управления
            ListWriteControl = new List<TwoRegister>() { 
                new TwoRegister() { Register1 = device.CurrOutput, 
                    Register2 = device.SetCurrOutput, 
                    stab = RezhStab.StabCurrent },
                new TwoRegister() { Register1 = device.ProtectPotenSumm, 
                    Register2 = device.SetSummPotOutput,  
                    stab = RezhStab.StabSummPot },
                new TwoRegister() { Register1 = device.ProtectPotenPol, 
                    Register2 = device.SetPolPotOutput, 
                    stab = RezhStab.StabPolPot },
                new TwoRegister() { Register1 = device.NaprOutput, 
                    Register2 = device.SetNaprOutput, 
                    stab = RezhStab.StabNapr },
//#if !CLIENT
//                new TwoRegister() { Register1 = device.TempCoolerOn, Register2 = device.TempCoolerOnWrite },
//                new TwoRegister() { Register1 = device.TempCoolerOff, Register2 = device.TempCoolerOffWrite },
//#endif
            }; 

            ListCoil = new List<RegisterBool>() { device.OnOffMS };


            // добавление в список целых регистров управления
#if !CLIENT
            ListWriteControl2 = new List<TwoRegister>() { 
                new TwoRegister() { Register1 = device.TimeWork, Register2 = device.TimeWorkWrite },
                new TwoRegister() { Register1 = device.TimeProtect, Register2 = device.TimeProtectWrite  },
                new TwoRegister() { Register1 = device.TempCoolerOn, Register2 = device.TempCoolerOnWrite },
                new TwoRegister() { Register1 = device.TempCoolerOff, Register2 = device.TempCoolerOffWrite },
            };

            ListDK3 = new List<TwoRegister>()
            {
                new TwoRegister() { Register1 = device.ResistPlast1, Register3 = device.SpeedCorr1 },
                new TwoRegister() { Register1 = device.ResistPlast2, Register3 = device.SpeedCorr2 },
                new TwoRegister() { Register1 = device.ResistPlast3, Register3 = device.SpeedCorr3 },
            };

            ListModeNapr = new List<TwoRegister>() 
            {
                new TwoRegister() { Register1 = device.ModeNaprOutput, Register2 = device.ModeNaprOutputWrite },
            };

            ListRealTime = new List<Register>() { device.RealTime };

#endif



        }


        //--------------------------------------------------------------------------------
        // событие после чтения всех регистров при старте
        //--------------------------------------------------------------------------------
        private void OnEndStartRead(object sender, EventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
            LastSetMode = device.SetMode.Value;
        }

        //--------------------------------------------------------------------------------
        // событие после чтения всех регистров
        //--------------------------------------------------------------------------------
        private void OnReadFinish(object sender, EventArgs e)
        {
            if (CountTimerSetMode == 0)
            {
                IsAvarModeVisible = device.Stabil.Value == LastSetMode
                    ? Visibility.Hidden
                    : Visibility.Visible;

                OnPropertyChanged(nameof(device.Stabil));
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

        //--------------------------------------------------------------------------------
        // Команда Установить режим стабилизации
        //--------------------------------------------------------------------------------
        public ICommand WriteStabCommand => new LambdaCommand(OnWriteStabCommandExecuted, CanWriteStabCommand);
        private bool CanWriteStabCommand(object p) => device != null && device.DistanceMode.ValueBool;
        private void OnWriteStabCommandExecuted(object p)
        {
            if (p is RezhStab mode)
            {
                device.SetMode.Value = (int)mode;
                device.WriteRegister(device.SetMode);
                LastSetMode = (int)mode;
                //LastSetMode = (reg as RegisterInt).Value;
                CountTimerSetMode = 3;
            }
        }
        

#if !CLIENT
        //--------------------------------------------------------------------------------
        // Команда Установить текущее время
        //--------------------------------------------------------------------------------
        public ICommand WriteTimeCommand => new LambdaCommand(OnWriteTimeModeCommandExecuted, CanWriteTimeModeCommand);
        private bool CanWriteTimeModeCommand(object p) => device != null && device.DistanceMode.ValueBool;
        private void OnWriteTimeModeCommandExecuted(object p)
        {
            device.RealTimeWrite.RealTimeValue = DateTime.Now;
            device.WriteRegister(device.RealTimeWrite);
        }


#endif
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
