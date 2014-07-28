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

        public void Push(Uri uri)
        {
            var record = new Record()
            {
                Uri = uri
            };
            if (_records.Contains(record)) return;

            record.Path = getFilePath(uri);
            record.State = State.New;
            _records.Add(record);
            _newRecords.Enqueue(record);
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
