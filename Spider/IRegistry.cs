using System;
using System.Collections.Generic;

namespace Spider
{
    public interface IRegistry
    {
        //Record Add(Uri uri);
        Resource Add(Uri uri, string suffix = null, string fileName = null);
        bool HasNew { get; }
        Resource PopNew();
        Resource FindByUri(Uri uri);
        IEnumerable<Resource> Records { get; }
    }
}
