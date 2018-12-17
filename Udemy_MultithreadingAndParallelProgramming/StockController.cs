using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy_MultithreadingAndParallelProgramming
{
    public class StockController
    {
        readonly ConcurrentDictionary<string, int> _stock = new ConcurrentDictionary<string, int>();

        public void BuyBook(string item, int quantity)
        {
            _stock.AddOrUpdate(item, quantity, (key, oldValue) => oldValue + quantity);
        }

        public bool TryRemoveBookFromStock(string item)
        {
             if(_stock.TryRemove(item, out int val))
            {
                Console.WriteLine($"How much was removed: {val}");
                return true;
            }

            return false;
        }

        public bool TrySellBook(string item)
        {
            bool success = false;

            _stock.AddOrUpdate(item,
                                                     itemName => { success = false;  return 0; },
                                                     (itemName, oldValue) =>
                                                     {
                                                         if (oldValue == 0)
                                                         {
                                                             success = false;
                                                             return 0;
                                                         }
                                                         else
                                                         {
                                                             success = true;
                                                             return oldValue - 1;
                                                         }
                                                     });
            return success;

        }

        public void DisplayStatus()
        {
            foreach (var pair in _stock)
            {
                Console.WriteLine($"{pair.Key} : {pair.Value}");
            }
        }


    }
}
