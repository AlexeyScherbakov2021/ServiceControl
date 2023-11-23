using Modbus.Data;
using Modbus.Message;
using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ServiceControl.Modbus.Devices
{
    internal abstract class DeviceMaster : Device
    {
        public event EventHandler<EventArgs> EndRead;
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private bool isWorked;

        public DeviceMaster(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {
            mainVM.SetStatusConnection(StatusConnect.Waiting);
            modbus.slave.DataStore.DataStoreWrittenTo += DataStore_DataStoreWrittenTo;
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            mainVM.SetStatusConnection(StatusConnect.Waiting);
           
        }


        public override async void Start()
        {
    m2:
            isWorked = true;
            try
            {
                await modbus.slave.ListenAsync();
            }
            catch
            {
                modbus.FlushBufferCOM();
                goto m2;
            }

            if (isWorked == true)
            {
                mainVM.SetStatusConnection(StatusConnect.NotAnswer);
                //modbus.Disconnect();
                //Thread.Sleep(1000);
                //modbus.CreateConnectSlave();
                //goto m2;
            }

        }

        private void DataStore_DataStoreWrittenTo(object sender, DataStoreEventArgs e)
        {
            timer.Stop();

            mainVM.SetStatusConnection(StatusConnect.Connected);

            ModbusFunc func;

            switch(e.ModbusDataType)
            {
                case ModbusDataType.Coil:
                    func = ModbusFunc.Coil;
                    break;
                case ModbusDataType.Input:
                    func = ModbusFunc.InputDiscrete;
                    break;
                case ModbusDataType.InputRegister:
                    func = ModbusFunc.InputRegister;
                    break;
                case ModbusDataType.HoldingRegister:
                    func = ModbusFunc.HoldingRegister;
                    break;
                default:
                    func = ModbusFunc.None;
                    break;
            }

            if(e.Data.B != null)
                GetRegisterData(e.StartAddress, func, e.Data.B);

            //if (e.Data.A != null)
            //    GetRegisterDataBool(e.StartAddress, func, e.Data.A);
            EndRead?.Invoke(null, null);

            timer.Start();
        }

        public override void Stop()
        {
            isWorked = false;
        }


        protected abstract void GetRegisterData(ushort StartAddr, ModbusFunc CodeFunc, ReadOnlyCollection<ushort> listData);
        public abstract void SetRegister(Register reg);

    }
}
