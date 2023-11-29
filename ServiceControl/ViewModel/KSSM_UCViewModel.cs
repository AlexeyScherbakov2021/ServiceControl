using ServiceControl.Commands;
using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using ServiceControl.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    internal class KSSM_UCViewModel : Observable //, IDataErrorInfo
    {

        //public class TwoRegister
        //{
        //    public Register Register1 { get; set; }
        //    public Register Register2 { get; set; }
        //}

        public DeviceKSSM device { get; set; }

        public List<TwoRegister> ListWriteControl { get; set; }
        public List<Register> ListControl { get; set; }
        public List<Register> ListRealTime { get; set; }

        private KIP_KSSM _SelectedKIP;
        public KIP_KSSM SelectedKIP 
        { 
            get => _SelectedKIP; 
            set 
            { 
                Set(ref _SelectedKIP, value); 
                OnPropertyChanged(nameof(_SelectedKIP.listInput)); 
            } 
        }


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KSSM_UCViewModel()
        {
        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KSSM_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {

            device = new DeviceKSSM(mainViewModel, work, Slave);
            device.EndRead += OnReadFinish;
            device.EndStartRead += OnEndStartRead; 
            device.Start();

            ListRealTime = new List<Register>
            {
                device.RealTime
            };

            // добавление в список регистров управления
            //ListWriteControl = new List<TwoRegister>() { 
            //    new TwoRegister() { Register1 = device.KIP_count, Register2 = device.KIP_countW, },
            //    new TwoRegister() { Register1 = device.KIP_find, Register2 = device.KIP_findW,},
            //    new TwoRegister() { Register1 = device.SD_check, Register2 = device.SD_checkW,},
            //    new TwoRegister() { Register1 = device.ElectricMeter1, Register2 = device.ElectricMeter1W,},
            //    new TwoRegister() { Register1 = device.ElectricMeter2, Register2 = device.ElectricMeter2W,},
            //    new TwoRegister() { Register1 = device.isDoor, Register2 = device.isDoorW,},
            //};

            ListControl = new List<Register>() { device.KIP_count, device.KIP_find,device.SD_check,device.ElectricMeter1, 
                device.ElectricMeter2,device.isDoor
            };

        }


        //--------------------------------------------------------------------------------
        // событие после чтения всех регистров при старте
        //--------------------------------------------------------------------------------
        private void OnEndStartRead(object sender, EventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
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
        //        catch(TimeoutException)
        //        {

        //        }
        //    }
        //}


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
