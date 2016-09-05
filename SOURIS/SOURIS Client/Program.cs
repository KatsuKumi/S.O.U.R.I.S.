using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace SOURIS_Client
{
    class Program
    {
        public static string server_host = "90.60.70.205";
        public static int server_port = 8888;
        public static string message = "test swagg";

        static void Main(string[] args)
        {
            bool is_ok = false;
            NetworkStream stream;
            TcpClient tcpClient = new TcpClient();

            Console.WriteLine("[ ] En attente du server.");
            while (!is_ok)
            {
                try
                {
                    tcpClient.Connect(server_host, server_port);
                    is_ok = true;
                }
                catch (SocketException e) { }
                Thread.Sleep(10);
            }
            Console.WriteLine("[ ] Je suis maintenant connecté.");

            stream = tcpClient.GetStream();
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            byte[] array = new byte[500000];
            stream.Read(array, 0, tcpClient.ReceiveBufferSize);
            string @string = Encoding.ASCII.GetString(array);
            tcpClient.Close();
        }
    }
}
