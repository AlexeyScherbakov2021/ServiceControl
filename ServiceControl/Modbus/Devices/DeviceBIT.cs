using ServiceControl.Modbus.Registers;
using ServiceControl.ViewModel;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Devices
{
    internal class DeviceBIT : DeviceSlave
    {
        public const float defK1 = 0;
        public const float defK2 = 0.1f;
        public const float defK3 = 1;

        public RegisterFloat Gauss;
        public RegisterFloat Gauss1;
        public RegisterFloat Gauss2;
        public RegisterFloat Amper;
        public RegisterFloat Celsius;
        //public RegisterFloat X1;
        //public RegisterFloat X2;
        //public RegisterFloat Y2;
        //public RegisterFloat Y1;
        //public RegisterFloat Z1;
        //public RegisterFloat Z2;
        //public RegisterFloat T1;
        //public RegisterFloat T2;

        public RegisterInt Address;
        public RegisterFloat K1;        // по умолчанию 0
        public RegisterFloat K2;        // по умолчанию 0.1
        public RegisterFloat K3;        // по умолчанию 1
        //public RegisterFloat K4;

        List<Register> ListInput;
        List<Register> ListCommandHolding;

        public DeviceBIT(MainWindowViewModel vm, MbWork modb, int slave) : base(vm, modb, slave)
        {
            modb.master.isWriteNoAnswer = true;

            // список входных регистров
            //--------------------------------------------------------------------------------------------------------------------------------------
            ListInput = new List<Register>();

            Gauss = new RegisterFloat()
            {
                Address = 0x0,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Коррекция нуля тока",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Description = "",
                Scale = 1f,
                MinValue = -1000,
                MaxValue = 1000
            };
            ListInput.Add(Gauss);

            Gauss1 = new RegisterFloat()
            {
                Address = 0x02,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Gauss1",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Description = "",
                Scale = 1f,
                MinValue = -1000,
                MaxValue = 1000
            };
            ListInput.Add(Gauss1);

            Gauss2 = new RegisterFloat()
            {
                Address = 0x04,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Gauss2",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Description = "",
                Scale = 1f,
                MinValue = -1000,
                MaxValue = 1000
            };
            ListInput.Add(Gauss2);

            Amper = new RegisterFloat()
            {
                Address = 0x06,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Ток",
                NameRes = "",
                Measure = "А",
                MeasureRes = "",
                Description = "",
                Scale = 1f,
                MinValue = -1000,
                MaxValue = 1000
            };
            ListInput.Add(Amper);

            Celsius = new RegisterFloat()
            {
                Address = 0x08,
                CodeFunc = ModbusFunc.InputRegister,
                Size = 2,
                Name = "Температура",
                NameRes = "",
                Measure = "°C",
                MeasureRes = "",
                Description = "",
                Scale = 1f,
                MinValue = -1000,
                MaxValue = 1000
            };
            ListInput.Add(Celsius);


            ListCommandHolding = new List<Register>();

            Address = new RegisterInt()
            {
                Address = 0x00,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 1,
                Name = "Установка адреса",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Description = "",
                MinValue = 0,
                MaxValue = 100
            };
            ListCommandHolding.Add(Address);

            K1 = new RegisterFloat()
            {
                Address = 0x01,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 2,
                Name = "K1",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Description = "K1",
                Scale = 1f,
                MinValue = -1000,
                MaxValue = 1000
            };
            ListCommandHolding.Add(K1);

            K2 = new RegisterFloat()
            {
                Address = 0x03,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 2,
                Name = "K2",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Description = "K2",
                Scale = 2000f,
                MinValue = -1000,
                MaxValue = 1000
            };
            ListCommandHolding.Add(K2);

            K3 = new RegisterFloat()
            {
                Address = 0x05,
                CodeFunc = ModbusFunc.HoldingRegister,
                Size = 2,
                Name = "K3",
                NameRes = "",
                Measure = "",
                MeasureRes = "",
                Description = "K3",
                Scale = 1f,
                MinValue = -1000,
                MaxValue = 1000
            };
            ListCommandHolding.Add(K3);

        }

        public override Task RequestValue()
        {
            ReadRegisters(ListInput);
            return Task.CompletedTask;
        }

        public override Task StartRequestValue()
        {
            ReadRegisters(ListInput);
            ReadRegisters(ListCommandHolding);
            return Task.CompletedTask;
        }

        protected override void CheckListRegister()
        {
            CheckReg(ListInput);
            CheckReg(ListCommandHolding);
        }
    }
}
