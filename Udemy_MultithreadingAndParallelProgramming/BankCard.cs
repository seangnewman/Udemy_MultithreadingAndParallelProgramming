using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Udemy_MultithreadingAndParallelProgramming
{
    class BankCard
    {



        private readonly object _sync = new object();
        private decimal _moneyAmount;
        private decimal _credit;

        public decimal   TotalMoneyAmount { get
            {
                //lock (_sync)
                //{
                //    return _moneyAmount + _credit;
                //}

                //var rw = new ReaderWriterLockSlim();
                //rw.EnterReadLock();
                //decimal result = _moneyAmount + _credit;
                //rw.ExitReadLock();

                using (ReaderWriterLockSlimExt.TakeReaderLock(TimeSpan.FromMilliseconds(3)))
                {
                    return _moneyAmount + _credit;
                }

                
            }
        }

        

        public BankCard(decimal moneyAmount)
        {
            _moneyAmount = moneyAmount;
        }

        public void ReceivePayment(decimal amount)
        {

            //bool lockTaken = false;
            //try
            //{
            //    //Monitor.Enter(_sync, ref lockTaken);
            //    Monitor.TryEnter(_sync, TimeSpan.FromSeconds(10), ref lockTaken);
            //     _moneyAmount += amount;

            //}

            //finally
            //{
            //    if (lockTaken)
            //    {
            //        Monitor.Exit(_sync);
            //    }

            //}

            //var rw = new ReaderWriterLockSlim();
            //rw.EnterWriteLock();
            //_moneyAmount += amount;
            //rw.ExitWriteLock();

            //using (ReaderWriterLockSlimExt.TakeWriterLock(TimeSpan.FromMilliseconds(3)))
            //{
            //    _moneyAmount += amount;
            //}

            var rw = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            rw.EnterWriteLock();
            _moneyAmount += amount;
            rw.ExitWriteLock();


        }

        public void TransferToCard(decimal amount, BankCard recipient)
        {

            //bool lockTaken = false;
            //try
            //{
            //    Monitor.Enter(_sync, ref lockTaken);

            //    _moneyAmount -= amount;
            //    recipient.ReceivePayment(amount);
            //}

            //finally
            //{
            //    if (lockTaken)
            //    {
            //        Monitor.Exit(_sync);
            //    }
            //}

            //lock (_sync)
            //{
            //    _moneyAmount -= amount;
            //    recipient.ReceivePayment(amount);
            //}
            
            using(_sync.Lock(TimeSpan.FromSeconds(3)))
            {
                 _moneyAmount -= amount;
                  recipient.ReceivePayment(amount);
            }

        }

    }
}
