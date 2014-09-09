using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spider
{
    public sealed class Registry : IRegistry
    {
        private readonly HashSet<Resource> _records;
        private readonly Queue<Resource> _newRecords = new Queue<Resource>();

        Registry()
        {
            _records = new HashSet<Resource>();
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
            static DelayConstructor()
            {
            }

// ReSharper disable once MemberHidesStaticFromOuterClass
            internal static readonly Registry Instance = new Registry();
        }

        public Resource Add(Uri uri, string suffix = null, string fileName = null)
        {
            Resource record;
            lock (_records)
            {
                record = string.IsNullOrWhiteSpace(fileName) ? new Resource(_records.Count + suffix) : new Resource(fileName + suffix);
                record.Uri = uri;
                if (_records.Contains(record)) return record.InstanceInHashSet;

                _records.Add(record);
            }

            lock (((ICollection)_newRecords).SyncRoot)
            {
                _newRecords.Enqueue(record);
            }
            return record; 
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
            return _records.Contains(record) ? record.InstanceInHashSet : null;
        }

        public IEnumerable<Resource> Records
        {
            get { return _records.AsEnumerable(); }
        }
    }
}
