using System;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GetCHM.Spider.UnitTest
{
    /// <summary>
    /// TestWorker 的摘要说明
    /// </summary>
    [TestClass]
    public class TestWorker
    {
        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestStartAsync()
        {
            var client = new HttpClient();
            var fetcher = new Fetcher(client, @"F:\GetCHM");
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
            var seeds = new[]
            {
                new Uri(@"http://www.ituring.com.cn/minibook/950")
            };
            var worker = new Worker(seeds, fetcher, parser, 10);
            worker.StartAsync().Wait();
        }
    }
}
