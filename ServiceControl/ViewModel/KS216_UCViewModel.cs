using ServiceControl.Commands;
using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using ServiceControl.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ServiceControl.ViewModel
{
    internal class KS216_UCViewModel : Observable
    {
        public class IKP
        {
            public RegisterDouble SpeedIKP { get; set; }
            public RegisterDouble DeepIKP { get; set; }
        }


        private bool _IsOn;
        public bool IsOn 
        {
            get => _IsOn;
            set
            {
                if(Set(ref _IsOn, value))
                {
                    if (device.OnOffMS.ValueBool != _IsOn)
                    {
                        device.OnOffMS.ValueBool = _IsOn;
                        OnOnOffCommandExecuted(null);
                    }
                }
            }
        }

        public Device216 device { get; set; }

        public List<Register> ListInput { get; set; }
        public List<Register> ListInputMS { get; set; }
        public List<IKP> ListInputDK { get; set; }
        public List<RegisterBool> ListStatus { get; set; }
        public List<RegisterBool> ListCoil { get; set; }
        public List<Register> ListWriteControl { get; set; }
        public List<Register> ListService { get; set; }

        public string DeviceInfo =>
            "Версия платы " + device?.InfoReg.VersionDev
            + "; Верстия ПО " + device?.InfoReg.VersionPO
            + "; Год " + device?.InfoReg.Year
            + "; № " + device?.InfoReg.NumberDev;

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS216_UCViewModel()
        {
        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS216_UCViewModel(MbWork work, int Slave)
        {
            device = new Device216(work, Slave);
            device.InfoReg.PropertyChanged += InfoReg_PropertyChanged;

            device.EndStartRead += OnStartFinish;

            device.Start();

            ListInput = new List<Register>()
            {
                device.NaprSeti1, device.CountEE1, device.NaprSeti2, device.CountEE2, device.Temper, device.TimeWork,
                device.TimeProtect, device.CurrOutput, device.NaprOutput, device.ProtectPotenSumm, device.ProtectPotenPol
            };

            ListInputMS = new List<Register>();
            for (int i = 0; i < Device216.CountMS; i++)
                ListInputMS.Add(device.MS[i]);

            ListInputDK = new List<IKP>();
            for (int i = 0; i < Device216.CountDK; i++)
            {
                ListInputDK.Add( new IKP() { SpeedIKP = device.SpeedDK[i], DeepIKP = device.DeepDK[i] });
                //ListInputDK.Add(device.DeepDK[i]);
            }

            ListStatus = new List<RegisterBool>() { device.IllegalAccess, device.ControlMode, device.Fault, device.BreakCirc, device.OnMS, 
                device.SpeedCorr1, device.SpeedCorr2, device.SpeedCorr3 };

            ListWriteControl = new List<Register>() { device.SetCurrOutput, device.SetSummPotOutput, device.SetPolPotOutput, device.SetNaprOutput };
            ListCoil = new List<RegisterBool>() { device.OnOffMS };
            ListService = new List<Register>() { device.TempCoolerOn, device.TempCoolerOff, device.ModeNaprOutput };

        }

        private void OnStartFinish(object sender, EventArgs e)
        {
            IsOn = device.OnOffMS.ValueBool;
        }

        private void InfoReg_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DeviceInfo));
        }

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
                    device.WriteRegister(device.SetCurrOutput);
                    break;
                case 0x82:
                    device.WriteRegister(device.SetSummPotOutput);
                    break;
                case 0x83:
                    device.WriteRegister(device.SetPolPotOutput);
                    break;
                case 0x85:
                    device.WriteRegister(device.SetNaprOutput);
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
            device.WriteRegister(device.SetMode);
        }

        //--------------------------------------------------------------------------------
        // Команда Вкл - откл силовых модулей
        //--------------------------------------------------------------------------------
        //public ICommand OnOffCommand => new LambdaCommand(OnOnOffCommandExecuted);
        private void OnOnOffCommandExecuted(object p)
        {
            device.WriteRegister(device.OnOffMS);
        }

        #endregion

    }
}
