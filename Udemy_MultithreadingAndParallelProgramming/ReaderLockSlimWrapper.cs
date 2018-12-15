using System.Threading;

namespace Udemy_MultithreadingAndParallelProgramming
{
    public class ReaderLockSlimWrapper
    {
        private ReaderWriterLockSlim rwlock;

        public ReaderLockSlimWrapper(ReaderWriterLockSlim rwlock)
        {
            this.rwlock = rwlock;
        }
    }
}