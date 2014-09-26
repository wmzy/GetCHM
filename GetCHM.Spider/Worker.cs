using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async void StartAsync()
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

            var resources = new List<ResourceInfo>(tasks.Length);
            resources.AddRange(Repository.Instance.Get());

            for (int i = 0; i < Depth; ++i)
            {
                // todo: parse i
                var attributes = _parser.Parse(resources);
                // todo: fetch i+1
                await _fetcher.FetchAsync(attributes.Select(attr => new Uri(attr.Value)).ToArray());
                // todo: replace i
                // todo: save i
            }
        }

        public int Depth { get; private set; }
    }
}
