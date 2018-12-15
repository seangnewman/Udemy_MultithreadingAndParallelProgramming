using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Udemy_MultithreadingAndParallelProgramming
{
    class Chapter3
    {

        public Task ImportXmlFilesAsync(string dataDirectory, CancellationToken ct)
        {
            return Task.Factory.StartNew(() => {
                foreach (FileInfo file in new DirectoryInfo(dataDirectory).GetFiles("*.xml"))
                {
                    XElement doc = XElement.Load(file.FullName);
                    InternalProcessXml(doc);
                }
            }, ct);
        }

        public Task ImportXmlFilesAsync2(string dataDirectory, CancellationToken ct)
        {
            return Task.Factory.StartNew(() => {
                foreach (FileInfo file in new DirectoryInfo(dataDirectory).GetFiles("*.xml"))
                {
                    string fileToProcess = file.FullName;
                    Task.Factory.StartNew(_ => {
                        ct.ThrowIfCancellationRequested();
                        XElement doc = XElement.Load(file.FullName);
                        InternalProcessXml(doc, ct);
                    }, ct, TaskCreationOptions.AttachedToParent);
                }
            }, ct);
        }

        private void InternalProcessXml(XElement doc, CancellationToken ct)
        {

        }

        private void InternalProcessXml(XElement doc)
        {

        }

        private static void TestAggregateException()
        {
            var parent = Task.Factory.StartNew(() => {
                // We'll throw 3 exceptions at once using 3 child tasks:

                int[] numbers = { 0 };

                var childFactory = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.None);
                childFactory.StartNew(() => 5 / numbers[0]);
                childFactory.StartNew(() => numbers[1]);
                childFactory.StartNew(() => { throw null; });

            });

            try
            {
                parent.Wait();
            }
            catch (AggregateException aex)
            {
                aex.Flatten().Handle(ex => {
                    if (ex is DivideByZeroException)
                    {
                        Console.WriteLine("Divide by Zero");
                        return true;
                    }

                    if (ex is IndexOutOfRangeException)
                    {
                        Console.WriteLine("Index out of range");
                        return true;
                    }

                    return false;
                });

            }
        }

        private static int Print(bool isEven, CancellationToken token)
        {
            throw new InvalidOperationException();

            if (token.WaitHandle.WaitOne(2000))
            {
                token.ThrowIfCancellationRequested();
            }

            Console.WriteLine($"Is thread pool thread:{Thread.CurrentThread.IsThreadPoolThread}");
            int total = 0;
            if (isEven)
            {
                // throw new InvalidOperationException();

                for (int i = 0; i < 100; i += 2)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation Requested");
                    }
                    token.ThrowIfCancellationRequested();
                    total++;
                    Console.WriteLine($"Current task id = {Task.CurrentId}. Value={i}");
                }
            }
            else
            {
                for (int i = 1; i < 100; i += 2)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation Requested");
                    }
                    token.ThrowIfCancellationRequested();
                    total++;
                    Console.WriteLine($"Current task id = {Task.CurrentId}. Value={i}");
                }
            }
            return total;
        }

        public static void Chapter3Exercises()
        {
            //var cts = new CancellationTokenSource();

            //var t1 = Task.Run<int>(() => Print(true, cts.Token), cts.Token);
            //var t2 = Task.Run<int>(() => Print(false, cts.Token), cts.Token);
            //Task t2 = t1.ContinueWith(prevTask =>
            //{
            //    Console.WriteLine($"How many numbers were processed by prev. tasks = {prevTask.Result}");
            //    Task.Run<int>(() => Print(false, cts.Token), cts.Token);
            //}, TaskContinuationOptions.OnlyOnRanToCompletion);

            //t1.ContinueWith(t => Console.WriteLine("Finally, we are here!"), TaskContinuationOptions.OnlyOnFaulted);

            //Task.Factory.ContinueWhenAll(new[] { t1, t2 }, tasks =>
            //{
            //    var t1Task = tasks[0];
            //    var t2Task = tasks[1];

            //    Console.WriteLine($"t1Task:{t1Task.Result}, t2Task{t2Task.Result}");
            //});


            //Console.WriteLine("Started t1");
            //Console.WriteLine("Started t2");
            //t1.Wait();
            // Task.WaitAll(t1, t2);
            //int result = Task.WaitAny(t1, t2);

            //var tr = Task.WhenAny(t1, t2);
            //tr.ContinueWith(x => Console.WriteLine($"The id of a task wich completed first = {tr.Result.Id}"));
            //Console.WriteLine("After when any");

            //Console.WriteLine("Finished t1");
            //Console.WriteLine("Finished t2");


            //var t1 = Task.Run( () => Print(true, CancellationToken.None));
            //Task t2 = null;
            //Console.WriteLine("Started t1");

            //Task.Delay(1000).ContinueWith( x =>
            //  {
            //      t2 = Task.Run( () => Print(false, CancellationToken.None));
            //      Console.WriteLine("Started t2");
            //  }  );

            // AmpEap.Test();

            // Console.WriteLine("Main Thread is not blocked");

            //var t1 = Task.Run(() => Print(true, CancellationToken.None));

            //try
            //{
            //    t1.Wait();
            //}
            //catch (AggregateException ex)
            //{
            //    var flattenList = ex.Flatten().InnerExceptions;
            //    foreach (var curEx in flattenList)
            //    {
            //        Console.WriteLine(curEx);
            //    }

            //   // ReadOnlyCollection<Exception> exs = ex.InnerExceptions;
            //}

            Task.Factory.StartNew(() => {
                Task nested = Task.Factory.StartNew(() => Console.WriteLine("Hello, World!"), TaskCreationOptions.AttachedToParent);
            }).Wait();

            // Console.Read();



        }

    }
}
