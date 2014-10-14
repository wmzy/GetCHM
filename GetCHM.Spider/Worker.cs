using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace GetCHM.Spider
{
    public class Worker
    {
        private readonly Uri[] _seeds;
        private readonly Fetcher _fetcher;
        private readonly Parser _parser;
        public Worker(Uri[] seeds, Fetcher fetcher, Parser parser, int depth = int.MaxValue)
        {
            _seeds = seeds;
            _fetcher = fetcher;
            _parser = parser;
            Depth = depth;
        }

        public async Task StartAsync()
        {
            var tasks = new Task<ResourceInfo>[_seeds.Length];
            for (int i = 0; i < _seeds.Length; ++i)
            {
                tasks[i] = _fetcher.FetchAsync(_seeds[i]);
            }
            foreach (var task in tasks)
            {
                var resource = await task;
                resource.Depth = 0;
                Repository.Instance.Add(resource);
            }

            var resources = Repository.Instance.Get().ToArray();

            for (int i = 0; i < Depth; ++i)
            {
                List<HtmlAttribute> urlAttributesToUpdate;
                List<Uri> urlToFetch;
                _parser.Parse(resources, out urlAttributesToUpdate, out urlToFetch);
                // fetch i+1
                var newResources = await _fetcher.FetchAsync(urlToFetch.ToArray());
                // replace i
                foreach (var htmlAttribute in urlAttributesToUpdate)
                {
                    htmlAttribute.Value = Repository.Instance.GetByKey(new Uri(htmlAttribute.Value)).FileName;
                }
                // save i
                foreach (var resource in resources)
                {
                    resource.HtmlDocument.Save(resource.FileName);
                    resource.State = State.Saved;
                }
                resources = newResources;
            }
            foreach (var resource in resources)
            {
                _parser.ReplaceRelativeUrl(resource);
                resource.HtmlDocument.Save(resource.FileName);
                resource.State = State.Saved;
            }
        }

        public int Depth { get; private set; }
    }
}
