using System;
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
            resource.FileName = Guid.NewGuid().ToString("N") +
                                MimeMapping.ConvertMimeTypeToExtension(response.Content.Headers.ContentType.MediaType);

            if (IsHtml(response.Content))
            {
                resource.HtmlDocument = await GetHtmlAsync(response.Content);
                resource.State = State.Fetched;
            }
            else
            {
                using (var fs = File.OpenWrite(Path.Combine(SaveSavePath, resource.FileName)))
                {
                    await response.Content.CopyToAsync(fs);
                }
                resource.State = State.Saved;
            }
            resource.ContentType = response.Content.Headers.ContentType;
        }

        private static bool IsHtml(HttpContent content)
        {
            try
            {
                return content.Headers.ContentType.MediaType.ToLower().Contains("html");
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

        public async Task FetchAsync(Uri[] urls)
        {
            Task[] tasks = new Task[urls.Length];
            for (int i = 0; i < urls.Length; i++)
            {
                tasks[i] = FetchAsync(Repository.Instance.GetByKey(urls[i]));
            }
            foreach (var task in tasks)
            {
                await task;
            }
        }
    }
}
