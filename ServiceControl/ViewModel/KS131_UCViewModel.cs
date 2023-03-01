using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.ViewModel
{
    internal class KS131_UCViewModel : Observable
    {
        public class TwoRegister
        {
            public Register Reg1 { get; set; }
            public Register Reg2 { get; set; }
        }


        public Device131 device { get; set; }

        public List<TwoRegister> ListInfoReg { get; set; }


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KS131_UCViewModel()
        {

        }

        public KS131_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {
            device = new Device131(mainViewModel, work, Slave);

            ListInfoReg = new List<TwoRegister>()
            {
                new TwoRegister { Reg1 = device.CurrOutput, Reg2 = device.SetCurrOutput},
                new TwoRegister { Reg1 = device.VoltOutput, Reg2 = device.SetPotOutput},
                new TwoRegister { Reg1 = device.Potencial, Reg2 = device.SetPotOutput},
                new TwoRegister { Reg1 = device.Potencial, Reg2 = device.SetPotOutput},
            };

            device.Start();
        }

    }
}
