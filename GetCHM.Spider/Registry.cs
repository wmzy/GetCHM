using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GetCHM.Spider
{
    public sealed class Registry : IRegistry
    {
        private readonly HashSet<Resource> _resources;
        private readonly Queue<Resource> _newRecords = new Queue<Resource>();

        Registry()
        {
            _resources = new HashSet<Resource>();
        }
        public static Registry Instance
        {
            get
            {
                return DelayConstructor.Instance;
            }
        }
// ReSharper disable once ClassNeverInstantiated.Local
        class DelayConstructor
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static DelayConstructor() { }

// ReSharper disable once MemberHidesStaticFromOuterClass
            internal static readonly Registry Instance = new Registry();
        }

        public Resource Add(Uri uri, string suffix = null, string fileName = null)
        {
            Resource resource;
            lock (_resources)
            {
                resource = string.IsNullOrWhiteSpace(fileName) ? new Resource(_resources.Count + suffix) : new Resource(fileName + suffix);
                resource.Uri = uri;
                if (_resources.Contains(resource)) return resource.InstanceInHashSet;

                _resources.Add(resource);
            }

            lock (((ICollection)_newRecords).SyncRoot)
            {
                _newRecords.Enqueue(resource);
            }
            return resource;
        }

        public bool HasNew
        {
            get { return _newRecords.Count > 0; }
        }
        public Resource PopNew()
        {
            lock (((ICollection)_newRecords).SyncRoot)
            {
                return _newRecords.Dequeue();
            }
        }


        public Resource FindByUri(Uri uri)
        {
            var record = new Resource(null) {Uri = uri};
            return _resources.Contains(record) ? record.InstanceInHashSet : null;
        }

        public IEnumerable<Resource> Records
        {
            get { return _resources.AsEnumerable(); }
        }
    }
}
