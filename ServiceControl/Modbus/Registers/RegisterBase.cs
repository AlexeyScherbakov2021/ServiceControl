using ServiceControl.Based;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ServiceControl.Modbus.Registers
{
    public abstract class RegisterBase : Observable
    {
        public int Number { get; set; }
        public ushort Address { get; set; }

        private string _Name;
        public string Name { get => _Name; set { Set(ref _Name, value); } }
        public string NameRes;

        public string Description { get; set; }
        
        public ushort Size = 1;
        public ModbusFunc CodeFunc;

        private string _ValueString;
        public string ValueString { get => _ValueString; set { Set(ref _ValueString, value); } }

        public string ToolTip => $"{CodeFunc} 0x{Address:X2}";


        public virtual void ChangeLang() { }

        //-------------------------------------------------------------
        // Изменение языка
        //-------------------------------------------------------------
        public virtual void SetLanguage()
        {
            if (string.IsNullOrEmpty(NameRes))
                return;

            string name = App.Current.Resources[NameRes]?.ToString();
            if (!string.IsNullOrEmpty(name))
                Name = name;
        }

        //public abstract T SetOutput();

    }
}
