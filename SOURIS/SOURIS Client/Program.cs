using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace SOURIS_Client
{
    class Program
    {
        public static string server_host = "62.210.152.119";
        public static int server_port = 8888;
        public static string message = "test swagg";

        static void Main(string[] args)
        {

            //---create a TCPClient object at the IP and port no.---
            TcpClient client = new TcpClient(server_host, server_port);
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(message);

            //---send the text---
            Console.WriteLine("Sending : " + message);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            //---read back the text---
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            Console.ReadLine();
            client.Close();
        }
    }
}
