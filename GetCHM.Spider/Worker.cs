using System;
using System.Threading.Tasks;

namespace GetCHM.Spider
{
    public class Worker
    {
        private readonly Uri[] _seeds;
        private readonly Fetcher _fetcher;

        public Worker(Uri[] seeds, Fetcher fetcher)
        {
            _seeds = seeds;
            _fetcher = fetcher;
        }

        public void Start()
        {
            var tasks = new Task<ResourceInfo>[_seeds.Length];
            for (int i = 0; i < _seeds.Length; ++i)
            {
                tasks[i] = _fetcher.FetchAsync(_seeds[i]);
            }
        }
    }
}
