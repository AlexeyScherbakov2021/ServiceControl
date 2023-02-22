using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterKIP : Register
    {
        public float Scale = 1;
        public float MinValue = float.MinValue;
        public float MaxValue = float.MaxValue;

        private float? _ValueDouble;
        public float? Value { get => _ValueDouble; set { Set(ref _ValueDouble, value); } }

        private float? _TempValue;


        public override void SetResultValues(ushort[] val)
        {
            if (val == null || val.Length < 1)
            {
                _TempValue = null;
                return;
            }

            int res = (short)val[0];
            for (int i = 1; i < val.Length; i++)
            {
                int res2 = val[i];
                res2 <<= 16 * i;
                res |= (int)(short)val[i];
            }
            _TempValue = (int)res * Scale;
            if (_TempValue > MaxValue || _TempValue < MinValue) _TempValue = null;

        }

        public void SetValueFromStatus(bool status)
        {
            if(status)
                Value = _TempValue;
        }


        public override void ChangeLang()
        {
            Name = App.Current.Resources[NameRes].ToString();
        }


        public override ushort[] SetOutput()
        {
            throw new NotImplementedException();
        }

    }
}
