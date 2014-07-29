using System;
using System.Collections;
using System.Collections.Generic;

namespace Spider
{
    sealed class Registry : IRegistry
    {
        private readonly HashSet<Uri> _records;
        private readonly Queue<Uri> _newRecords;
        private readonly Queue<Uri> _downloadedRecords;

        Registry()
        {
            _records = new HashSet<Uri>();
            _newRecords = new Queue<Uri>();
            _downloadedRecords = new Queue<Uri>();
        }
        public static Registry Instance
        {
            get
            {
                return DelayConstructor.Instance;
            }
        }
        class DelayConstructor
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static DelayConstructor()
            {
            }

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

        public Uri GetForDownload()
        {
            lock (((ICollection)_newRecords).SyncRoot)
            {
                return _newRecords.Dequeue();
            }
        }

        public void WaitParse(Uri uri)
        {
            lock (((ICollection)_newRecords).SyncRoot)
            {
                _downloadedRecords.Enqueue(uri);
            }
        }

        public Uri GetForParse()
        {
            lock (((ICollection)_newRecords).SyncRoot)
            {
                return _downloadedRecords.Dequeue();
            }
        }
    }
}
