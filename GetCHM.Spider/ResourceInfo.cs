using System;
using System.Net.Http.Headers;
using HtmlAgilityPack;

namespace GetCHM.Spider
{
    public class ResourceInfo
    {
        public Uri Uri { get; set; }
        public string FileName { get; set; }
        public MediaTypeHeaderValue ContentType { get; set; }
        public HtmlDocument HtmlDocument { get; set; }
        public ElementQuery ElementQuery { get; set; }
        public int Depth { get; set; }
        public State State { get; set; }
    }
}
