using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class TestDownloader
    {
        [TestMethod]
        public void TestMethod1()
        {
            var uri = new Uri("http://baidu.com");
            Console.WriteLine(uri.GetHashCode());
        }
    }
}
