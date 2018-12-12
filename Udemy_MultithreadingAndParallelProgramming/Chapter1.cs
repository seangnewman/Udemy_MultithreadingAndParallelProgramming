﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Udemy_MultithreadingAndParallelProgramming
{


    public static class Chapter1
    {
        private const string FilePath = @"c:\demo.txt";

        public static void APMAndEAP()
        {
            TestWrite();

            //Wait operations to complete
            Thread.Sleep(60000);
        }

        public static void TestWrite()
        {
            // Must specify FileOptions.Asynchronous otherwise the BeginXxx/EndXxx methods are
            // handled synchronously

            FileStream fs = new FileStream( FilePath,
                                                                            FileMode.OpenOrCreate,
                                                                            FileAccess.Write,
                                                                            FileShare.None,
                                                                            8,
                                                                            FileOptions.Asynchronous
                                                                            );
            string content = "A quick brown fox jumps over the lazy dog";
            byte[] data = Encoding.Unicode.GetBytes(content);

            //Begins to write content to the file stream
            Console.WriteLine("Begin to write");

            fs.BeginWrite(data, 0, data.Length, OnWriteCompleted, fs);
            Console.WriteLine("Write queued");
        }

        public static void OnWriteCompleted(IAsyncResult asyncResult)
        {
            // End the async operation
            FileStream fs = (FileStream)asyncResult.AsyncState;
            fs.EndWrite(asyncResult);

            // Close the file stream
            fs.Close();

            // Test async read bytes from the file stream
            TestRead();

        }

        public static void TestRead()
        {
            // Must specify FileOptions.Asynchronous otherwise BeginXxx/EndXxx methods are
            // handled synchronously

            FileStream fs = new FileStream(FilePath,
                                                                            FileMode.OpenOrCreate,
                                                                            FileAccess.Read,
                                                                            FileShare.None,
                                                                            8,
                                                                            FileOptions.Asynchronous
                                                                            );

            byte[] data = new byte[1024];

            // Begins to read content to the file stream
            Console.WriteLine("Begin to read");
            // Pass both Fs and data as async state object
            fs.BeginRead(data, 0, data.Length, OnReadCompleted, new { Stream = fs, Data = data });
            Console.WriteLine("Read queued");
        }

        public static void OnReadCompleted(IAsyncResult asyncResult)
        {
            dynamic state = asyncResult.AsyncState;

            // End Read
            int bytesRead = state.Stream.EndRead(asyncResult);
        }

        public static void CoordinatingThreads()
        {
            var printInfo = new PrintingInfo();
            Thread t1 = new Thread(() => Print(false, printInfo));
            t1.IsBackground = true;
            t1.Priority = ThreadPriority.Highest;
            t1.Start();

            if (t1.Join(TimeSpan.FromMilliseconds(500)))
            {
                Console.WriteLine($"I'm sure that spawned thread processed that many: {printInfo.ProcessedNumbers}");
            }
            else
            {
                Console.WriteLine("Timed out.  Can't process results.");
            }



            Print(true, printInfo);
        }

        public static void CancellingThreads()
        {
            Thread t1 = new Thread(() => Print(false));
            t1.Start();

            Thread.Sleep(10);

            t1.Abort();
            Console.WriteLine("After abort");

            Print(true);
        }

        public static void StartingAThread()
        {
            Thread t1 = new Thread(() => Print(false));
            t1.Start();

            Print(true);
        }
        public static void Print(bool isEven, PrintingInfo printInfo)
        {

            Console.WriteLine($"Current Thread ID : {Thread.CurrentThread.ManagedThreadId}");

            try
            {
                if (isEven)
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        if (i % 2 == 0)
                        {
                            printInfo.ProcessedNumbers++;
                            Console.WriteLine(i);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        if (i % 2 != 0)
                        {
                            printInfo.ProcessedNumbers++;
                            Console.WriteLine(i);
                        }
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                Console.WriteLine(ex);
            }

        }
        public static void Print(bool isEven)
        {

            Console.WriteLine($"Current Thread ID : {Thread.CurrentThread.ManagedThreadId}");

            try
            {
                if (isEven)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        if (i % 2 == 0)
                        {
                            Console.WriteLine(i);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 100; i++)
                    {
                        if (i % 2 != 0)
                        {
                            Console.WriteLine(i);
                        }
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                Console.WriteLine(ex);
            }

        }


        public static void CreateProcess()
        {
            //Process.Start("notepad.exe", @"c:\temp\HelloWorld.txt");
            //Process.Start(@"c:\temp\HelloWorld.txt");

            var app = new Process()
            {
                StartInfo =
                {
                    FileName = @"notepad.exe",
                    Arguments = @"c:\temp\HelloWorld.txt"
                 }
            };


            app.Start();
            app.PriorityClass = ProcessPriorityClass.RealTime;
            app.WaitForExit();
            Console.WriteLine("No more waiting");


            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                if (process.ProcessName == "notepad")
                {
                    process.Kill();
                }
            }
        }

        public static void TestEap()
        {
            WebClient wc = new WebClient();

            wc.DownloadDataCompleted += (s, e) => Console.WriteLine(Encoding.UTF8.GetString(e.Result));
            wc.DownloadDataAsync(new Uri("http://www.engineerspock.com"));
        }
    }
}