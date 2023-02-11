﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Modbus.Registers
{
    internal class RegisterNapr4896 : RegisterInt
    {

        public override void SetResultValues(ushort[] val)
        {
            if (val.Length < 1) return;
            ValueInt = val[0];
            ValueString = ValueInt == 0 ? "48 В" : "96 В";
        }

    }
}