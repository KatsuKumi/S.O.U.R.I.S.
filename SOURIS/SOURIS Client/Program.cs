using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Management;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace SOURIS_Client
{

    class Program
    {
        static void Main(string[] args)
        {
            SocketClient.StartClient();
        }
    }
}
