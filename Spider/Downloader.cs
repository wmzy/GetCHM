using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;

namespace Spider
{
    class Downloader : IDownloader
    {
        private readonly IRegistry _registry;
        public Downloader(IRegistry registry)
        {
            _registry = registry;
        }

        public void Start()
        {
            while (true)
            {
                try
                {
                    Uri uri = _registry.GetForDownload();
                    HttpWebRequest hwr = WebRequest.CreateHttp(uri);
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
