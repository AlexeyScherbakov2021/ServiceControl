using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ServiceControl.Modbus.Registers
{
    internal abstract class Device
    {
        //public string Name;
        public List<DoubleRegister> ListInput { get; set; }
        public List<DoubleRegister> ListHolding { get; set; }
        public List<UshortRegister> ListInputShort { get; set; }
        public List<UshortRegister> ListHoldingShort { get; set; }
        public List<BoolRegister> ListCoil { get; set; }
        public List<BoolRegister> ListDiscret { get; set; }
        public byte Slave;
        protected MbWork modbus;

        private DispatcherTimer timer;

        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device(MbWork modb, int slave)
        {
            modbus = modb;
            Slave = (byte)slave;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += Timer_Tick;
            timer.Start();
        }



        //----------------------------------------------------------------------------------------------
        // чтение списка регистров
        //----------------------------------------------------------------------------------------------
        public void ReadRegisters(IEnumerable<Register<ushort, double>> listReg)
        {
            foreach (var item in listReg)
            {
                if (item.CodeFunc == ModbusFunc.Holding)
                    modbus.ReadRegisterHolding(item, Slave);
                if (item.CodeFunc == ModbusFunc.InputReg)
                    modbus.ReadRegisterInput(item, Slave);
            }
        }

        //----------------------------------------------------------------------------------------------
        // чтение списка регистров
        //----------------------------------------------------------------------------------------------
        public void ReadRegisters(IEnumerable<Register<ushort, ushort>> listReg)
        {
            foreach (var item in listReg)
            {
                if (item.CodeFunc == ModbusFunc.Holding)
                    modbus.ReadRegisterHolding(item, Slave);
                if (item.CodeFunc == ModbusFunc.InputReg)
                    modbus.ReadRegisterInput(item, Slave);
            }
        }

        //----------------------------------------------------------------------------------------------
        // чтение списка регистров
        //----------------------------------------------------------------------------------------------
        public void ReadRegistersBool(IEnumerable<Register<bool, bool>> listReg)
        {
            foreach (var item in listReg)
            {
                if(item.CodeFunc == ModbusFunc.CoilRead)
                    modbus.ReadRegisterCoil(item, Slave);
                if(item.CodeFunc == ModbusFunc.Discrete)
                    modbus.ReadRegisterDiscret(item, Slave);
            }
        }

        //----------------------------------------------------------------------------------------------
        // запись регистроа
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(Register<ushort, double> Reg)
        {
            modbus.WriteRegister(Reg, Slave);
        }


        //----------------------------------------------------------------------------------------------
        // запись регистроа
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(Register<ushort, ushort> Reg)
        {
            modbus.WriteRegister(Reg, Slave);
        }


        private async void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            await Task.Run(() => RequestValue());
            timer.Start();
        }


        //public Task RequestValue()
        //{
        //    Thread.Sleep(4000);

        //    return Task.CompletedTask;
        //}

        public abstract Task RequestValue();
    }
}
