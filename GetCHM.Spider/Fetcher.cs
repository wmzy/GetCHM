﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WMZY.Util;

namespace GetCHM.Spider
{
    public class Fetcher
    {
        private readonly HttpClient _httpClient;
        private string _savePath;

        #region events
        //public event EventHandler<PerFetchEventArgs> PerFetch;
        //protected virtual void OnPerFetch(PerFetchEventArgs e)
        //{
        //    EventHandler<PerFetchEventArgs> handler = PerFetch;
        //    if (handler != null) handler(this, e);
        //}

        public event EventHandler<DomLoadEventArgs> DomLoad;
        protected virtual void OnDomLoad(DomLoadEventArgs e)
        {
            EventHandler<DomLoadEventArgs> handler = DomLoad;
            if (handler != null) handler(this, e);
        }
        #endregion

        public string SaveSavePath
        {
            get { return _savePath; }
            set
            {
                if (!Directory.Exists(value))
                {
                    Directory.CreateDirectory(value);
                }
                _savePath = value;
            }
        }

        #region .ctor

        public Fetcher()
        {
            _httpClient = new HttpClient();
        }

        public Fetcher(HttpClient httpClient, string savePath)
        {
            _httpClient = httpClient;
            SaveSavePath = savePath;
        }

        #endregion

        public async Task<ResourceInfo> FetchAsync(Uri url)
        {
            var resource = new ResourceInfo
            {
                Uri = url,
                State = State.New
            };
            await FetchAsync(resource);

            return resource;
        }

        public async Task FetchAsync(ResourceInfo resource)
        {
            var response = await _httpClient.GetAsync(resource.Uri);
            if (!response.IsSuccessStatusCode)
            {
                resource.State = State.NotFound;
                return;
            }
            resource.FileName = Guid.NewGuid().ToString("N");


            resource.ContentType = response.Content.Headers.ContentType;
            if (IsHtml(response.Content))
            {
                resource.HtmlDocument = await GetHtmlAsync(response.Content);
                OnDomLoad(new DomLoadEventArgs(resource.HtmlDocument));
                resource.FileName += ".html";
                resource.State = State.Fetched;
            }
            else
            {
                string suffix;
                if (resource.ElementQuery == null)
                {
                    suffix = MimeMapping.ConvertMimeTypeToExtension(resource.ContentType.MediaType);
                }
                else
                {
                    suffix =
                        resource.ElementQuery.Suffix.NullOrEmptyDefault(
                            MimeMapping.ConvertMimeTypeToExtension(resource.ContentType.MediaType)
                                .NullOrWhiteSpaceDefault(resource.ElementQuery.OptionalSuffix));
                }
                resource.FileName += suffix;
                using (var fs = File.OpenWrite(Path.Combine(SaveSavePath, resource.FileName)))
                {
                    await response.Content.CopyToAsync(fs);
                }
                resource.State = State.Saved;
            }
        }

        private static bool IsHtml(HttpContent content)
        {
            try
            {
                return content.Headers.ContentType.MediaType.ToLower().Contains("htm");
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task<HtmlDocument> GetHtmlAsync(HttpContent content)
        {
            if (IsHtml(content))
            {
                var html = new HtmlDocument();
                var stream = await content.ReadAsStreamAsync();
                html.Load(stream,
                    Encoding.GetEncoding(content.Headers.ContentType.CharSet.NullOrWhiteSpaceDefault("UTF-8")));
                if (html.ParseErrors.All(e => e.Code != HtmlParseErrorCode.CharsetMismatch))
                    return html;
            }

            return null;
        }
        public async Task<ResourceInfo[]> FetchAsync(Uri[] urls)
        {
            Task[] tasks = new Task[urls.Length];
            ResourceInfo[] resources = new ResourceInfo[urls.Length];
            for (int i = 0; i < urls.Length; i++)
            {
                resources[i] = Repository.Instance.GetByKey(urls[i]);
                tasks[i] = FetchAsync(resources[i]);
            }
            await Task.WhenAll(tasks);
            return resources;
        }
    }
}
