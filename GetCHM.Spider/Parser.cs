using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace GetCHM.Spider
{
    public class Parser
    {
        private readonly IEnumerable<ElementQuery> _elementQueries;
        public FilterUrl FilterUrl = url => true;
        public Parser(IEnumerable<ElementQuery> elementQueries)
        {
            _elementQueries = elementQueries;
        }

        public void Parse(ResourceInfo[] resources, out List<HtmlAttribute> urlAttributesToUpdate, out List<Uri> urlToFetch)
        {
            urlAttributesToUpdate = new List<HtmlAttribute>();
            urlToFetch = new List<Uri>();

            foreach (var resource in resources)
            {
                if (resource.HtmlDocument != null)
                {
                    // 获取文档base url
                    Uri baseUrl;
                    var baseElem = resource.HtmlDocument.DocumentNode.SelectSingleNode("//head/base");
                    if (baseElem != null)
                    {
                        var href = baseElem.GetAttributeValue("href", null);
                        baseUrl = href != null ? new Uri(resource.Uri, href) : resource.Uri;
                    }
                    else
                    {
                        baseUrl = resource.Uri;
                    }
                    ReplaceRelativeUrl(resource.HtmlDocument, baseUrl);

                    foreach (var elementQuery in _elementQueries)
                    {
                        var elements = resource.HtmlDocument.DocumentNode.SelectNodes(elementQuery.Query);
                        if (elements == null)
                            continue;
                        foreach (var elem in elements)
                        {
                            var urlAttr = elem.Attributes[elementQuery.AttributeName];
                            if (urlAttr == null || string.IsNullOrWhiteSpace(urlAttr.Value))
                                continue;
                            if (!FilterUrl(urlAttr.Value)) continue;

                            var absoluteUrl = new Uri(baseUrl, urlAttr.Value);
                            if (!absoluteUrl.Scheme.StartsWith("http", StringComparison.OrdinalIgnoreCase)) continue;

                            urlAttr.Value = absoluteUrl.AbsoluteUri;

                            if (Repository.Instance.IsExist(absoluteUrl))
                            {
                                var name = Repository.Instance.GetByKey(absoluteUrl).FileName;
                                if (!string.IsNullOrEmpty(name))
                                {
                                    urlAttr.Value = name;
                                }
                                else
                                {
                                    urlAttributesToUpdate.Add(urlAttr);
                                }
                            }
                            else
                            {
                                urlToFetch.Add(absoluteUrl);
                                urlAttributesToUpdate.Add(urlAttr);
                                Repository.Instance.Add(absoluteUrl, resource.Depth + 1, elementQuery);
                            }
                        }
                    }
                }
            }
        }

        public void ReplaceRelativeUrl(HtmlDocument html, Uri baseUrl)
        {
            // todo:替换相对url
        }
    }
    public delegate bool FilterUrl(string url);
}
