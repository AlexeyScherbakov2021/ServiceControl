using ServiceControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ServiceControl.Modbus.Registers
{
    internal abstract class Register : RegisterBase
    {
        private string _Measure;
        public string Measure { get => _Measure; set { Set(ref _Measure, value); } }

        public string MeasureRes;

        public abstract void SetResultValues(ushort[] val);

        public abstract ushort[] SetOutput();


        //-------------------------------------------------------------
        // Изменение языка
        //-------------------------------------------------------------
        public override void SetLanguage()
        {
            base.SetLanguage();

            if (!string.IsNullOrEmpty(MeasureRes))
            {
                string name = App.Current.Resources[MeasureRes]?.ToString();
                if (!string.IsNullOrEmpty(name))
                    Measure = name;
            }
        }

    }
}
