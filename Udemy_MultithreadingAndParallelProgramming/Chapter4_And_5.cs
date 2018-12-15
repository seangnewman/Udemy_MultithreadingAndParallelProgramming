using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Udemy_MultithreadingAndParallelProgramming
{
    class Chapter4_And_5
    {

        static readonly object firstLock = new object();
        static readonly object seconLock = new object();
        public static void SynchronizationExample()
        {
            Task.Run((Action)Do);

            // Wait until we're fairly sure the first thread // has grabbed secondLock
            Thread.Sleep(500);
            Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Locking secondLock");

            lock (seconLock)
            {
                Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Locking secondLock");
                Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Locking firstlock");

                lock (firstLock)
                {
                    Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Locking firstlock");
                }
                Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Released secondLock");
            }
        }

        private static void Do()
        {
            Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Locking firstlock");
            lock (firstLock)
            {
                Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Locking firstlock");
                // Wait until we're fairly sure the first thread // has grabbed secondLock
                Thread.Sleep(1000);

                Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Locking secondLock");
                lock (seconLock)
                {
                    Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Locking secondLock");
                }

                Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Locking secondLock");
            }

            Console.WriteLine($"\t\t\t\t{Thread.CurrentThread.ManagedThreadId}-Locking firstlock");
        }

        private static void SemaphoreExamples()
        {
            //Create the semaphore with 3 slots, 3 available
            Bouncer = new SemaphoreSlim(3, 3);
            OpenNightClub();
            Thread.Sleep(20000);
        }
        private static void Swap(object obj1, object obj2)
        {
            //object tmp = obj1;
            //obj1 = obj2;
            //obj2 = tmp;

            object obj1Ref = Interlocked.Exchange(ref obj1, obj2);

            Interlocked.Exchange(ref obj2, obj1Ref);
        }


        public static SemaphoreSlim Bouncer { get; set; }
        private static void OpenNightClub()
        {
            for (int i = 0; i <= 50; i++)
            {
                // Let each guest enter on their own thread
                var number = i;
                Task.Run(() => Guest(number));
            }
        }

        private static void Guest(int guestNumber)
        {
            // Wait to enter the nightclub ( a semaphore to be released)
            Console.WriteLine($"Guest {guestNumber} is waiting to enter nightclub");
            Bouncer.Wait();

            // Do some dancing
            Console.WriteLine($"Guest {guestNumber} is dancing");
            Thread.Sleep(500);

            // Let one guest out (release a semaphore)
            Console.WriteLine($"Guest {guestNumber} is leaving");
            Bouncer.Release(1);

        }

        private static void InterlockedExample()
        {
            Character c = new Character();
            Character c2 = new Character();

            Swap(c, c2);

            var tasks = new List<Task>();

            for (int i = 0; i < 100; i++)
            {
                Task t1 = Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        c.CastArmorSpell(true);
                    }
                });
                tasks.Add(t1);

                Task t2 = Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        c.CastArmorSpell(false);
                    }
                });
                tasks.Add(t2);
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Resulting Armor = {c.Armor}");
        }

        private void DumpWebPage(string uri)
        {
            WebClient wc = new WebClient();
            string page = wc.DownloadString(uri);
            Console.WriteLine(page);
        }

        private async void DumpWebPageAsync(string uri)
        {
            WebClient wc = new WebClient();
            string page = await wc.DownloadStringTaskAsync(uri);
            Console.WriteLine(page);
        }

        public static void Chapter4And5Exercises()
        {
            InterlockedExample();

            SemaphoreExamples();

             SynchronizationExample();
        }
    }
}
