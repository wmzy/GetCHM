using System;
using System.Collections.Generic;

namespace Spider
{
    public interface IRegistry
    {
        //Record Add(Uri uri);
        Record Add(Uri uri, string suffix = null, string fileName = null);
        bool HasNew { get; }
        Record PopNew();
        Record FindByUri(Uri uri);
        IEnumerable<Record> Records { get; } 
    }
}
