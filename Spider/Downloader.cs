using System;
using System.Collections.Generic;
using System.IO;
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
            List<Uri> urilList = _registry.GetNews();
            while (urilList.Count > 0)
            {
                try
                {
                    Parallel.ForEach(urilList, async (uri) =>
                    {
                        HttpWebRequest hwr = WebRequest.CreateHttp(uri);
                        var res = await hwr.GetResponseAsync();
                        if (Filter(res))
                        {
                            if (res.ContentType.Contains("text/html"))
                            {
                                var hDoc = new HtmlDocument();
                                hDoc.Load(res.GetResponseStream());
                                ParseUrl(hDoc);
                                hDoc.Save(Path.Combine(FilePath, uri.GetHashCode().ToString()));
                            }
                            else
                            {
                                using (var fs = File.OpenWrite(FilePath + uri.GetHashCode().ToString()))
                                {
                                    await res.GetResponseStream().CopyToAsync(fs);
                                }
                            }
                        }
                    });
                }
                catch (ArgumentNullException)
                {
                }
                catch (AggregateException)
                {
                }
            }
        }

        private void ParseUrl(HtmlDocument hDoc)
        {
            List<Uri> urls = new List<Uri>();
            string aQuery = "//a";
            string imgQuery = "//img@src";
            var elements = hDoc.DocumentNode.SelectNodes(aQuery);
            foreach (var elem in elements)
            {
            }
        }

        public Filter Filter = res => true;

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public string FilePath { get; set; }
    }
    
    delegate bool Filter(WebResponse res);
}
