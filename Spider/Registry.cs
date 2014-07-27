using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    class Registry : IRegistry
    {
        private HashSet<Record> _records;
        private Queue<Record> _newRecords;
        private Queue<Record> _downloadedRecords; 

        public Registry()
        {
            _records = new HashSet<Record>();
            _newRecords = new Queue<Record>();
            _downloadedRecords = new Queue<Record>();
        }

        public void Push(string uri)
        {
            var u = new Uri(uri);
            var r = new Record()
            {
                Uri = new Uri(uri)
            };
            if (_records.Contains(r)) return;

            r.Path = getFilePath(u);
            r.State = State.New;
            _records.Add(r);
            _newRecords.Enqueue(r);
        }

        public Record GetNew()
        {
            return _newRecords.Dequeue();
        }

        public void WaitParse(Record record)
        {
            _downloadedRecords.Enqueue(record);
        }

        public Record GetForParse()
        {
            return _downloadedRecords.Dequeue();
        }

        private string getFilePath(Uri u)
        {
            throw new NotImplementedException();
        }
    }
}
