using System;
using System.Collections.Generic;

namespace Spider
{
    public interface IRegistry
    {
        void Add(Uri uri);
        List<Uri> GetNews();
    }
}
