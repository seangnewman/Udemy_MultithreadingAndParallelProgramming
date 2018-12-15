using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Udemy_MultithreadingAndParallelProgramming
{
    public class Protocol
    {
        private readonly IPEndPoint _endpoint;

        public Protocol(IPEndPoint endPoint)
        {
            _endpoint = endPoint;
        }

        public void Send(int opCode, object parameters)
        {
            Task.Run( () => {
                // emulating interoperation with a bank terminal device
                Console.WriteLine("Operation is active");
                Thread.Sleep(3000);
                OnMessageReceived?.Invoke(this, new ProtocolMessage(OperationStatus.Finished));
            });
        }

        public event EventHandler<ProtocolMessage> OnMessageReceived;

    }
}
