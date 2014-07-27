using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Spider
{
    class Downloader : IDownloader
    {
        public Downloader()
        {
        }
        public void Download(string url)
        {
            HttpWebRequest hwr = WebRequest.CreateHttp(url);
        }
    }
}
