using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Udemy_MultithreadingAndParallelProgramming
{
    public class SalesManager
    {
        public string Name { get; }

        public SalesManager(string id)
        {
            Name = id;
        }

        public void StartWork(StockController stockController, TimeSpan workDay)
        {
            Random rand = new Random((int)DateTime.UtcNow.Ticks);
            DateTime start = DateTime.UtcNow;

            while(DateTime.UtcNow - start < workDay)
            {
                Thread.Sleep(rand.Next(50));
                int generatedNumber = rand.Next(10);
                bool shouldPurchase = generatedNumber % 2 == 0;
                bool shouldRemove = generatedNumber == 9;
                string itemName = RemoteBookStock.Books[rand.Next(RemoteBookStock.Books.Count)];

                if (shouldPurchase)
                {
                    int quantity = rand.Next(9) + 1;
                    stockController.BuyBook(itemName, quantity);
                    DisplayPurchase(itemName, quantity);

                }
                else if(shouldRemove)
                {
                    stockController.TryRemoveBookFromStock(itemName);
                    DisplayRemoveAttempt(itemName);
                }
                else
                {
                    bool success = stockController.TrySellBook(itemName);
                    DisplaySaleAttempt(success, itemName);
                }

                Console.WriteLine($"Sales Manager {Name} finished it's work!");
            }
        }

        private void DisplayPurchase(string itemName, int quantity)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId }: {Name} bought {quantity} of  {itemName}");
        }

        private void DisplaySaleAttempt(bool success, string itemName)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(success? $"Thread {threadId} : {Name} sold {itemName}" : $"Thread {threadId}:{Name}:Out of stock of {itemName}");
        }

        private void DisplayRemoveAttempt(string itemName)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId }: {Name} removed  {itemName}");
        }
    }
}
