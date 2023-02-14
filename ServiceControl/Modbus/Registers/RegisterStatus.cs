using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    [Flags]
    public enum RegStatus : ushort 
    { 
        SensorOpened = 0b000001,
        BlockOpened =  0b000010,
        BlockAbsent =  0b000100,
        BlockAlarm =    0b000110,
        DK1Break =     0b001000,
        DK2Break =     0b010000,
        DK3Break =     0b100000,
    }

    public enum BlockStatus { Good, Opened, Absent, Alarm };

    internal class RegisterStatus : RegisterInt
    {
        private bool _IsSensorOpen;
        public bool IsSensorOpen { get => _IsSensorOpen; set { Set(ref _IsSensorOpen, value); } }

        private bool _IsDK1Break;
        public bool IsDK1Break { get => _IsDK1Break; set { Set(ref _IsDK1Break, value); } }

        private bool _IsDK2Break;
        public bool IsDK2Break { get => _IsDK2Break; set { Set(ref _IsDK2Break, value); } }

        private bool _IsDK3Break;
        public bool IsDK3Break { get => _IsDK3Break; set { Set(ref _IsDK3Break, value); } }

        private BlockStatus _StatusBlock; 
        public BlockStatus StatusBlock { get => _StatusBlock; set { Set(ref _StatusBlock, value); } }


        public override void SetResultValues(ushort[] val)
        {
            if (val.Length < 1) return;
            Value = val[0];

            IsSensorOpen = (Value & (ushort)RegStatus.SensorOpened) != 0;

            int block = Value.Value & 0b000110;
            if (block == (int)RegStatus.BlockOpened)
                StatusBlock = BlockStatus.Opened;
            else if (block == (ushort)RegStatus.BlockAbsent)
                StatusBlock = BlockStatus.Absent;
            else if (block == (ushort)RegStatus.BlockAlarm)
                StatusBlock = BlockStatus.Alarm;
            else
                StatusBlock = BlockStatus.Good;

            IsDK1Break = (Value & (ushort)RegStatus.DK1Break) != 0;
            IsDK2Break = (Value & (ushort)RegStatus.DK2Break) != 0;
            IsDK3Break = (Value & (ushort)RegStatus.DK3Break) != 0;

            //return Value;

        }

    }
}
