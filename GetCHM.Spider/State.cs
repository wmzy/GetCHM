﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetCHM.Spider
{
    public enum State
    {
        New,
        Fetched,
        NotFound,
        Parsed,
        Saved
    }
}
