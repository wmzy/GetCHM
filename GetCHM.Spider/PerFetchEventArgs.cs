using System;

namespace GetCHM.Spider
{
    public class PerFetchEventArgs : EventArgs
    {
        public PerFetchEventArgs(Resource resource)
        {
            Resource = resource;
        }
        public Resource Resource { get; set; }
    }
}
