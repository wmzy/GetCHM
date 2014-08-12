using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spider
{
    public sealed class Registry : IRegistry
    {
        private readonly HashSet<Record> _records;
        private readonly Queue<Record> _newRecords = new Queue<Record>();

        Registry()
        {
            _records = new HashSet<Record>();
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

        public Record Add(Uri uri, string suffix = null, string fileName = null)
        {
            Record record;
            lock (_records)
            {
                record = string.IsNullOrWhiteSpace(fileName) ? new Record(_records.Count + suffix) : new Record(fileName + suffix);
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
        public Record PopNew()
        {
            lock (((ICollection)_newRecords).SyncRoot)
            {
                return _newRecords.Dequeue();
            }
        }


        public Record FindByUri(Uri uri)
        {
            var record = new Record(null) {Uri = uri};
            return _records.Contains(record) ? record.InstanceInHashSet : null;
        }

        public IEnumerable<Record> Records
        {
            get { return _records.AsEnumerable(); }
        }
    }
}
