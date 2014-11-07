using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using GetCHM.Spider;

namespace GetCHM.CLI
{
    class Program
    {
        private static Options _options;
        private static readonly string _seedsPath = Path.Combine(".getchm", "seeds");
        private static readonly string _blacklistPath = Path.Combine(".getchm", "blacklist");
        private static readonly string _whitelistPath = Path.Combine(".getchm", "whitelist");
        private static readonly string _queryPath = Path.Combine(".getchm", "elementquery");

        static void Main(string[] args)
        {
            _options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, _options, (s, o) =>
            {
                if (!string.IsNullOrEmpty(s) && s.Equals("init"))
                {
                    Init((InitSubOptions)o);
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
                        case "urlquery":
                            ConfigUrlQuery((UrlQuerySubOptions)o);
                            break;
                        case "blacklist":
                            Blacklist((BlacklistSubOptions)o);
                            break;
                        case "whitelist":
                            Whitelist((WhitelistSubOptions) o);
                            break;
                        case "seeds":
                            Seeds((SeedsSubOptions)o);
                            break;
                        case "spider":
                            Spider((SpiderSubOptions)o);
                            break;
                    }
                }
            }))
            {
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }
        }

        private static void Blacklist(BlacklistSubOptions options)
        {
            if (options.Clear)
            {
                File.Delete(_blacklistPath);
                Console.WriteLine("blacklist are cleared");
                return;
            }

            var blacklist = File.Exists(_blacklistPath) ? File.ReadAllLines(_blacklistPath).ToList() : new List<string>(1);

            if (!string.IsNullOrWhiteSpace(options.AddRegex) && !blacklist.Contains(options.AddRegex))
            {
                blacklist.Add(options.AddRegex);
                File.WriteAllLines(_blacklistPath, blacklist);
                Console.WriteLine("blacklist has been removed");
            }
            else if (!string.IsNullOrWhiteSpace(options.RemoveRegex))
            {
                blacklist.Remove(options.RemoveRegex);
                File.WriteAllLines(_blacklistPath, blacklist);
                Console.WriteLine("removed success");
            }
            else
            {
                foreach (var li in blacklist)
                {
                    Console.WriteLine(li);
                }
            }
        }
        private static void Whitelist(WhitelistSubOptions options)
        {
            if (options.Clear)
            {
                File.Delete(_whitelistPath);
                Console.WriteLine("whitelist are cleared");
                return;
            }

            var whitelist = File.Exists(_whitelistPath) ? File.ReadAllLines(_whitelistPath).ToList() : new List<string>(1);

            if (!string.IsNullOrWhiteSpace(options.AddRegex) && !whitelist.Contains(options.AddRegex))
            {
                whitelist.Add(options.AddRegex);
                File.WriteAllLines(_whitelistPath, whitelist);
                Console.WriteLine("whitelist has been removed");
            }
            else if (!string.IsNullOrWhiteSpace(options.RemoveRegex))
            {
                whitelist.Remove(options.RemoveRegex);
                File.WriteAllLines(_whitelistPath, whitelist);
                Console.WriteLine("removed success");
            }
            else
            {
                foreach (var li in whitelist)
                {
                    Console.WriteLine(li);
                }
            }
        }


        private static void ConfigUrlQuery(UrlQuerySubOptions options)
        {
            if (options.Clear)
            {
                File.Delete(_queryPath);
                Console.WriteLine("elementquery clear");
            }
            else if (options.Add && !string.IsNullOrWhiteSpace(options.AttributeName))
            {
                List<ElementQuery> elementQueries = File.Exists(_queryPath)
                    ? (List<ElementQuery>)DeserializeFromFile(_queryPath)
                    : new List<ElementQuery>(1);

                elementQueries.Add(new ElementQuery
                {
                    AttributeName = options.AttributeName,
                    OptionalSuffix = options.OptionalSuffix,
                    Query = options.Xpath,
                    Suffix = options.Suffix
                });

                SerializeToFile(_queryPath, elementQueries);

                Console.WriteLine("elementquery added");
            }
            else if (options.Remove)
            {
                Console.WriteLine("removed");
            }
        }

        private static void SerializeToFile(string path, object obj)
        {
            using (var fs = File.Create(path))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
            }
        }

        private static object DeserializeFromFile(string path)
        {
            using (var fs = File.OpenRead(path))
            {
                var bf = new BinaryFormatter();
                return bf.Deserialize(fs);
            }
        }

        private static void Init(InitSubOptions options)
        {
            var path = string.IsNullOrWhiteSpace(options.Path) ? ".getchm" : Path.Combine(options.Path, ".getchm");
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }

        private static void Seeds(SeedsSubOptions options)
        {
            if (options.Clear)
            {
                File.Delete(_seedsPath);
                Console.WriteLine("Seeds are cleared");
                return;
            }

            var seeds = File.Exists(_seedsPath) ? File.ReadAllLines(_seedsPath).ToList() : new List<string>(1);

            if (!string.IsNullOrWhiteSpace(options.AddSeed) && !seeds.Contains(options.AddSeed))
            {
                seeds.Add(options.AddSeed);
                File.WriteAllLines(_seedsPath, seeds);
                Console.WriteLine("seed has been removed");
            }
            else if (!string.IsNullOrWhiteSpace(options.RemoveSeed))
            {
                seeds.Remove(options.RemoveSeed);
                File.WriteAllLines(_seedsPath, seeds);
                Console.WriteLine("removed success");
            }
            else
            {
                foreach (var seed in seeds)
                {
                    Console.WriteLine(seed);
                }
            }
        }

        private static void Spider(SpiderSubOptions options)
        {
            var srcPath = Path.Combine(".getchm", "src");
            string[] seeds;

            if (!File.Exists(_seedsPath) || (seeds = File.ReadAllLines(_seedsPath)).Length <= 0)
            {
                Console.WriteLine("no seeds");
                return;
            }

            var client = new HttpClient();
            var fetcher = new Fetcher(client, srcPath);

            // Dom裁剪
            if (string.IsNullOrWhiteSpace(options.TrimXpath))
                fetcher.DomLoad += (sebder, e) =>
                {
                    var html = e.HtmlDocument;
                    // "//*[@id='topnav' or @id='header' or @id='CommentForm' or @id='footer'] | //iframe"
                    var nodes = html.DocumentNode.SelectNodes(options.TrimXpath);
                    foreach (var node in nodes)
                    {
                        if (node != null) node.Remove();
                    }

                };

            var eq = (List<ElementQuery>) DeserializeFromFile(_queryPath);
            eq.AddRange(new List<ElementQuery>
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
            var parser = new Parser(eq)
            {
                FilterUrl = url =>
                {
                    if (
                        File.ReadLines(_whitelistPath)
                            .Any(line => Regex.IsMatch(url, line, RegexOptions.IgnoreCase | RegexOptions.Singleline)))
                    {
                        return true;
                    }
                    return
                        File.ReadLines(_blacklistPath)
                            .All(line => !Regex.IsMatch(url, line, RegexOptions.IgnoreCase | RegexOptions.Singleline));
                }
            };
            //var seeds = new[]
            //{
            //    new Uri(@"http://www.ituring.com.cn/minibook/950")
            //};
            var worker = new Worker(seeds.Select(url => new Uri(url)).ToArray(), fetcher, parser, options.Depth);
            worker.StartAsync().Wait();
        }
    }
}
