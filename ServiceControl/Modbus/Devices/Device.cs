using ServiceControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace ServiceControl.Modbus.Registers
{
    internal abstract class Device
    {
        //public string Name;
        protected byte Slave;
        protected MbWork modbus;
        protected IEnumerable<List<RegisterBase>> ListList;

        public event EventHandler<EventArgs> EndStartRead;

        private DispatcherTimer timer;

        //----------------------------------------------------------------------------------------------
        // Конструктор
        //----------------------------------------------------------------------------------------------
        public Device(MbWork modb, int slave)
        {
            ListList = new List<List<RegisterBase>>();
            modbus = modb;
            Slave = (byte)slave;
            timer = new DispatcherTimer();
        }

        public async void Start()
        {
            CheckListRegister();
            //StartRequestValue();
            await Task.Run(() => StartRequestValue());

            EndStartRead?.Invoke(null, null);

            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
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

        public void ReadRegister(RegisterBool reg)
        {
            if (reg.CodeFunc == ModbusFunc.CoilRead)
            {
                bool[] res = modbus.ReadRegisterCoil(reg.Address, reg.Size, Slave);
                reg.SetResultValues(res);
            }
            if (reg.CodeFunc == ModbusFunc.Discrete)
            {
                bool[] res = modbus.ReadRegisterDiscret(reg.Address, reg.Size, Slave);
                reg.SetResultValues(res);
            }

        }

        //----------------------------------------------------------------------------------------------
        // чтение информационного регистра
        //----------------------------------------------------------------------------------------------
        public void ReadInfoRegister(Register Reg)
        {
            ushort[] res = modbus.ReadInfoRegister(10, Slave);
            Reg.SetResultValues(res);
        }

        //----------------------------------------------------------------------------------------------
        // чтение списка регистров
        //----------------------------------------------------------------------------------------------
        public void ReadRegisters(IEnumerable<Register> listReg)
        {
            if (listReg == null || listReg.Count() == 0) return;

            RegisterBase reg = listReg.First();
            ushort[] res = null;

            ushort AllSize = (ushort)listReg.Sum(it => it.Size);
            if (reg.CodeFunc == ModbusFunc.Holding)
                res = modbus.ReadRegisterHolding(reg.Address, AllSize, Slave);
            
            if (reg.CodeFunc == ModbusFunc.InputReg)
                res = modbus.ReadRegisterInput(reg.Address, AllSize, Slave);

            if (res?.Length != AllSize) return;

            int pos = 0;
            foreach(var it in listReg)
            {
                ushort[] res2 = new ushort[it.Size];
                for (int i = 0; i < it.Size; i++)
                    res2[i] = res[pos + i];
                it.SetResultValues(res2);
                pos += it.Size;
            }

        }

        //----------------------------------------------------------------------------------------------
        // чтение списка регистров
        //----------------------------------------------------------------------------------------------
        public void ReadRegisters(IEnumerable<RegisterBool> listReg)
        {
            if (listReg == null || listReg.Count() == 0) return;

            RegisterBase reg = listReg.First();
            bool[] res = null;

            ushort AllSize = (ushort)listReg.Sum(it => it.Size);

            if (reg.CodeFunc == ModbusFunc.CoilRead)
                res = modbus.ReadRegisterCoil(reg.Address, AllSize, Slave);

            if (reg.CodeFunc == ModbusFunc.Discrete)
                res = modbus.ReadRegisterDiscret(reg.Address, AllSize, Slave);

            if (res?.Length != AllSize) return;

            int pos = 0;
            foreach (var it in listReg)
            {
                bool[] res2 = new bool[it.Size];
                for (int i = 0; i < it.Size; i++)
                    res2[i] = res[pos + i];
                it.SetResultValues(res2);
                pos += it.Size;
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
        //private async void Timer_TickStart(object sender, EventArgs e)
        //{
        //    timer.Stop();
        //    await Task.Run(() => StartRequestValue());
        //    timer.Tick -= Timer_TickStart;
        //    timer.Tick += Timer_Tick;
        //    timer.Interval = new TimeSpan(0,0,2);
        //    timer.Start();
        //}

        //----------------------------------------------------------------------------------------------
        // событие таймера для чтения
        //----------------------------------------------------------------------------------------------
        private async void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            await Task.Run(() => RequestValue());
            timer.Start();
        }

        protected void CheckReg(IEnumerable<RegisterBase> ListReg)
        {

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



        public abstract Task StartRequestValue();
        public abstract Task RequestValue();
        protected abstract void CheckListRegister();
    }
}
