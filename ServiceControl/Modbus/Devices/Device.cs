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
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_TickStart;
            timer.Start();
        }


        public void ReadRegister(Register Reg)
        {
            if (Reg.CodeFunc == ModbusFunc.Holding)
            {
                ushort[] res = modbus.ReadRegisterHolding(Reg.Address, Reg.Size, Slave);
                Reg.SetResultValues(res);
            }
            if (Reg.CodeFunc == ModbusFunc.InputReg)
            {
                ushort[] res = modbus.ReadRegisterInput(Reg.Address, Reg.Size, Slave);
                Reg.SetResultValues(res);
            }
        }
        //----------------------------------------------------------------------------------------------
        // чтение списка регистров
        //----------------------------------------------------------------------------------------------
        public void ReadRegisters(IEnumerable<Register> listReg)
        {
            foreach (var item in listReg)
            {
                if (item.CodeFunc == ModbusFunc.Holding)
                {
                    ushort[] res = modbus.ReadRegisterHolding(item.Address, item.Size,  Slave);
                    item.SetResultValues(res);
                }
                if (item.CodeFunc == ModbusFunc.InputReg)
                {
                    ushort[] res = modbus.ReadRegisterInput(item.Address, item.Size, Slave);
                    item.SetResultValues(res);
                }
            }
        }

        //----------------------------------------------------------------------------------------------
        // чтение списка регистров
        //----------------------------------------------------------------------------------------------
        public void ReadRegisters(IEnumerable<RegisterBool> listReg)
        {
            foreach (var item in listReg)
            {
                if (item.CodeFunc == ModbusFunc.CoilRead)
                {
                    bool[] res = modbus.ReadRegisterCoil(item.Address, item.Size, Slave);
                    item.SetResultValues(res);
                }
                if (item.CodeFunc == ModbusFunc.Discrete)
                {
                    bool[] res = modbus.ReadRegisterDiscret(item.Address, item.Size, Slave);
                    item.SetResultValues(res);
                }
            }
        }

        //----------------------------------------------------------------------------------------------
        // запись регистроа
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(Register Reg)
        {
            if (Reg.Size > 1)
            {
                ushort[] data = Reg.SetOutput();
                modbus.WriteRegister(Reg.Address, data, Slave);
            }
            else
            {
                ushort[] val = Reg.SetOutput();
                modbus.WriteRegister(Reg.Address, val[0], Slave);
            }
        }


        //----------------------------------------------------------------------------------------------
        // запись регистроа
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(RegisterBool Reg)
        {
            bool val = Reg.SetOutput();
            modbus.WriteRegister(Reg.Address, val, Slave);
        }


        //----------------------------------------------------------------------------------------------
        // стартовое событие таймера для чтения
        //----------------------------------------------------------------------------------------------
        private async void Timer_TickStart(object sender, EventArgs e)
        {
            timer.Stop();
            await Task.Run(() => StartRequestValue());
            timer.Tick -= Timer_TickStart;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0,0,2);
            timer.Start();
        }

        //----------------------------------------------------------------------------------------------
        // событие таймера для чтения
        //----------------------------------------------------------------------------------------------
        private async void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            await Task.Run(() => RequestValue());
            timer.Start();
        }


        public abstract Task StartRequestValue();

        public abstract Task RequestValue();
    }
}
