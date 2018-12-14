﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Udemy_MultithreadingAndParallelProgramming
{

    
    public class AmpEap
    {

        private const string FilePath = @"c:\temp\demo.txt";

        public static void Test()
        {
            //FileStream fs = new FileStream(FilePath,
            //                                                            FileMode.OpenOrCreate,
            //                                                            FileAccess.Write,
            //                                                            FileShare.None,
            //                                                            8,
            //                                                            true);
            //string content = "A quick brown fox jumps over the lazy dog";
            //byte[] data = Encoding.Unicode.GetBytes(content);

            //Task task = fs.WriteAsync(data, 0, data.Length);
            //task.ContinueWith( (t =>
            //  {
            //      fs.Close();
            //      TestAsyncTaskRead();
            //  }));

            TestFromAsync();
            Console.Read();

        }

        private static void TestAsyncTaskRead()
        {
            FileStream fs = new FileStream(FilePath,
                                                                           FileMode.OpenOrCreate,
                                                                           FileAccess.Read,
                                                                           FileShare.None,
                                                                           8,
                                                                           true);
            byte[] data = new byte[1024];

            Task<int> readTask = fs.ReadAsync(data, 0, data.Length);

            readTask.ContinueWith( t => {
                fs.Close();
                string content = Encoding.Unicode.GetString(data, 0, t.Result);
                Console.WriteLine($"Read completed.  Content is :{content}");
                });


        }

        private static void TestTaskWrite()
        {

        }

        public static void TestEap()
        {
            WebClient wc = new WebClient();

            Task<byte[]> task = wc.DownloadDataTaskAsync(new Uri("http://www.engineerspock.com"));
            task.ContinueWith(t => Console.WriteLine(Encoding.UTF8.GetString(t.Result)));

            Console.ReadKey();
        }

        public static void TestEapTask()
        {
            WebClient wc = new WebClient();

            Task<byte[]> task = wc.DownloadDataTaskAsync(new Uri("http://www.engineerspock.com"));

            task.ContinueWith(t => Console.WriteLine(Encoding.UTF8.GetString(t.Result)));
        }

        public static void TestFromAsync()
        {
            FileStream fs = new FileStream(FilePath,
                                                                          FileMode.OpenOrCreate,
                                                                          FileAccess.ReadWrite,
                                                                          FileShare.None,
                                                                          8,
                                                                          true);

            string content = "A quick brown fox jumps over the lazy dog";
            byte[] buffer = Encoding.Unicode.GetBytes(content);

            var writeChunk = Task.Factory.FromAsync(fs.BeginWrite,
                                                                                                fs.EndWrite,
                                                                                                buffer, 
                                                                                                0,
                                                                                                buffer.Length,
                                                                                                null
                                                                                                );
            writeChunk.ContinueWith(t => 
                    {
                        fs.Position = 0;
                        var data = new byte[buffer.Length];
                        var readChunk = Task<int>.Factory.FromAsync(fs.BeginRead,
                                                                                                                     fs.EndRead,
                                                                                                                     data,
                                                                                                                     0,
                                                                                                                     data.Length,
                                                                                                                     0);
                        readChunk.ContinueWith(read => {
                            string readResult = Encoding.Unicode.GetString(data, 0, read.Result);
                            Console.WriteLine(readResult);
                        });
                    });


        }

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

            FileStream fs = new FileStream(FilePath,
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




    }
}
