using ServiceControl.Based;
using ServiceControl.Commands;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ServiceControl.ViewModel
{
    internal class BAVR_UCViewModel : Observable
    {
        public List<Register> ListInput { get; set; }
        public List<Register> ListWrite { get; set; }
        public List<RegisterBool> ListStatus { get; set; }
        public List<RegisterBool> ListMode { get; set; }

        public DeviceBAVR device { get; set; }

        private bool _isAutoMode;
        public bool isAutoMode { get => _isAutoMode; set { Set(ref _isAutoMode, value); } }

        private bool _isMainMode;
        public bool isMainMode { get => _isMainMode; set { Set(ref _isMainMode, value); } }

        private bool _isSecondMode;
        public bool isSecondMode { get => _isSecondMode; set { Set(ref _isSecondMode, value); } }


        public BAVR_UCViewModel()
        {
        }

        public BAVR_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {
            device = new DeviceBAVR(mainViewModel, work, Slave);
            device.EndStartRead += Device_EndStartRead;
            device.Start();

            ListInput = new List<Register>()
            {
                device.Temper, device.VoltageAKB, device.NumberCounter, device.Power,
                device.Uvalue, device.Ivalue, device.Pvalue
            };

            ListWrite = new List<Register>
            {
                device.AddressBAVRSave, device.NumberCounterSave
            };

            ListStatus = new List<RegisterBool>() { device.EndingAvr, device.isAKB };
            ListMode = new List<RegisterBool>() { device.ModeAuto, device.ModeMain, device.ModeSecond };

        }

        private void Device_EndStartRead(object sender, EventArgs e)
        {
            isAutoMode = (device.ModeSave.Value.Value & 1) == 1;
            isMainMode = (device.ModeSave.Value.Value & 2) == 2;
            isSecondMode = (device.ModeSave.Value.Value & 4) == 4;
            //device.EndStartRead -= Device_EndStartRead;
        }

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
                }
                catch (TimeoutException)
                {

                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Отправить Режим
        //--------------------------------------------------------------------------------
        public ICommand WriteModeCommand => new LambdaCommand(OnWriteModeCommandExecuted, CanWriteModeCommand);
        private bool CanWriteModeCommand(object p) => device != null;
        private void OnWriteModeCommandExecuted(object p)
        {

            device.ModeSave.Value = isAutoMode ? 1 : 0;
            device.ModeSave.Value |= isMainMode ? 2 : 0;
            device.ModeSave.Value |= isSecondMode ? 4 : 0;

            try
            {
                    device.WriteRegister(device.ModeSave);
                }
                catch (TimeoutException)
                {

                }
        }

    }
}
