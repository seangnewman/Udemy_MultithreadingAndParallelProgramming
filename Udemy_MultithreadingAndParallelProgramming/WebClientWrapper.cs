using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Udemy_MultithreadingAndParallelProgramming
{
    class WebClientWrapper
    {

        private WebClient wc = new WebClient();

        private async Task LongRunningOperation(CancellationToken t)
        {
            if(!t.IsCancellationRequested)
            {
                using (CancellationTokenRegistration ctr = t.Register( () => wc.CancelAsync()))
                {
                    wc.DownloadStringAsync( new Uri("http://www.newmanuevers.com"));
                }
            }
        }
    }
}
