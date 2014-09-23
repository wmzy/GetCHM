using System;
using System.IO;
using GetCHM.Spider;

namespace GetCHM.CLI
{
    class Program
    {
        private static Options _options;
        static void Main(string[] args)
        {
            _options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, _options))
            {
                DownloadHtml();
            }
        }

        private static void DownloadHtml()
        {
            var downloader = new Downloader
            {
                FilePath = _options.FilePath ?? Path.Combine(Directory.GetCurrentDirectory(), "GetCHM", "src"),
                MaxDepth = _options.Depth
            };
            // downloader.FilterUrl = url => url.StartsWith("http://www.jiangmiao.org/blog/wp-content/uploads/2011/01/2011-01-26-144055_1020x746_scrot.png");

            for (int i = 0; i < _options.SeedUrls.Length; ++i)
            {
                var resource = Registry.Instance.Add(new Uri(_options.SeedUrls[i]), ".html", "Index" + i);
                resource.Depth = 0;
            }

            downloader.Start();
            Console.WriteLine(downloader.FilePath);
        }
    }
}
