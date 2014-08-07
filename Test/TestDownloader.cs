﻿using System;
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
            Downloader downloader = new Downloader();
            downloader.FilterUrl = url => url.StartsWith("http://www.jiangmiao.org");
            downloader.Start();
            Thread.Sleep(10000);
        }
        [TestMethod]
        public void TestUri()
        {
            var uri = new Uri("http://baidu.com/s?S=11");
            Console.WriteLine(uri.OriginalString + ":" + uri.OriginalString.GetHashCode().ToString());
            Console.WriteLine(new Uri("http://baidu.com/indes.html").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/s").GetHashCode());
            Console.WriteLine(new Uri("http://baidu.com/").AbsoluteUri);
            Console.WriteLine(new Uri("http://baidu.com/#s").AbsoluteUri);
            Console.WriteLine(new Uri("http://baidu.com/#s").Equals(new Uri("http://baidu.com/")));
            Console.WriteLine(new Uri(new Uri("http://baidu.com/"), "http://bai.com/"));
            var q = new Queue<int>();
            q.Enqueue(1);
            q.Dequeue();
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
            for (int i  = 0; i < 5; ++i)
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
            var list = new List<int> {1, 2};
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
    }
}
