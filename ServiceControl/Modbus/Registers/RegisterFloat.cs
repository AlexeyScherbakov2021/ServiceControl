using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterFloat : Register
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
                if (Set(ref _ValueDouble, value))
                {
                }
            }
        }
        public override void SetResultValues(ushort[] val)
        {
            if(val == null || val.Length != 2)
            {
                Value = null;
                return;
            }

            byte[] values = new byte[4];

            values[0] = (byte)(val[0] & 0xFF);
            values[1] = (byte)(val[0] >> 8);
            values[2] = (byte)(val[1] & 0xFF);
            values[3] = (byte)(val[1] >> 8);
            Value = BitConverter.ToSingle(values, 0);
            Value *= Scale;
            if (Value > MaxValue || Value < MinValue) Value = null;
        }

        public override ushort[] SetOutput()
        {
            if (Value == null)
                return null;
            float val = (float)Value / Scale;
            byte[] values = BitConverter.GetBytes(val);

            ushort[] res = new ushort[2];
            res[0] = (ushort)((ushort)(values[1] << 8) | values[0]);
            res[1] = (ushort)((ushort)(values[3] << 8) | values[2]);

            return res;
        }

        public override void ChangeLang()
        {
            Name = App.Current.Resources[NameRes].ToString();
        }

    }
}
