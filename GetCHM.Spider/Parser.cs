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

        public IList<HtmlAttribute> Parse(List<ResourceInfo> resources)
        {
            IList<HtmlAttribute> results = new List<HtmlAttribute>();
            foreach (var resource in resources)
            {
                if (resource.HtmlDocument != null)
                {
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
                            var absoluteUrl = new Uri(resource.Uri, urlAttr.Value);
                            string name;
                            if (Repository.Instance.IsExist(absoluteUrl) &&
                                !string.IsNullOrEmpty(name = Repository.Instance.GetByKey(absoluteUrl).FileName))
                            {
                                urlAttr.Value = name;
                            }
                            else
                            {
                                urlAttr.Value = absoluteUrl.AbsoluteUri;
                                results.Add(urlAttr);
                                Repository.Instance.Add(absoluteUrl, resource.Depth + 1);
                            }
                        }
                    }
                }
                ReplaceRelativeUrl(resource);
            }
            return results;
        }

        private void ReplaceRelativeUrl(ResourceInfo resource)
        {
            // todo:替换相对url
        }
    }
    public delegate bool FilterUrl(string url);
}
