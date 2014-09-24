using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GetCHM.Spider
{
    public class Fetcher
    {
        private HttpClient _httpClient;
        public Uri Uri { get; set; }

        protected HttpClient HttpClient
        {
            get { return _httpClient; }
            set { _httpClient = value; }
        }

        #region .ctor

        public Fetcher(Uri uri)
        {
            Uri = uri;
        }

        public Fetcher(string uri)
        {
            Uri = new Uri(uri);
        } 
        #endregion

        public async Task<ResourceInfo> FetchAsync()
        {
            var response = await HttpClient.GetAsync(Uri);
            return new ResourceInfo
            {
                ContentType = response.Content.Headers.ContentType
            };
        }
    }
}
