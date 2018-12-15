using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Udemy_MultithreadingAndParallelProgramming
{
    class BankTerminal
    {
        private readonly Protocol _protocol;
        //private readonly ManualResetEventSlim _operationSignal = new ManualResetEventSlim();
        private readonly AutoResetEvent _operationSignal = new AutoResetEvent(false);


        public BankTerminal(IPEndPoint endPoint)
        {
            _protocol = new Protocol(endPoint);
            _protocol.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, ProtocolMessage e)
        {
            if (e.Status == OperationStatus.Finished)
            {
                System.Console.WriteLine("Signaling!");
                _operationSignal.Set();
                
            }
            
        }

        public Task Purchase(decimal amount)
        {

            return Task.Run( () => {
                const int purchaseOpCode = 1;
                _protocol.Send(purchaseOpCode, amount);

                //_operationSignal.Reset();
                System.Console.WriteLine("Waiting for Signal");
                _operationSignal.WaitOne();
            });
           
        }
    }
}
