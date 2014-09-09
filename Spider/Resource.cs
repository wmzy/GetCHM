using System;
using System.Collections.Generic;

namespace Spider
{
    public class Resource
    {
        public Resource(string fileName)
        {
            FileName = fileName;
        }
        public Uri Uri { get; set; }
        public string FileName { get; private set; }
        public string Title { get; set; }
        public Resource InstanceInHashSet { get; private set; }

        public override int GetHashCode()
        {
            return Uri.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var record = obj as Resource;
            if (record == null) return false;

            if (Uri.Equals(record.Uri))
            {
                record.InstanceInHashSet = this;
                return true;
            }

            return false;
        }
    }
}
