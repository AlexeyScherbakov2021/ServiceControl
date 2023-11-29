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
    internal class BI_MM_UCViewModel : Observable //, IDataErrorInfo
    {
        //public class TwoRegister
        //{
        //    public Register Register1 { get; set; }
        //    public Register Register2 { get; set; }
        //}

        private Visibility _IsAvarModeVisible = Visibility.Hidden;
        public Visibility IsAvarModeVisible { get => _IsAvarModeVisible; set { Set(ref _IsAvarModeVisible, value); } }

        public DeviceBIMMaster device { get; set; }

        public List<Register> ListInput { get; set; }
        public List<Register> ListInput2 { get; set; }

        public List<TwoRegister> ListInputDK { get; set; }

        public List<RegisterBool> ListStatus { get; set; }
        public RegisterBool isDoor { get; set; }
        public RegisterBool isDK1 { get; set; }
        public RegisterBool isDK2 { get; set; }
        public RegisterBool isDK3 { get; set; }
        public RegisterBool isError { get; set; }

        public List<Register> ListHolding { get; set; }
        public List<Register> ListHolding2 { get; set; }
        public List<TwoRegister> ListHoldingTwo { get; set; }
        public List<TwoRegister> ListHoldingTwo2 { get; set; }
        public List<TwoRegister> ListHoldingTwoCal { get; set; }
        public List<Register> ListRealTime { get; set; }


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public BI_MM_UCViewModel()
        {
        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public BI_MM_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {
            isDoor = new RegisterBool() { Name = "Вскрытие", ValueString = "норм", CodeFunc= ModbusFunc.InputRegister, Address = 1};
            isDK1 = new RegisterBool() { Name = "ДК1", ValueString = "норм", CodeFunc = ModbusFunc.InputRegister, Address = 1 };
            isDK2 = new RegisterBool() { Name = "ДК2", ValueString = "норм", CodeFunc = ModbusFunc.InputRegister, Address = 1 };
            isDK3 = new RegisterBool() { Name = "ДК3", ValueString = "норм", CodeFunc = ModbusFunc.InputRegister, Address = 1 };
            isError = new RegisterBool() { Name = "Системная ошибка", ValueString = "норм", CodeFunc = ModbusFunc.InputRegister, Address = 1 };
            
            ListStatus = new List<RegisterBool>() { isDoor, isDK1, isDK2, isDK3, isError };


            device = new DeviceBIMMaster(mainViewModel, work, Slave);
            device.EndRead += OnReadFinish;
            //device.EndStartRead += OnEndStartRead; 

            ListRealTime = new List<Register>
            {
                device.RealTime, device.TimeAwak,

            };
            // добавление в список входных параметров
            ListHolding = new List<Register>()
            {
                device.Status, device.ID, device.NumPacket,device.VoltPower, 
                device.VoltCurrOtnosL, device.VoltCurrOtnosR,
                device.DataCurrBIT_L, device.DataCurrBIT_R, 
                device.SpeedKorr, device.DeepKorr, device.SlaveID, device.WakeUp,
                device.VoltOut, device.CurrOut, device.TemperBoard,
                device.NominalShunt, device.CntWriteEEPROM
            };

            ListHolding2 = new List<Register>()
            {
                device.VoltOutC, device.CurrOutC, device.ValtageOtnosC,device.Bit_L_0, 
                device.Bit_L_Koef, device.Bit_R_0,  device.Bit_R_Koef, 
                device.ReadWriteCalb, device.DenyCalibr
            };

            // добавление в список регистров управления
            ListHoldingTwo = new List<TwoRegister>() {
                new TwoRegister() { Register1 = device.SummPot,  Register2 = device.SummPot2, },
                new TwoRegister() { Register1 = device.PolPot, Register2 = device.PolPot2,},
                new TwoRegister() { Register1 = device.CurrPot, Register2 = device.CurrPot2,},
            };

            ListHoldingTwo2 = new List<TwoRegister>() {
                new TwoRegister() { Register1 = device.ConstSumPot,  Register2 = device.ConstSumPot2, },
                new TwoRegister() { Register1 = device.VoltNaveden, Register2 = device.VoltNaveden2,},
                new TwoRegister() { Register1 = device.FreqVoltNaveden, Register2 = device.FreqVoltNaveden2,},
            };

            ListHoldingTwoCal = new List<TwoRegister>() {
                new TwoRegister() { Register1 = device.SummPotC,  Register2 = device.SummPotC2, },
                new TwoRegister() { Register1 = device.PolPotC, Register2 = device.PolPotC2,},
                new TwoRegister() { Register1 = device.CurrPotC, Register2 = device.CurrPotC2,},
            };

            // добавление в список целых регистров управления
#if !CLIENT
            ListInputDK = new List<TwoRegister>()
            {
                new TwoRegister() { Register1 = device.SpeedKorr, Register2 = device.DeepKorr },
            };


#endif
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
        private void OnReadFinish(object sender, EventArgs e)
        {
            if (device.Status.Value == null)
                return;

            if((device.Status.Value.Value & 1) == 1)
            {
                isDoor.IsAlarm = true;
                isDoor.ValueString = "не норм.";
            }
            else
            {
                isDoor.IsAlarm = false;
                isDoor.ValueString = "норм";
            }

            if((device.Status.Value.Value & 2) == 2)
            {
                isDK1.IsAlarm = true;
                isDK1.ValueString = "не норм.";
            }
            else
            {
                isDK1.IsAlarm = false;
                isDK1.ValueString = "норм";
            }

            if((device.Status.Value.Value & 4) == 4)
            {
                isDK2.IsAlarm = true;
                isDK2.ValueString = "не норм.";
            }
            else
            {
                isDK2.IsAlarm = false;
                isDK2.ValueString = "норм";
            }

            if((device.Status.Value.Value & 8) == 8)
            {
                isDK3.IsAlarm = true;
                isDK3.ValueString = "не норм.";
            }
            else
            {
                isDK3.IsAlarm = false;
                isDK3.ValueString = "норм";
            }

            if((device.Status.Value.Value & 16) == 16)
            {
                isError.IsAlarm = true;
                isError.ValueString = "не норм.";
            }
            else
            {
                isError.IsAlarm = false;
                isError.ValueString = "норм";
            }

        }



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
            //device.TimeNow.RealTimeValue = DateTime.Now;
            //device.WriteRegister(device.TimeNow);
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
