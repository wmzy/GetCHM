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
    public class Downloader : IDownloader
    {
        private readonly IRegistry _registry;

        public Downloader()
        {
            _registry = Registry.Instance;
            FilePath = @"D:\";
            ElementQueries = new List<ElementQuery>()
            {
                new ElementQuery() {Query = "//a", AttributeName = "href"},
                new ElementQuery() {Query = "//img", AttributeName = "src"}
            };
        }
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
                        using (var res = await hwr.GetResponseAsync())
                        {
                            if (Filter(res))
                            {
                                if (res.ContentType.Contains("text/html"))
                                {
                                    var hDoc = new HtmlDocument();
                                    using (var resStream = res.GetResponseStream())
                                    {
                                        hDoc.Load(resStream);
                                    }
                                    ReplaceUrl(hDoc, uri);
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
                        }
                    });
                }
                catch (ArgumentNullException)
                {
                }
                catch (AggregateException)
                {
                }
                urilList = _registry.GetNews();
            }
        }

        public List<ElementQuery> ElementQueries { get; set; }

        private void ReplaceUrl(HtmlDocument hDoc, Uri uri)
        {
            foreach (var elementQuery in ElementQueries)
            {
                var elements = hDoc.DocumentNode.SelectNodes(elementQuery.Query);
                if (elements == null)
                    continue;
                foreach (var elem in elements)
                {
                    var url = elem.Attributes[elementQuery.AttributeName].Value;
                    if (FilterUrl(url))
                    {
                        var newUrl = new Uri(uri, url);
                        elem.Attributes[elementQuery.AttributeName].Value = newUrl.GetHashCode() + url.Substring(url.IndexOf('#'));
                        _registry.Add(newUrl);
                    }
                }
            }
        }

        public Filter Filter = res => true;
        public FilterUrl FilterUrl = url => true;

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

    public delegate bool Filter(WebResponse res);

    public delegate bool FilterUrl(string url);
}
