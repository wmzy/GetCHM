using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLine;

namespace GetCHM.CLI
{
    class Options
    {
        [VerbOption("init", HelpText = "初始化")]
        public InitSubOptions InitVerb { get; set; }
        [VerbOption("seeds", HelpText = "种子操作")]
        public SeedsSubOptions SeedsVerb { get; set; }
        [VerbOption("Spider", HelpText = "爬虫")]
        public SpiderSubOptions SpiderVerb { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            //  or using HelpText.AutoBuild
            var usage = new StringBuilder();
            usage.AppendLine("GetCHM-CLI 1.0");
            usage.AppendLine("Read user manual for usage instructions...");
            return usage.ToString();
        }
    }

    class InitSubOptions
    {
        [ValueOption(0)]
        public string Path { get; set; }
    }

    class SeedsSubOptions
    {
        [OptionArray('a', "add", HelpText = "添加种子url")]
        public string[] AddSeeds { get; set; }
        [OptionArray('r', "remove", HelpText = "移出种子url")]
        public string[] RemoveSeeds { get; set; }
        [Option('c', "Clear", HelpText = "清空种子")]
        public bool Clear { get; set; }
    }

    class SpiderSubOptions
    {
        [Option('d', "depth", HelpText = "下载深度", DefaultValue = 5)]
        public int Depth { get; set; }
        
    }
}
