using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class TestRun
    {
        [TestMethod]
        public void TestHashSet()
        {
            var hashSet = new HashSet<MyClass> {new MyClass {String = "test"}};
            var hs = hashSet;
            hashSet.UnionWith(hs);
        }

        [TestMethod]
        public void TestUri()
        {
            var uri = new Uri("http://baidu.com/s?S=11");
            Console.WriteLine(uri.OriginalString + ":" + uri.OriginalString.GetHashCode().ToString());
            Console.WriteLine(new Uri("http://baidu.com/indes.html").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/s").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/s/").AbsoluteUri);
            Console.WriteLine(new Uri("http://baidu.com/#s").AbsoluteUri);
            Console.WriteLine(new Uri("http://baidu.com/#s").Equals(new Uri("http://baidu.com/")));
            Console.WriteLine(new Uri(new Uri("http://baidu.com/"), "http://bai.com/"));
            Console.WriteLine(new Uri("http://baidu.com/s/").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/s").GetHashCode());
        }

        [TestMethod]
        public void TestParallel()
        {
            Task[] tasks = new Task[5];
            DateTime time1 = DateTime.Now;
            Parallel.For(0, 5, i =>
            {
                //Thread.CurrentThread.Name = "Thread" + i;

                Console.WriteLine(i + ":" + Thread.CurrentThread.ManagedThreadId);
                tasks[i] = Task.Run(async () =>
                {
                    await Task.Delay((6 - 1) * 1000);
                    Console.WriteLine(i + ":" + Thread.CurrentThread.ManagedThreadId);
                });
            });
            Console.WriteLine("end");
            Task.WaitAll(tasks);
            Console.WriteLine(DateTime.Now.Ticks - time1.Ticks);

            DateTime time2 = DateTime.Now;
            for (int i = 0; i < 5; ++i)
            {
                var ii = i;
                tasks[i] = Task.Run(async () =>
                {
                    await Task.Delay((6 - ii) * 1000);
                    Console.WriteLine(ii + ":" + Thread.CurrentThread.ManagedThreadId);
                });
            };
            Console.WriteLine("end");
            Console.WriteLine(":" + Thread.CurrentThread.ManagedThreadId);
            Task.WaitAll(tasks);
            Console.WriteLine(DateTime.Now.Ticks - time2.Ticks);
        }

        [TestMethod]
        public void TestForeach()
        {
            var list = new List<int> { 1, 2 };
            foreach (var i in list)
            {
                Console.WriteLine(i);
                if (i < 6)
                {
                    list.Add(i + 1);
                }
            }
        }

        [TestMethod]
        public void TestOther()
        {
            Console.WriteLine(1.ToString(CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void TestPath()
        {
            string s = @"";
            var t = s.Split('\n');
            foreach (var ss in t)
            { }
        }
    }

    class MyClass
    {
        public string String { get; set; }

        public override bool Equals(object obj)
        {
            Console.WriteLine("Call Equals");
            return String.Equals(obj);
        }

        public override int GetHashCode()
        {
            return String.GetHashCode();
        }
    }
}
