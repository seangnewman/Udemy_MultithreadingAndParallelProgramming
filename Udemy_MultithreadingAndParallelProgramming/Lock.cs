using System;
using System.Threading;

namespace Udemy_MultithreadingAndParallelProgramming
{
    public struct Lock : IDisposable
    {
        private readonly object _obj;

        public Lock(object obj)
        {
            _obj = obj;
        }
        public void Dispose()
        {
            Monitor.Exit(_obj);
        }
    }
}