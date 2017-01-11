using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SOURIS_Client
{
    class OrderClient
    {

        static Socket server = null;
        static MemoryStream ms;
        static IPEndPoint endpoint = null;
        public static void TCPClient(Bitmap bmp)
        {
            server = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
            endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);
            byte[] byteArray = ms.ToArray();
            Task.Delay(1500).Wait();
            server.Connect(endpoint);
            server.Send(byteArray);
        }
    }
}
