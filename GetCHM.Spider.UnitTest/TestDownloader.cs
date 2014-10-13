using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GetCHM.Spider.UnitTest
{
    [TestClass]
    public class TestDownloader
    {
        [TestMethod]
        public void TestStart()
        {
            var record = Registry.Instance.Add(new Uri("http://www.jiangmiao.org/blog/"), ".html", "Index");

            Console.WriteLine((string) record.FileName);
            var downloader = new Downloader {FilterUrl = url => url.StartsWith("http://www.jiangmiao.org")};
            downloader.Start();
        }
        //http://www.jiangmiao.org/blog/wp-content/uploads/2011/01/2011-01-26-144055_1020x746_scrot.png
        [TestMethod]
        public void TestDownloadPng()
        {
            var record = Registry.Instance.Add(new Uri("http://www.jiangmiao.org/blog/wp-content/uploads/2011/01/2011-01-26-144055_1020x746_scrot.png"), ".html", "Index");

            Console.WriteLine((string) record.FileName);
            var downloader = new Downloader();
            downloader.FilterUrl = url => url.StartsWith("http://www.jiangmiao.org/blog/wp-content/uploads/2011/01/2011-01-26-144055_1020x746_scrot.png");
            downloader.Start();
        }
    }
}
