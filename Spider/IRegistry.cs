using System;
using System.Collections.Generic;

namespace Spider
{
    interface IRegistry
    {
        void Add(Uri uri);
        List<Uri> GetNews();
    }
}
