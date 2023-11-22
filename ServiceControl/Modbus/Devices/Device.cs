using ServiceControl.Infrastructure;
using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace ServiceControl.Modbus.Devices
{
    public enum StatusConnect { Disconnected, Connected, Answer, NotAnswer, Waiting };

    internal abstract class Device 
    {
        private bool _IsTimeout;
        public bool IsTimeout 
        { 
            get => _IsTimeout;
            set
            {
                if (_IsTimeout == value) return;
                _IsTimeout = value;
                mainVM.SetStatusConnection(_IsTimeout ? StatusConnect.NotAnswer : StatusConnect.Answer);
                //mainVM.IsConnected = !_IsTimeout;
            }
        }

        protected bool IsReadAllRegisters = false;
        //private bool TimerWork = false;
        protected byte Slave;
        protected MbWork modbus;
        protected IEnumerable<List<RegisterBase>> ListList;
        //public event EventHandler<EventArgs> EndStartRead;
        //public event EventHandler<EventArgs> EndRead;
        //private DispatcherTimer timer;
        protected MainWindowViewModel mainVM;

        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device(MainWindowViewModel vm, MbWork modb, int slave)
        {
            mainVM = vm;
            ListList = new List<List<RegisterBase>>();
            modbus = modb;
            Slave = (byte)slave;
            //timer = new DispatcherTimer();
        }




        //----------------------------------------------------------------------------------------------
        // проверка регистров на однотипность и последовательность
        //----------------------------------------------------------------------------------------------
        protected void CheckReg(IEnumerable<RegisterBase> ListReg)
        {
            if (ListReg == null)
                return;

            ushort StartAddress = 0;
            ModbusFunc PrevFunc = ModbusFunc.None;
            foreach (var it in ListReg)
            {
                if (StartAddress == 0)
                {
                    StartAddress = it.Address;
                    PrevFunc = it.CodeFunc;
                }

                if (it.Address != StartAddress || it.CodeFunc != PrevFunc)
                    throw new Exception($"Список адрес {ListReg.First().Address} с размером {ListReg.Count()} должен быть непрерывным.");

                StartAddress += it.Size;
            }
        }

        protected abstract void CheckListRegister();

        public virtual void ChangeLangRegister() { }

    }
}
