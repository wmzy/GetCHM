﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCHM.Spider
{
    public class ElementQuery
    {
        public string Query { get; set; }
        public string AttributeName { get; set; }
        public string Suffix { get; set; }
        public string OptionalSuffix { get; set; }
        public bool IsAutoIdentifySuffix { get; set; }
    }
}
