using System;

namespace GetCHM.Spider
{
    [Serializable]
    public class ElementQuery
    {
        public string Query { get; set; }
        public string AttributeName { get; set; }
        public string Suffix { get; set; }
        public string OptionalSuffix { get; set; }
        public bool IsAutoIdentifySuffix { get; set; }
    }
}
