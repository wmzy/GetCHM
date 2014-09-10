using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCHM.Builder
{
    public class HhpOptions
    {
        public string Compatibility
        {
            get { return "1.1 or later"; }
        }

        public string CompiledFile { get; set; }
        public string ContentsFile { get; set; }//=Table of Contents.hhc
        public string DefaultFont { get; set; }//=@宋体,8,0
        public string DefaultTopic { get; set; }//=1.html

        public string DisplayCompileProgress
        {
            get { return "Yes"; }
        }

        public string FullTextSearch { get; set; }//=Yes
        public string IndexFile { get; set; }//=Index.hhk

        public string Language
        {
            get { return "0x804 中文(简体，中国)"; }
        }

        public string Title { get; set; }//=完美主义

        public override string ToString()
        {
            var sb = new StringBuilder("[OPTIONS]");
            sb.AppendLine(Combine("Compatibility", Compatibility))
                .AppendLine(Combine("Compiled file", CompiledFile))
                .AppendLine(Combine("Contents file", ContentsFile))
                .AppendLine(Combine("Default Font", DefaultFont))
                .AppendLine(Combine("Default topic", DefaultTopic))
                .AppendLine(Combine("Display compile progress", DisplayCompileProgress))
                .AppendLine(Combine("Full-text search", FullTextSearch))
                .AppendLine(Combine("Index file", IndexFile))
                .AppendLine(Combine("Language", Language))
                .AppendLine(Combine("Title", Title));
            return sb.ToString();
        }

        private string Combine(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            return name + "=" + value;
        }
    }
}
