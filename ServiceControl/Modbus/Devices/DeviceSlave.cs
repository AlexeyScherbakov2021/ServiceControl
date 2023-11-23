using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ServiceControl.Modbus.Devices
{
    internal abstract class DeviceSlave : Device
    {
        private DispatcherTimer timer;
        public event EventHandler<EventArgs> EndStartRead;
        public event EventHandler<EventArgs> EndRead;

        protected DeviceSlave(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {
            timer = new DispatcherTimer();
        }


        //----------------------------------------------------------------------------------------------
        // первоначальное чтение регистров (в т.ч. для записи)
        //----------------------------------------------------------------------------------------------
        public override async void Start()
        {
            CheckListRegister();
            //StartRequestValue();
            await Task.Run(() => StartRequestValue());

            if (!modbus.IsWorked()) return;

            EndStartRead?.Invoke(null, null);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        //----------------------------------------------------------------------------------------------
        // 
        //----------------------------------------------------------------------------------------------
        public override void Stop()
        {
            timer.Stop();
        }


        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public void ReadRegister(Register Reg)
        {
            try
            {
                if (Reg.CodeFunc == ModbusFunc.HoldingRegister)
                {
                    ushort[] res = modbus.ReadRegisterHolding(Reg.Address, Reg.Size, Slave);
                    Reg.SetResultValues(res);
                }
                if (Reg.CodeFunc == ModbusFunc.InputRegister)
                {
                    ushort[] res = modbus.ReadRegisterInput(Reg.Address, Reg.Size, Slave);
                    Reg.SetResultValues(res);
                }

                if (IsTimeout)
                {
                    IsTimeout = false;

                }

                if (IsTimeout)
                {
                    IsTimeout = false;
                    IsReadAllRegisters = true;
                }

            }

            catch (TimeoutException)
            {
                IsTimeout = true;
            }

        }

        //----------------------------------------------------------------------------------------------
        // чтение регистра
        //----------------------------------------------------------------------------------------------
        public void ReadRegister(RegisterBool reg)
        {
            try
            {
                if (reg.CodeFunc == ModbusFunc.Coil)
                {
                    bool[] res = modbus.ReadRegisterCoil(reg.Address, reg.Size, Slave);
                    reg.SetResultValues(res);
                }
                if (reg.CodeFunc == ModbusFunc.InputDiscrete)
                {
                    bool[] res = modbus.ReadRegisterDiscret(reg.Address, reg.Size, Slave);
                    reg.SetResultValues(res);
                }

                if (IsTimeout)
                {
                    IsTimeout = false;
                    IsReadAllRegisters = true;
                }

            }
            catch (TimeoutException)
            {
                IsTimeout = true;
            }

        }

        //----------------------------------------------------------------------------------------------
        // чтение информационного регистра
        //----------------------------------------------------------------------------------------------
        public void ReadInfoRegister(Register Reg)
        {
            try
            {
                ushort[] res = modbus.ReadInfoRegister(10, Slave);
                Reg.SetResultValues(res);
            }
            catch (TimeoutException)
            {
                //IsTimeout = true;
            }
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

            try
            {
                if (reg.CodeFunc == ModbusFunc.HoldingRegister)
                    res = modbus.ReadRegisterHolding(reg.Address, AllSize, Slave);

                if (reg.CodeFunc == ModbusFunc.InputRegister)
                    res = modbus.ReadRegisterInput(reg.Address, AllSize, Slave);

                if (res?.Length != AllSize) return;

                int pos = 0;
                foreach (var it in listReg)
                {
                    ushort[] res2 = new ushort[it.Size];
                    for (int i = 0; i < it.Size; i++)
                        res2[i] = res[pos + i];
                    it.SetResultValues(res2);
                    pos += it.Size;
                }

                if (IsTimeout)
                {
                    IsTimeout = false;
                    IsReadAllRegisters = true;
                }

            }
            catch (TimeoutException)
            {
                IsTimeout = true;
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

            try
            {
                if (reg.CodeFunc == ModbusFunc.Coil)
                    res = modbus.ReadRegisterCoil(reg.Address, AllSize, Slave);

                if (reg.CodeFunc == ModbusFunc.InputDiscrete)
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

                if (IsTimeout)
                {
                    IsTimeout = false;
                    IsReadAllRegisters = true;
                }

            }
            catch (TimeoutException)
            {
                IsTimeout = true;
            }
        }

        //----------------------------------------------------------------------------------------------
        // запись регистра
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(RegisterBool Reg)
        {
            bool val = Reg.SetOutput();
            modbus.WriteRegister(Reg.Address, val, Slave);
        }

        //----------------------------------------------------------------------------------------------
        // запись регистра
        //----------------------------------------------------------------------------------------------
        public void WriteRegister(Register Reg)
        {
            if (Reg.Size > 1)
            {
                ushort[] data = Reg.SetOutput();
                if (data != null)
                    modbus.WriteRegister(Reg.Address, data, Slave);
            }
            else
            {
                ushort[] val = Reg.SetOutput();
                if (val != null)
                    modbus.WriteRegister(Reg.Address, val[0], Slave);
            }
        }


        //----------------------------------------------------------------------------------------------
        // событие таймера для чтения
        //----------------------------------------------------------------------------------------------
        private async void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            //if (!TimerWork)
            //{
            //    TimerWork = true;

            if (IsReadAllRegisters)
            {
                IsReadAllRegisters = false;
                await Task.Run(() => StartRequestValue());
            }
            else
            {
                await Task.Run(() => RequestValue());

            }

            //TimerWork = false;
            EndRead?.Invoke(null, null);
            //}
            //else
            //    Debug.WriteLine("Пропуск таймера");

            timer.Start();
        }

        public abstract Task StartRequestValue();
        public abstract Task RequestValue();


    }
}
