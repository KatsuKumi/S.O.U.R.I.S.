using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SOURIS_Server.Sockets
{

    class TCPServer
    {
        private static Socket sock;
        private static IPAddress addr = IPAddress.Any;
        public static bool waitingforscreenshot = false;
        public static void ListenScreenshot()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 1234);
            server.Start();
            TcpClient client = server.AcceptTcpClient();  
            NetworkStream ns = client.GetStream();
            byte[] msg = new byte[200 * 1024];
            ns.Read(msg, 0, msg.Length);
            Form.FormUpdate.addlistbox("Test Screenshot : Saving");
            File.WriteAllBytes("yolo2.png", msg);
            server.Stop();
        }

    }
}
