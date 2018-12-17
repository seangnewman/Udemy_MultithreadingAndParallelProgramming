
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Udemy_MultithreadingAndParallelProgramming
{


    class Program
    {
       
        static void Main(string[] args)
        {
            //StackDemo();
            // QueueDemo();
            //ListDemo();
            //SetsDemo();
            //ImmutableDictionaryDemo();
            //ConcurrentQueueDemo();
            //ConcurrentStackDemo();
            // ConcurrentBagDemo();

            CancellationTokenSource cts = new CancellationTokenSource();

            ProducerConsumerDemo pcd = new ProducerConsumerDemo();
            Task.Run( () => pcd.Run(cts.Token));
            Console.Read();
            cts.Cancel();
            Console.WriteLine("End of Processing");
           // ConcurrentDictionaryDemo();

            Console.Read();
        }

        private static void ConcurrentDictionaryDemo()
        {
            var controller = new StockController();
            TimeSpan workDay = new TimeSpan(0, 0, 1);

            Task t1 = Task.Run(() => new SalesManager("Cody").StartWork(controller, workDay));
            Task t2 = Task.Run(() => new SalesManager("Connor").StartWork(controller, workDay));
            Task t3 = Task.Run(() => new SalesManager("Sean").StartWork(controller, workDay));

            Task.WaitAll(t1, t2, t3);

            controller.DisplayStatus();
        }

        private static readonly List<int> largeList = new List<int>(128);


        
        private static void GenerateList()
        {
            for (int i = 0; i < 100000; i++)
            {
                largeList.Add(i);
            }
        }


        static void ConcurrentBagDemo()
        {
            var names = new ConcurrentBag<string>();

            names.Add("Cody");
            names.Add("Connor");
            names.Add("Sean");

            Console.WriteLine($"After Add , count = {names.Count}");

            string item1;
            bool success = names.TryTake(out item1);
            if (success)
            {
                Console.WriteLine($"\nRemoving  {item1}");
            }
            else
            {
                Console.WriteLine("queue was empty");
            }


            string item2;
            success = names.TryPeek(out item2);
            if (success)
            {
                Console.WriteLine($"\nPeeking  {item2}");
            }
            else
            {
                Console.WriteLine("queue was empty");
            }

            Console.WriteLine("Enumerating the ");

            PrintOutCollection(names);

            Console.WriteLine($"\nAfter enumerating, count = {names.Count}");


        }

        static void ConcurrentStackDemo()
        {
            var names = new ConcurrentStack<string>();

            names.Push("Cody");
            names.Push("Connor");
            names.Push("Sean");

            Console.WriteLine($"After Push , count = {names.Count}");

            string item1;
            bool success = names.TryPop(out item1);
            if (success)
            {
                Console.WriteLine($"\nRemoving  {item1}");
            }
            else
            {
                Console.WriteLine("stack was empty");
            }


            string item2;
            success = names.TryPeek(out item2);
            if (success)
            {
                Console.WriteLine($"\nPeeking  {item2}");
            }
            else
            {
                Console.WriteLine("stack was empty");
            }

            Console.WriteLine("Enumerating .. ");

            PrintOutCollection(names);

            Console.WriteLine($"\nAfter enumerating, count = {names.Count}");


        }


        static void ConcurrentQueueDemo()
        {
            var names = new ConcurrentQueue<string>();

            names.Enqueue("Cody");
            names.Enqueue("Connor");
            names.Enqueue("Sean");

            Console.WriteLine($"After enqueing , count = {names.Count}");

            string item1;
            bool success = names.TryDequeue(out item1);
            if (success)
            {
                Console.WriteLine($"\nRemoving  {item1}");
            }
            else
            {
                Console.WriteLine("queue was empty");
            }


            string item2;
            success = names.TryPeek(out item2);
            if (success)
            {
                Console.WriteLine($"\nPeeking  {item2}");
            }
            else
            {
                Console.WriteLine("queue was empty");
            }

            Console.WriteLine("Enumerating the ");

            PrintOutCollection(names);

            Console.WriteLine($"\nAfter enumerating, count = {names.Count}");


        }



        static void BuildImmutableCollectionDemo()
        {
            //var builder = ImmutableList.CreateBuilder<int>();

            //foreach (var item in largeList)
            //{
            //    builder.Add(item);
            //}

            ////builder.AddRange(largeList);

            //var immutableList = builder.ToImmutable();

            var list = largeList.ToImmutableList();
        }


        static void ImmutableDictionaryDemo()
        {
            var dic = ImmutableDictionary<int, string>.Empty;
            dic =  dic.Add(1, "Cody");
           dic =  dic.Add(2, "Connor");
            dic = dic.Add(3, "Sean");

             

            IterateOverDictionary(dic);

            Console.WriteLine("Changing value of key 2 to Taylor");
           dic =  dic.SetItem(2, "Taylor");

            IterateOverDictionary(dic);

            var connor = dic[1];
            Console.WriteLine($"Who is at key 1 {connor}");

            Console.WriteLine("Remove record where key = 2");
            dic = dic.Remove(2);

            IterateOverDictionary(dic);
        }

        private static void IterateOverDictionary(ImmutableDictionary<int, string> dic)
        {
            foreach (var item in dic)
            {
                Console.WriteLine($"{item.Key} - {item.Value}");
            }
        }

        static void SetsDemo()
        {
            var hashSet = ImmutableHashSet<int>.Empty;
            hashSet = hashSet.Add(5);
            hashSet = hashSet.Add(10);

            // Displays "5" and "10" in unpredicatble order
            // at least in multithreaded scenarios
            PrintOutCollection(hashSet);

            Console.WriteLine("Remove 5");
            hashSet = hashSet.Remove(5);

            PrintOutCollection(hashSet);

            Console.WriteLine("--- ImmutableSortedSet Demo ---");

            var sortedSet = ImmutableSortedSet<int>.Empty;
            sortedSet = sortedSet.Add(5);
            sortedSet = sortedSet.Add(10);

            PrintOutCollection(sortedSet);

            var smallest = sortedSet[0];
            Console.WriteLine($"Smallest item: {smallest}");

            Console.WriteLine("Remove 5");
            sortedSet = sortedSet.Remove(5);

            PrintOutCollection(sortedSet);


        }

        static void ListDemo()
        {
            var list = ImmutableList<int>.Empty;
            list = list.Add(2);
            list = list.Add(3);
            list = list.Add(4);
            list = list.Add(5);

            PrintOutCollection(list);

            Console.WriteLine($"Remove 4 and then RemoveAt index = 2");
            list = list.Remove(4);
            list = list.RemoveAt(2);

            PrintOutCollection(list);

            Console.WriteLine("Insert 1 at 0 and 4 at 3");
            list = list.Insert(0, 1);
            list = list.Insert(3, 4);

            PrintOutCollection(list);

        }


        static void QueueDemo()
        {
            var queue = ImmutableQueue<int>.Empty;

            queue = queue.Enqueue(1);
            queue = queue.Enqueue(2);

            PrintOutCollection(queue);

            int first = queue.Peek();
            Console.WriteLine($"Last item: {first}");

            queue = queue.Dequeue(out first);
            Console.WriteLine($"Last item: {first}, Last after Dequeue: {queue.Peek()}");

        }


        static void StackDemo()
        {
            var stack = ImmutableStack<int>.Empty;

            stack = stack.Push(1);
            stack = stack.Push(2);

            int last = stack.Peek();
            Console.WriteLine($"Last item: {last}");

            stack = stack.Pop(out last);
            Console.WriteLine($"Last item: {last}, last after Pop: {stack.Peek()}");

        }

        private static void PrintOutCollection<T>(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }



    }
  }
