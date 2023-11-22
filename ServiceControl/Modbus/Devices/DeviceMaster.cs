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
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ServiceControl.Modbus.Devices
{
    internal abstract class DeviceMaster : Device
    {
        public event EventHandler<EventArgs> EndRead;
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public DeviceMaster(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {
            mainVM.SetStatusConnection(StatusConnect.Waiting);
            modbus.slave.DataStore.DataStoreWrittenTo += DataStore_DataStoreWrittenTo;
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += Timer_Tick;

            //modb.slave.ModbusSlaveRequestReceived += Slave_ModbusSlaveRequestReceived;
            //modb.slave.WriteComplete += Slave_WriteComplete;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            mainVM.SetStatusConnection(StatusConnect.Waiting);
           
        }

        //private void Slave_WriteComplete(object sender, global::Modbus.Device.ModbusSlaveRequestEventArgs e)
        //{
        //var data1 = modbus.slave.DataStore.HoldingRegisters[0];
        //var data2 = modbus.slave.DataStore.HoldingRegisters[1];
        //var data3 = modbus.slave.DataStore.HoldingRegisters[2];
        //var data4 = modbus.slave.DataStore.HoldingRegisters[3];
        //modbus.slave.DataStore.DataStoreWrittenTo += DataStore_DataStoreWrittenTo;
        //modbus.slave.DataStore.InputRegisters[4] = 0;
        //}

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

        private void Slave_ModbusSlaveRequestReceived(object sender, global::Modbus.Device.ModbusSlaveRequestEventArgs e)
        {

            //GetRegisterData(e.Message);

            //e.Message.FunctionCode
        }

        //protected abstract void GetRegisterData(IModbusMessage message);
        protected abstract void GetRegisterData(ushort StartAddr, ModbusFunc CodeFunc, ReadOnlyCollection<ushort> listData);
        public abstract void SetRegister(Register reg);

    }
}
