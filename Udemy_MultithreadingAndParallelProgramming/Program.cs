
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace Udemy_MultithreadingAndParallelProgramming
{


    class Program
    {
       
        static void Main(string[] args)
        {
            //ParallelDemo();

            //IEnumerable<int> numbers = Enumerable.Range(3, 100000 - 3);

            //var parallelQuery = from n in numbers.AsParallel().AsOrdered()
            //                    where Enumerable.Range(2, (int)Math.Sqrt(n)).All(i => n % i > 0)
            //                    select n;
                                

            //int[] primes = parallelQuery.ToArray();


          

            Console.Read();
        }

        private static void ParallelDemo()
        {
            // Parallel.Invoke(RunLoop1, RunLoop2);
            //var data = new List<int>();
            //Parallel.Invoke(() => data.AddRange(RunLoop1()),
            //                              () => data.AddRange(RunLoop1()));

            //ParallelOptions po = new ParallelOptions();


            //Parallel.For(1, 11, i => {
            //    Console.WriteLine($"{ i * i*i}");
            //});

            string sentence = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vitae ex vel velit posuere aliquet. Fusce porta dolor non dictum vulputate. Donec dictum efficitur 
                                                    tristique. Quisque eleifend volutpat nunc, ut egestas libero molestie et. Vivamus ut feugiat est. Cras non neque lacus. Donec nibh urna, porta sed ex eget, vestibulum 
                                                    convallis magna. Etiam pulvinar est eu lacus commodo, eu condimentum lorem feugiat.";

            string[] words = sentence.Split(' ');

            Parallel.ForEach(words, word =>
            {
                Console.WriteLine($"\"{word}\" is of {word.Length} length = thread {Thread.CurrentThread.ManagedThreadId}");
            });
        }

        static IEnumerable<int> RunLoop1()
        {
            for (int i = 0; i < 100; i++)
            {
                //Console.WriteLine($"ThreadID;{Thread.CurrentThread.ManagedThreadId};  Iteration {i}");
                yield return i;
            }
        }

        static IEnumerable<int> RunLoop2()
        {
            for (int i = 0; i < 100; i++)
            {
                //Console.WriteLine($"ThreadID;{Thread.CurrentThread.ManagedThreadId};  Iteration {i}");
                yield return i;
            }
        }




    }
}
