using System;

namespace Spider
{
    interface IRegistry
    {
        void Add(Uri uri);
        Uri GetForDownload();
        void WaitParse(Uri record);
        Uri GetForParse();
    }
}
