using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
            Registry.Instance.Add(new Uri("http://msdn.microsoft.com/zh-cn/library/hh696703.aspx"));
            Downloader downloader = new Downloader();
            downloader.FilterUrl = url => url.Equals("http://msdn.microsoft.com/zh-cn/library/hh696703.aspx");
            downloader.Start();
            Thread.Sleep(10000);
        }
        [TestMethod]
        public void TestMethod1()
        {
            var uri = new Uri("http://baidu.com/s?S=11");
            Console.WriteLine(uri.OriginalString + ":" + uri.OriginalString.GetHashCode().ToString());
            Console.WriteLine(new Uri("http://baidu.com/indes.html").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/s").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/#s").GetHashCode());

            Console.WriteLine(new Uri(new Uri("http://baidu.com/"), "http://bai.com/"));
            var q = new Queue<int>();
            q.Enqueue(1);
            q.Dequeue();
        }
    }
}
