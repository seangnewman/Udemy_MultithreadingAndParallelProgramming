using System;
using System.Threading.Tasks;
using System.Threading;

namespace Udemy_MultithreadingAndParallelProgramming
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //Chapter1Exercises();
           // StartingTasks();
            // TaskCancellation();

        }

        private static void TaskCancellation()
        {
            var parentCts = new CancellationTokenSource();
            var childCts = CancellationTokenSource.CreateLinkedTokenSource(parentCts.Token);
            var cts = new CancellationTokenSource();

            var t1 = Task.Run(() => Print(true, parentCts.Token), parentCts.Token);
            var t2 = Task.Run(() => Print(false, childCts.Token), childCts.Token);

            // Thread.Sleep(10);
            parentCts.CancelAfter(10);


            try
            {
                Console.WriteLine($"The first task processed: {t1.Result}");
                Console.WriteLine($"The second task processed: {t2.Result}");
            }
            catch (AggregateException ex)
            {

                Console.WriteLine("Cancellation Requested");
            }


            Console.WriteLine($"T1:{t1.Status}");
            Console.WriteLine($"T2:{t2.Status}");
        }

        private static void StartingTasks()
        {
            Task<int> t1 = Task.Factory.StartNew(() => Print(true), CancellationToken.None, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning, TaskScheduler.Default);
            Task<int> t2 = Task.Factory.StartNew(() => Print(false));

            Console.WriteLine($"The first task processed: {t1.Result}");
            Console.WriteLine($"The second task processed: {t2.Result}");

            Console.Read();
        }

        private static int Print(bool isEven, CancellationToken token)
        {
            Console.WriteLine($"Is thread pool thread :{Thread.CurrentThread.IsThreadPoolThread}");
            int total = 0;

            if (isEven)
            {
                for (int i = 0; i < 100; i+=2)
                {

                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation Requested");
                        
                    }
                    token.ThrowIfCancellationRequested();
                    total++;
                    Console.WriteLine($"Current task id={Task.CurrentId}.  Value = {i}");
                }
            }
            else
            {
                for (int i = 0; i < 100; i+=2)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation Requested");
                       
                    }
                    token.ThrowIfCancellationRequested();
                    total++;
                    Console.WriteLine($"Current task id={Task.CurrentId}.  Value = {i}");
                }
            }

            return total;

        
        }

        private static int Print(bool isEven)
        {
            Console.WriteLine($"Is thread pool thread :{Thread.CurrentThread.IsThreadPoolThread}");
            int total = 0;

            if (isEven)
            {
                for (int i = 0; i < 100; i += 2)
                {
                                        total++;
                    Console.WriteLine($"Current task id={Task.CurrentId}.  Value = {i}");
                }
            }
            else
            {
                for (int i = 0; i < 100; i += 2)
                {
                    total++;
                    Console.WriteLine($"Current task id={Task.CurrentId}.  Value = {i}");
                }
            }

            return total;


        }

        private static void Chapter1Exercises()
        {
            Chapter1.CreateProcess();
            Chapter1.StartingAThread();
            Chapter1.CancellingThreads();
            Chapter1.CoordinatingThreads();
            Chapter1.APMAndEAP();
        }

    }
}
