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
            Thread.Sleep(10000);
        }
        
    }
}
