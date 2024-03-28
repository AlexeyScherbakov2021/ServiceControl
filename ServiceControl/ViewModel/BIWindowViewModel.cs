using ServiceControl.Based;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.ViewModel
{
    internal class BIWindowViewModel : Observable
    {
        public class Reg10BI
        {
            public Register[] reg { get; set; } = new Register[Device356.CountBI];
        }

        //---------------------------------------------------------------------

        public List<Reg10BI> ListBIRegisters { get; set; }


        public BIWindowViewModel()
        {
        }

        public BIWindowViewModel(Device356 device)
        {
            ListBIRegisters = new List<Reg10BI>();
            Reg10BI[] listReg = new Reg10BI[10];
            for (int i = 0; i < 10; i++)
                listReg[i] = new Reg10BI();

            for (int i = 0; i < Device356.CountBI; i++)
            {
                listReg[0].reg[i] = device.SpeedDK[i];
                listReg[1].reg[i] = device.DeepDK[i];
                listReg[2].reg[i] = device.BI_SummPot[i];
                listReg[3].reg[i] = device.BI_PolPot[i];
                listReg[4].reg[i] = device.BI_OutCurrent[i];
                listReg[5].reg[i] = device.BI_OutVoltage[i];
                listReg[6].reg[i] = device.BI_CurrPol[i];
                listReg[7].reg[i] = device.BI_IndVoltage[i];
                listReg[8].reg[i] = device.BI_FreqVoltage[i];
                listReg[9].reg[i] = device.BI_Temper[i];
            }

            for(int i = 0; i < 10; i++)
                ListBIRegisters.Add(listReg[i]);

        }
    }
}
