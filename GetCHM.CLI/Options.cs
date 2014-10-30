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
        [VerbOption("whitelist", HelpText = "白名单")]
        public WhitelistSubOptions WhitelistVerb { get; set; }
        [VerbOption("blacklist", HelpText = "黑名单")]
        public BlacklistSubOptions BlacklistVerb { get; set; }
        [VerbOption("spider", HelpText = "爬虫")]
        public SpiderSubOptions SpiderVerb { get; set; }
        [VerbOption("urlquery", HelpText = "url查询xpath设置")]
        public UrlQuerySubOptions UrlQueryVerb { get; set; }

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
        public string AddSeed { get; set; }
        [OptionArray('r', "remove", HelpText = "移出种子url")]
        public string RemoveSeed { get; set; }
        [Option('c', "clear", HelpText = "清空种子")]
        public bool Clear { get; set; }
    }

    class UrlQuerySubOptions
    {
        [Option('a', "add", HelpText = "添加")]
        public bool Add { get; set; }
        [Option('r', "remove", HelpText = "移除")]
        public bool Remove { get; set; }
        [Option('c', "Clear", HelpText = "清空种子")]
        public bool Clear { get; set; }
        [ValueOption(0)]
        public string Xpath { get; set; }
        [Option("attr", Required = true, HelpText = "url属性名")]
        public string AttributeName { get; set; }
        [Option('s', "suffix", HelpText = "保存文件后缀")]
        public string Suffix { get; set; }
        [Option("optional-suffix", HelpText = "当无法判断保存文件后缀时使用此后缀")]
        public string OptionalSuffix { get; set; }
    }

    class BlacklistSubOptions
    {
        [Option('a', "add", HelpText = "添加到黑名单，url正则表达式")]
        public string AddRegex { get; set; }
        [Option('r', "remove", HelpText = "从黑名单中移除")]
        public string RemoveRegex { get; set; }
        [Option('c', "clear", HelpText = "清空黑名单")]
        public bool Clear { get; set; }
    }

    class WhitelistSubOptions
    {
        [Option('a', "add", HelpText = "添加到黑名单，url正则表达式")]
        public string AddRegex { get; set; }
        [Option('r', "remove", HelpText = "从黑名单中移除")]
        public string RemoveRegex { get; set; }
        [Option('c', "clear", HelpText = "清空黑名单")]
        public bool Clear { get; set; }
         
    }
    class ConfigSubOptions
    {
        [Option('d', "depth", HelpText = "下载深度", DefaultValue = 5)]
        public int Depth { get; set; }
        [Option('t', "trim", HelpText = "Dom裁剪")]
        public string TrimXpath { get; set; }
    }

    class SpiderSubOptions
    {
        [Option('d', "depth", HelpText = "下载深度", DefaultValue = 5)]
        public int Depth { get; set; }
        [Option('t', "trim", HelpText = "Dom裁剪")]
        public string TrimXpath { get; set; }
    }
}
