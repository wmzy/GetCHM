using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using GetCHM.Spider;

namespace GetCHM.CLI
{
    class Program
    {
        private static Options _options;

        void Main(string[] args)
        {
            _options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, _options, (s, o) =>
            {
                if (s.Equals("init"))
                {
                    Init((InitSubOptions) o);
                }
                else
                {
                    if (Directory.Exists(".getchm"))
                    {
                        Console.WriteLine("not a project");
                        return;
                    }

                    switch (s)
                    {
                        case "init":

                            break;
                        case "seeds":
                            Seeds((SeedsSubOptions) o);
                            break;
                        case "spider":
                            Spider((SpiderSubOptions) o);
                            break;
                    }
                }
            }))
            {
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }
        }

        private static void Init(InitSubOptions options)
        {
            var path = Path.Combine(options.Path, ".getchm");
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }

        private static void Seeds(SeedsSubOptions options)
        {
            var seedsPath = Path.Combine(".getchm", "seeds");

            if (options.Clear)
            {
                File.Delete(seedsPath);
                Console.WriteLine("Seeds are cleared");
            }
            else if (options.AddSeeds != null)
            {
                File.AppendAllLines(seedsPath, options.AddSeeds);
            }
            else if (options.RemoveSeeds != null)
            {
                if (File.Exists(seedsPath))
                    File.WriteAllLines(seedsPath, File.ReadAllLines(seedsPath).Except(options.RemoveSeeds).ToArray());

                Console.WriteLine("removed success");
            }
            else
            {
                if (!File.Exists(seedsPath)) return;

                foreach (var seed in File.ReadLines(seedsPath))
                {
                    Console.WriteLine(seed);
                }
            }
        }

        private static void Spider(SpiderSubOptions options)
        {
            var seedsPath = Path.Combine(".getchm", "seeds");
            var srcPath = Path.Combine(".getchm", "src");
            string[] seeds;

            if (!File.Exists(seedsPath) || (seeds = File.ReadAllLines(seedsPath)).Length <= 0)
            {
                Console.WriteLine("no seeds");
                return;
            }


            var client = new HttpClient();
            var fetcher = new Fetcher(client, srcPath);
            fetcher.DomLoad += (sebder, e) =>
            {
                var html = e.HtmlDocument;
                var nodes =
                    html.DocumentNode.SelectNodes(
                        "//*[@id='topnav' or @id='header' or @id='CommentForm' or @id='footer'] | //iframe");
                foreach (var node in nodes)
                {
                    node.Remove();
                }

            };
            var parser = new Parser(new List<ElementQuery>
            {
                new ElementQuery {Query = "//div[@class='minibook-list' or @class='well minibook-toc']//a", AttributeName = "href", OptionalSuffix = ".html"},
                new ElementQuery {Query = "//img", AttributeName = "src"},
                new ElementQuery
                {
                    Query = @"//script[@src]",
                    AttributeName = "src",
                    Suffix = ".js"
                },
                new ElementQuery
                {
                    Query = @"//link[@rel='stylesheet']",
                    AttributeName = "href",
                    Suffix = ".css"
                }
            });
            //var seeds = new[]
            //{
            //    new Uri(@"http://www.ituring.com.cn/minibook/950")
            //};
            var worker = new Worker(seeds.Select(url => new Uri(url)).ToArray(), fetcher, parser, 10);
            worker.StartAsync().Wait();
        }
    }
}
