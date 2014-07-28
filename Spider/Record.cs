using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    class Record
    {
        public Uri Uri { get; set; }
        public string Path { get; set; }
        public State State { get; set; }
        public FileType FileType { get; set; }

        public override int GetHashCode()
        {
            return Uri.GetHashCode();
        }
    }

    enum State
    {
        New,
        Downloading,
        Downloadead,
        Parsing,
        Parsed
    }
    enum FileType
    {
        Html,
        JavaScript,
        Image,
        File
    }
}
