using System;
using System.Collections.Generic;

namespace Spider
{
    public interface IRegistry
    {
        void Add(Uri uri);
        bool HasNew { get; }
        Uri PopNew();
    }
}
