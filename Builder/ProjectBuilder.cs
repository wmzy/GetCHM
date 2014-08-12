using System;
using System.Linq;
using Spider;

namespace Builder
{
    public class ProjectBuilder
    {
        private readonly IRegistry _registry;
        public ProjectBuilder(string hhpFilePath)
        {
            HhpFileName = hhpFilePath;
        }

        public ProjectBuilder(IRegistry registry)
        {
            _registry = registry;
        }

        public string HhpFileName { get; set; }
        public string HhcFileName { get; set; }
        public string HhkFileName { get; set; }

        public string BuildHhcByRegistry()
        {
            var records = _registry.Records.Where(r => !string.IsNullOrWhiteSpace(r.Title)).OrderBy(r => r.Uri.AbsoluteUri).ToArray();
            if (records.Length == 0) return null;
            var baseRecords = records[0];
            Console.WriteLine(baseRecords.Uri.AbsoluteUri);
            for (int i = 1; i < records.Length; ++i)
            {
                var path = records[i - 1].Uri.MakeRelativeUri(records[i].Uri).ToString().Split('/');
                Console.WriteLine(r.Uri.AbsoluteUri + "\t" + r.FileName);
            }
            return null;
        }
        public void Compile()
        {
            if (string.IsNullOrWhiteSpace(HhpFileName)) return;

            var compiler = new Compiler(HhpFileName);
            compiler.Compile();
        }
    }
}
