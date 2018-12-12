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

            //Task.Run(() => Print());
            //Task.Run(() => Print());

            StartingTasks();

        }

        private static void StartingTasks()
        {
            Task<int> t1 = Task.Factory.StartNew(() => Print(true), CancellationToken.None, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning, TaskScheduler.Default);
            Task<int> t2 = Task.Factory.StartNew(() => Print(false));

            Console.WriteLine($"The first task processed: {t1.Result}");
            Console.WriteLine($"The second task processed: {t2.Result}");

            Console.Read();
        }

        private static int Print(bool isEven)
        {
            Console.WriteLine($"Is thread pool thread :{Thread.CurrentThread.IsThreadPoolThread}");
            int total = 0;

            if (isEven)
            {
                for (int i = 0; i < 100; i+=2)
                {
                    total++;
                    Console.WriteLine($"Current task id={Task.CurrentId}.  Value = {i}");
                }
            }
            else
            {
                for (int i = 0; i < 100; i+=2)
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
