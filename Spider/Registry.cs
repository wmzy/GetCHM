using System;
using System.Collections;
using System.Collections.Generic;

namespace Spider
{
    public sealed class Registry : IRegistry
    {
        private readonly HashSet<Uri> _records;
        private Queue<Uri> _newRecords = new Queue<Uri>();

        Registry()
        {
            _records = new HashSet<Uri>();
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

        public void Add(Uri uri)
        {
            lock (_records)
            {
                if (_records.Contains(uri)) return;

                _records.Add(uri);
            }

            lock (((ICollection)_newRecords).SyncRoot)
            {
                _newRecords.Enqueue(uri);
            }
        }

        public bool HasNew
        {
            get { return _newRecords.Count > 0; }
        }
        public Uri PopNew()
        {
            lock (((ICollection)_newRecords).SyncRoot)
            {
                return _newRecords.Dequeue();
            }
        }
    }
}
