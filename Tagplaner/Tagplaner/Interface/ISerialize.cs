﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagplaner
{
    interface ISerialize
    {
        void SerializeObject(Object obj);
        MCalendar DeserializeObject();
    }
}
