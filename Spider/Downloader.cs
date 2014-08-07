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
                new ElementQuery() {Query = "//a", AttributeName = "href", Suffix = ".html"},
                new ElementQuery() {Query = "//img", AttributeName = "src"},
                new ElementQuery() {Query = @"//script[@type='text/javascript']", AttributeName = "src", Suffix = ".js"},
                new ElementQuery() {Query = @"//link[@rel='stylesheet']", AttributeName = "href", Suffix = ".css"}
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
                    var record = _registry.PopNew();
                    t = FetchAsync(record);

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

        private async Task FetchAsync(Record record)
        {
            HttpWebRequest hwr = WebRequest.CreateHttp(record.Uri);
            hwr.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
            hwr.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var res = await hwr.GetResponseAsync())
            {
                if (Filter(res))
                {
                    if (res.ContentType.Contains("text/html"))
                    {
                        var hDoc = new HtmlDocument();
                        using (var resStream = res.GetResponseStream())
                        {
                            hDoc.Load(resStream, Encoding.UTF8);
                        }
                        ReplaceUrl(hDoc, record.Uri);
                        hDoc.Save(Path.Combine(FilePath, record.FileName));
                    }
                    else
                    {
                        using (var fs = File.OpenWrite(FilePath + record.FileName))
                        {
                            var responseStream = res.GetResponseStream();
                            if (responseStream != null) await responseStream.CopyToAsync(fs);
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
                        string suffix = string.IsNullOrWhiteSpace(elementQuery.Suffix) ? GetSuffixFromUrl(url) : elementQuery.Suffix;
                        var newUrl = new Uri(uri, url);
                        var record = _registry.Add(newUrl, suffix);
                        int index = url.IndexOf('#');
                        elem.Attributes[elementQuery.AttributeName].Value = index > -1
                            ? record.FileName +
                              url.Substring(index)
                            : record.FileName;
                    }
                }
            }
        }

        private string GetSuffixFromUrl(string url)
        {
            int queryIndex = url.IndexOf('?');
            if (queryIndex > -1)
            {
                int suffixIndex = url.LastIndexOf('.', queryIndex);
                return suffixIndex > -1 ? url.Substring(suffixIndex, queryIndex - suffixIndex) : null;
            }
            else
            {
                int suffixIndex = url.LastIndexOf('.');
                return suffixIndex > -1 ? url.Substring(suffixIndex) : null;
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
