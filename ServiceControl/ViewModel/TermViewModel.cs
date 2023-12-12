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
    internal class TermViewModel : Observable //, IDataErrorInfo
    {

        public DeviceKIPM5Ext device { get; set; }

        public List<Register> ListHolding { get; set; }


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public TermViewModel()
        {

        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public TermViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {

            device = new DeviceKIPM5Ext(mainViewModel, work, Slave);
            //device.EndRead += OnReadFinish;
            device.EndStartRead += OnEndStartRead; 

            // добавление в список входных параметров
            ListHolding = new List<Register>()
            {
               device.Flags, device.CurrOut, device.VoltOut, 
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
            if (p is Register reg)
            {
                try
                {
                    device.WriteRegister(reg);
                    //if (reg.GetType() == typeof(RegisterFloat))
                    //{
                    //    //(reg as RegisterFloat).Value = 0;
                    //}
                }
                catch (TimeoutException)
                {

                }
            }
        }


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
