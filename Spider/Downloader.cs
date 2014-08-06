using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
            FilePath = @"D:\GetCHM\tem\";
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
            while (true)
            {
                var taskQueue = new Queue<Task>();
                Task t;
                while (_registry.HasNew)
                {
                    var url = _registry.PopNew();
                    t = FetchAsync(url);

                    taskQueue.Enqueue(t);
                }
                if (taskQueue.Count <= 0)
                {
                    break;
                }

                t = taskQueue.Dequeue();
                t.Wait(5000);
            }
        }

        private async Task FetchAsync(Uri uri)
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
                    var urlAttr = elem.Attributes[elementQuery.AttributeName];
                    if (urlAttr == null)
                        continue;
                    var url = urlAttr.Value;
                    if (FilterUrl(url))
                    {
                        var newUrl = new Uri(uri, url);
                        int index = url.IndexOf('#');
                        if (index > -1)
                        {
                            elem.Attributes[elementQuery.AttributeName].Value = newUrl.GetHashCode() +
                                                                                url.Substring(index);
                        }
                        else
                        {
                            elem.Attributes[elementQuery.AttributeName].Value = newUrl.GetHashCode().ToString();
                        }
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

        private string _fileName;

        public string FilePath
        {
            get { return _fileName; }

            set
            {
                if (!Directory.Exists(value))
                {
                    Directory.CreateDirectory(value);
                }
                _fileName = value;
            }
        }
    }

    public delegate bool Filter(WebResponse res);

    public delegate bool FilterUrl(string url);
}
