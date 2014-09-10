using System;
using GetCHM.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetCHM.Spider;


namespace Test
{
    [TestClass]
    public class TestProjectBuilder
    {
        [TestMethod]
        public void TestCompile()
        {
            var builder = new ProjectBuilder(@"D:\GetCHM\tem\123.hhp");
            builder.Compile();
        }

        [TestMethod]
        public void TestBuildHhcByUrl()
        {
            var record = Registry.Instance.Add(new Uri("http://www.jiangmiao.org/blog/"), ".html", "Index");

            Console.WriteLine(record.FileName);
            var downloader = new Downloader();
            downloader.FilterUrl = url => url.StartsWith("http://www.jiangmiao.org");
            downloader.Start();
            var builder = new ProjectBuilder(Registry.Instance);
            builder.BuildHhcByUrl();
        }
    }
}
