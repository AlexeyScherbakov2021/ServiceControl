using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    public enum StatusMS { On, Off, Absent, Avar };

    internal class RegisterMS : RegisterInt
    {
        //private string _ValueString;
        //public string ValueString { get => _ValueString; set { Set(ref _ValueString, value); } }
        //public StatusMS Status { get; set; }

        public override void SetResultValues(ushort[] val)
        {
            base.SetResultValues(val);

            if (Value == null) return;

            switch ((StatusMS)Value)
            {
                case StatusMS.On:
                    ValueString = "00 - включен";
                    break;

                case StatusMS.Off:
                    ValueString = "01 - выключен";
                    break;

                case StatusMS.Absent:
                    ValueString = "02 - отсутствует";
                    break;

                case StatusMS.Avar:
                    ValueString = "03 - авария";
                    break;
            }


            //return (ushort)result;
        }

    }
}
