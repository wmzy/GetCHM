using System;
using System.Collections.Generic;

namespace GetCHM.Spider
{
    public sealed class Repository
    {
        private readonly Dictionary<Uri, ResourceInfo> _resource = new Dictionary<Uri, ResourceInfo>();
        private Repository()
        {
        }

        public bool IsExist(Uri url)
        {
            return _resource.ContainsKey(url);
        }

        public void Add(Uri url, int depth, ElementQuery elementQuery = null)
        {
            _resource.Add(url, new ResourceInfo
            {
                Uri = url,
                State = State.New,
                ElementQuery = elementQuery,
                Depth = depth
            });
        }

        public void Add(ResourceInfo resource)
        {
            _resource.Add(resource.Uri, resource);
        }

        public IEnumerable<ResourceInfo> Get()
        {
            return _resource.Values;
        }

        public static Repository Instance
        {
            get { return DelayConstructor.Instance; }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class DelayConstructor
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static DelayConstructor()
            {
            }

            // ReSharper disable once MemberHidesStaticFromOuterClass
            internal static readonly Repository Instance = new Repository();
        }

        public ResourceInfo GetByKey(Uri url)
        {
            ResourceInfo resource;
            _resource.TryGetValue(url, out resource);
            return resource;
        }
    }
}
