using System;
using System.IO;
using System.Linq;
using System.Text;
using GetCHM.Spider;

namespace GetCHM.Builder
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

        public string HtmlFilePath { get; set; }
        public string ProjectPath { get; set; }
        public string HhpFileName { get; set; }
        public HhpOptions HhpOptions { get; set; }

        public string BuildHhcByUrl()
        {
            var records = _registry.Records.Where(r => !string.IsNullOrWhiteSpace(r.Title)).OrderBy(r => r.Uri.AbsoluteUri).ToArray();
            if (records.Length == 0) return null;
            var hhc = new StringBuilder();
            hhc.Append("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN\">" + "\n"
                       + "<HTML>" + "\n"
                       + "  <HEAD>" + "\n"
                       + "    <meta name=\"GENERATOR\" content=\"Microsoft&reg; HTML Help Workshop 4.1\">" + "\n"
                       + "    <!-- Sitemap 1.0 -->" + "\n"
                       + "  </HEAD>" + "\n"
                       + "  <BODY>" + "\n");
            hhc.Append("    <OBJECT type=\"text/site properties\">" + "\n"
                       + "      <param name=\"Window Styles\" value=\"0x800025\">" + "\n"
                       + "      <param name=\"ImageType\" value=\"Folder\">" + "\n"
                       + "      <param name=\"Font\" value=\"宋体,8,134\">" + "\n"
                       + "    </OBJECT>" + "\n"
                       + "    <UL>" + "\n");

            const string format = "<LI> <OBJECT type=\"text/sitemap\"><param name=\"Name\" value=\"{0}\"><param name=\"Local\" value=\"{1}\"></OBJECT>\n";
            int spaceCount = 6;
            hhc.Append(' ', spaceCount).AppendFormat(format, records[0].Title, records[0].FileName);
            for (int i = 1; i < records.Length; ++i)
            {
                var paths = records[i - 1].Uri.MakeRelativeUri(records[i].Uri).ToString().Split('/');
                for (int j = 0, l = paths.Length - 1; j < l; ++j)
                {
                    if (paths[j].Equals(".."))
                    {
                        spaceCount -= 2;
                        hhc.Append(' ', spaceCount).AppendLine("</UL>");
                    }
                    else
                    {
                        hhc.Append(' ', spaceCount).AppendLine("<UL>");
                        spaceCount += 2;
                    }
                }
                hhc.Append(' ', spaceCount).AppendFormat(format, records[i].Title, Path.Combine(HtmlFilePath, records[i].FileName));
            }
            hhc.Append("    </UL>" + "\n"
                       + "  </BODY>" + "\n"
                       + "</HTML>");
            Console.WriteLine(hhc);
            return null;
        }
        public void BuildHhpByUrl()
        {
            var hhp = new StringBuilder();
            hhp.AppendLine("[OPTIONS]")
                .AppendLine("Compatibility=1.1 or later")
                .AppendLine("Compiled file=123.chm")
                .AppendLine("Contents file=Table of Contents.hhc")
                .AppendLine("Default Font=@宋体,8,0")
                .AppendLine("Default topic=1.html")
                .AppendLine("Display compile progress=Yes")
                .AppendLine("Full-text search=Yes")
                .AppendLine("Index file=Index.hhk")
                .AppendLine("Language=0x804 中文(简体，中国)")
                .AppendLine("Title=完美主义")
                .AppendLine("");
            hhp.AppendLine("[FILES]");
            var records = _registry.Records.Where(r => r.FileName.EndsWith(".html"));
            foreach (var record in records)
            {
                hhp.AppendLine(record.FileName);
            }
            hhp.AppendLine("\n[INFOTYPES]");

        }
        public void Compile()
        {
            if (string.IsNullOrWhiteSpace(HhpFileName)) return;

            var compiler = new Compiler(HhpFileName);
            compiler.Compile();
        }
    }
}
