using System;
using HtmlAgilityPack;

namespace GetCHM.Spider
{
    public class PerFetchEventArgs : EventArgs
    {
        public PerFetchEventArgs(Resource resource)
        {
            Resource = resource;
        }
        public Resource Resource { get; set; }
    }

    public class DomLoadEventArgs : EventArgs
    {
        public DomLoadEventArgs(HtmlDocument htmlDocument)
        {
            HtmlDocument = htmlDocument;
        }

        public HtmlDocument HtmlDocument { get; set; }
    }
}
