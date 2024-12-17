using ServiceControl.Based;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.ViewModel
{

    internal class DumpRegWindowViewModel : Observable
    {
        public List<RegisterBase> listRegister { get; set; } = new List<RegisterBase>();


        public DumpRegWindowViewModel(List<RegisterBase> _listReg)
        {
            listRegister = _listReg;
        }
    }
}
