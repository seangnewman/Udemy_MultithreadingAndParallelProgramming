using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Udemy_MultithreadingAndParallelProgramming
{
    class Chapter7
    {
        private static bool _done;

        private static void MultipleyXByY(ref int val, int factor)
        {
            var spinWait = new SpinWait();
            while (true)
            {
                int snapshot1 = val;

                int calc = snapshot1 * factor;

                int snapshot2 = Interlocked.CompareExchange(ref val, calc, snapshot1);

                if (snapshot1 == snapshot2)
                {
                    // No one preempted
                    return;
                }
                spinWait.SpinOnce();
            }
        }

        private static void SpinningExamples()
        {
            Task.Run(() =>
            {

                try
                {
                    Console.WriteLine("Task Started.");
                    Thread.Sleep(1000);
                    Console.WriteLine("Task is done");
                }
                finally
                {
                    _done = true;
                }

            });

            //while (!_done)
            //{
            //    Thread.Sleep(10);
            //}

            SpinWait.SpinUntil(() =>
            {
                Thread.MemoryBarrier();
                return _done;
            });
            Console.WriteLine("The end of the program");
        }

    }
}
