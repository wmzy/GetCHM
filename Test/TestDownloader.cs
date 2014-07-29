using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class TestDownloader
    {
        [TestMethod]
        public void TestMethod1()
        {
            var uri = new Uri("http://baidu.com/s?S=11");
            Console.WriteLine(uri.OriginalString + ":" + uri.OriginalString.GetHashCode().ToString());
            Console.WriteLine(new Uri("http://baidu.com/indes.html").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/s").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/s.htm").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/s?").GetHashCode());

            var q = new Queue<int>();
            q.Enqueue(1);
            q.Dequeue();
        }
    }
}
