using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spider;

namespace Test
{
    [TestClass]
    public class TestDownloader
    {
        [TestMethod]
        public void TestStart()
        {
            var record = Registry.Instance.Add(new Uri("http://www.jiangmiao.org/blog/"), ".html", "Index");

            Console.WriteLine(record.FileName);
            var downloader = new Downloader();
            downloader.FilterUrl = url => url.StartsWith("http://www.jiangmiao.org");
            downloader.Start();
        }
        //http://www.jiangmiao.org/blog/wp-content/uploads/2011/01/2011-01-26-144055_1020x746_scrot.png
        [TestMethod]
        public void TestDownloadPng()
        {
            var record = Registry.Instance.Add(new Uri("http://www.jiangmiao.org/blog/wp-content/uploads/2011/01/2011-01-26-144055_1020x746_scrot.png"), ".html", "Index");

            Console.WriteLine(record.FileName);
            var downloader = new Downloader();
            downloader.FilterUrl = url => url.StartsWith("http://www.jiangmiao.org/blog/wp-content/uploads/2011/01/2011-01-26-144055_1020x746_scrot.png");
            downloader.Start();
        }
    }
}
