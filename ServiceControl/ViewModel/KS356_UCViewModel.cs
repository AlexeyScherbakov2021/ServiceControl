using ServiceControl.Commands;
using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ServiceControl.ViewModel
{
    internal class KS356_UCViewModel: Observable
    {
        public Device356 device { get; set; }
        public List<Register> ListInput { get; set; }
        public List<Register> ListInputMS { get; set; }
        public List<Register> ListInputDK { get; set; }
        public List<RegisterBool> ListStatus { get; set; }
        public List<RegisterBool> ListCoil { get; set; }
        public List<Register> ListWriteControl { get; set; }
        public List<Register> ListWriteControlInt { get; set; }
        public List<Register> ListService { get; set; }

        public string DeviceInfo =>
            "Версия " + device?.InfoReg.VersionDev
            + "; Верстия ПО " + device?.InfoReg.VersionPO
            + "; Год " + device?.InfoReg.Year
            + "; № " + device?.InfoReg.NumberDev;


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS356_UCViewModel()
        {

        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS356_UCViewModel(MbWork work, int Slave)
        {
            device = new Device356(work, Slave);
            device.InfoReg.PropertyChanged += InfoReg_PropertyChanged;
            device.Start();

            ListInput = new List<Register>()
            {
                device.NaprSeti1, device.CountEE1, device.NaprSeti2, device.CountEE2, device.Temper, device.TimeWork,
                device.TimeProtect, device.CurrOutput, device.NaprOutput, device.ProtectPotenSumm, device.ProtectPotenPol
            };

            ListInputMS = new List<Register>();
            for(int i = 0; i < Device356.CountMS; i++)
                ListInputMS.Add(device.MS[i]);


            //{ device.MS1, device.MS2, device.MS3, device.MS4, device.MS5, device.MS6, device.MS7,
            //    device.MS8, device.MS9, device.MS10, device.MS11, device.MS12 };

            ListInputDK = new List<Register>();
            for (int i = 0; i < Device356.CountDK; i++)
            {
                ListInputDK.Add(device.SpeedDK[i]);
                ListInputDK.Add(device.DeepDK[i]);
            }

            //{
            //    device.SpeedDK1,device.DeepDK1,device.SpeedDK2,device.DeepDK2,device.SpeedDK3,device.DeepDK3,device.SpeedDK4,device.DeepDK4,
            //    device.SpeedDK5,device.DeepDK5,device.SpeedDK6,device.DeepDK6,device.SpeedDK7,device.DeepDK7,device.SpeedDK8,device.DeepDK8
            //};

            ListStatus = new List<RegisterBool>() { device.IllegalAccess, device.ControlMode, device.Fault, device.BreakCirc, device.OnMS,
                device.SpeedCorr1, device.SpeedCorr2, device.SpeedCorr3 };

            ListWriteControl = new List<Register>() { device.SetCurrOutput, device.SetSummPotOutput, device.SetPolPotOutput, 
                device.SetNaprOutput};
            
            ListWriteControlInt = new List<Register>() { device.WorkedTime, device.ProtectTime };

            ListCoil = new List<RegisterBool>() { device.OnOffMS };
            ListService = new List<Register>() {
                device.TempCoolerOn, device.TempCoolerOff, device.Year, device.Number, device.ModeNaprOutput, device.TimeProtect,
                device.WorkedTime
            };

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
                case 0xC7:
                    device.WriteRegister(device.WorkedTime);
                    break;
                case 0xC9:
                    device.WriteRegister(device.ProtectTime);
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

        #endregion

    }
}
