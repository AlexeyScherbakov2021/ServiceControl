using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterFloat : Register //, IDataErrorInfo
    {
        public float Scale = 1;
        public float MinValue = float.MinValue;
        public float MaxValue = float.MaxValue;

        private float? _ValueDouble;
        public float? Value 
        { 
            get => _ValueDouble;
            set
            {
                if(Set(ref _ValueDouble, value))
                {
                       
                }
            }
        }

        //public string Error => null;

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        if (columnName == "Value")
        //        {

        //            if (Value == null)
        //                return "NULL";

        //        }
        //        return null;
        //    }
        //}




        public override void SetResultValues(ushort[] val)
        {
            if (val == null || val.Length < 1)
            {
                Value = null;
                return;
            }

            int res = (short)val[0];
            for(int i = 1; i < val.Length; i++)
            {
                int res2 = val[i];
                res2 <<= 16 * i;
                res |= (int)(short)val[i];
            }
            Value = (int)res * Scale;
            if (Value > MaxValue || Value < MinValue) Value = null;
        }

        public override ushort[] SetOutput()
        {
            ushort[] res = new ushort[Size];
            int val = (int)(Value / Scale);

            for (int i = Size - 1; i >= 0; i--)
            {
                res[i] = (ushort)val;
                val >>= 16;
            }
            return res;
        }

        public override void ChangeLang() 
        { 
            Name = App.Current.Resources[NameRes].ToString();
        }

    }
}
