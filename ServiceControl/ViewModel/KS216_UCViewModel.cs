using ServiceControl.Commands;
using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Registers;
using ServiceControl.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ServiceControl.ViewModel
{
    internal class KS216_UCViewModel : Observable
    {
        public Device216 device { get; set; }
        //DispatcherTimer timer;


        public KS216_UCViewModel()
        {

        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS216_UCViewModel(MbWork work, int Slave)
        {
            device = new Device216(work, Slave);
            //timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(0, 0, 2);
            //timer.Tick += Timer_Tick;
            //timer.Start();
            device.ReadRegisters(device.ListHolding);
            device.ReadRegisters(device.ListHoldingShort);
            device.ReadRegistersBool(device.ListCoil);

        }

        //--------------------------------------------------------------------------------------------
        // Событие таймера
        //--------------------------------------------------------------------------------------------
        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    timer.Stop();
        //    device.ReadRegisters(device.ListInput);
        //    device.ReadRegisters(device.ListInputShort);
        //    device.ReadRegistersBool(device.ListDiscret);
        //    device.ReadRegisters(device.ListInputDK);
        //    timer.Start();

        //}

        #region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Отправить значение
        //--------------------------------------------------------------------------------
        public ICommand WriteValueCommand => new LambdaCommand(OnWriteValueCommandExecuted, CanWriteValueCommand);
        private bool CanWriteValueCommand(object p) => true;
        private void OnWriteValueCommandExecuted(object p)
        {
            int address = int.Parse(p.ToString());

            switch (address)
            {
                case 0x81:
                    device.WriteRegister(device.ListHolding[0]);
                    break;
                case 0x82:
                    device.WriteRegister(device.ListHolding[1]);
                    break;
                case 0x83:
                    device.WriteRegister(device.ListHolding[2]);
                    break;
                case 0x85:
                    device.WriteRegister(device.ListHolding[3]);
                    break;
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Установить режим
        //--------------------------------------------------------------------------------
        public ICommand WriteModeCommand => new LambdaCommand(OnWriteModeCommandExecuted, CanWriteModeCommand);
        private bool CanWriteModeCommand(object p) => true;
        private void OnWriteModeCommandExecuted(object p)
        {
            device.WriteRegister(device.ListHoldingShort[0]);
        }

        #endregion

    }
}
