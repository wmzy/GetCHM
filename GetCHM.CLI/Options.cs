using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace GetCHM.CLI
{
    class Options
    {
        [OptionArray('s', "seed", Required = true, HelpText = "种子url")]
        public string[] SeedUrls { get; set; }

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
}
