﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Based
{
    public enum ListStep { Stepping, NotSteping };
    public class ListR<T> : List<T>
    {
        public ListStep Stepping;
    }
}
