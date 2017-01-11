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
        private static  int port = 1234;
        private static  IPAddress addr = IPAddress.Any;
        public static bool waitingforscreenshot = true;
        public static void ListenScreenshot()
        {
            Listening();
        }
        private static void Listening()
        {
            sock = new Socket(
               addr.AddressFamily,
               SocketType.Stream,
               ProtocolType.Tcp);
            sock.Bind(new IPEndPoint(addr, port));
            sock.Listen(100);
            sock.BeginAccept(OnConnectRequest, sock);
        }
        private static void OnConnectRequest(IAsyncResult result)
        {

            Socket sock = (Socket)result.AsyncState;
            waitingforscreenshot = false;
            Connection newConn = new Connection(sock.EndAccept(result));
        }

    }
    class Connection
    {
        private Socket sock;
        private Encoding encoding = Encoding.UTF8;
        public const int BufferSize = 500 * 2048;
        public byte[] dataRcvBuf = new byte[BufferSize];

        public Connection(Socket s)
        {
            this.sock = s;
            this.BeginReceive();
        }
        
        private void BeginReceive()
        {
            sock.BeginReceive(
                    dataRcvBuf, 0,
                    dataRcvBuf.Length,
                    SocketFlags.None,
                    new AsyncCallback(this.OnBytesReceived),
                    this);
        }
        
        protected void OnBytesReceived(IAsyncResult result)
        {
            try
            {
                int nBytesRec = this.sock.EndReceive(result);
                if (nBytesRec <= 0)
                {
                    this.sock.Close();
                    return;
                }
                string strReceived = this.encoding.GetString(
                    this.dataRcvBuf, 0, nBytesRec);

                File.WriteAllBytes("yolo.png", dataRcvBuf);
                this.sock.Close();
            }
            catch (System.Net.Sockets.SocketException)
            {
                this.sock.Close();
            }
            TCPServer.waitingforscreenshot = true;
        }
    }
}
