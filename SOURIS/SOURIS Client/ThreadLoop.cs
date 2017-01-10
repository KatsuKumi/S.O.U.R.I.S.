using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOURIS_Client
{
    class ThreadLoop
    {
        public static void refreshinfo()
        {
            while (true)
            {
                SocketClient.StartClient(InfoFunc.GetAllInfo());
                Task.Delay(5000).Wait();
            }
        }
    }
}
