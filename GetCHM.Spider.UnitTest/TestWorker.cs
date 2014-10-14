using System;
using System.Net.Http;
using System.Text;
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
        public TestWorker()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

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
            var parser = new Parser(new List<ElementQuery>()
            {
                new ElementQuery {Query = "//a", AttributeName = "href", Suffix = ".html", IsAutoIdentifySuffix = true},
                new ElementQuery {Query = "//img", AttributeName = "src", IsAutoIdentifySuffix = true},
                new ElementQuery
                {
                    Query = @"//script[@type='text/javascript']",
                    AttributeName = "src",
                    Suffix = ".js",
                    IsAutoIdentifySuffix = false
                },
                new ElementQuery
                {
                    Query = @"//link[@rel='stylesheet']",
                    AttributeName = "href",
                    Suffix = ".css",
                    IsAutoIdentifySuffix = false
                }
            });
            var seeds = new Uri[]
            {
                new Uri(@"http://www.ituring.com.cn/book/1421")
            };
            var worker = new Worker(seeds, fetcher, parser, 2);
            worker.StartAsync().Wait();
        }
    }
}
