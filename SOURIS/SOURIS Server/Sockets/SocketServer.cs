using System;
using System.Net.Sockets;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Threading.Tasks;

namespace SOURIS_Server.Sockets
{
    class SocketServer
    {
        //<------------------------------- Déclaration des classes et variables---------------------------------->
        public class StateObject
        {
            // Client  socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();
        }

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static bool WaitForInteract = false;
        //<------------------------------- Début du sniffing de packet ---------------------------------->
        public static void StartListening()
        {
            byte[] bytes = new Byte[1024];
#pragma warning disable CS0618 // Le type ou le membre est obsolète
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
#pragma warning restore CS0618 // Le type ou le membre est obsolète
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);
                while (true)
                {
                    allDone.Reset();
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);
                    Form.FormUpdate.addlistbox("Waiting for a connection...");
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                if (e.ToString().Contains("0x80004005"))
                {
                    Task.Delay(1000).Wait();
                    Form.FormUpdate.ShowDialog("Server already running !", "Oops, look like the server is already running." + Environment.NewLine + "Or maybe the server port ( 11000 ) is already used." + Environment.NewLine + "The server will not work." );
                    Form.FormUpdate.addlistbox("Cannot listen for connection, server running or port used.");
                }
                else
                {
                    Form.FormUpdate.addlistbox(e.ToString());
                }
            }

            Console.Read();

        }
        //<--------------------------- Acceptation des packets -------------------------------------->
        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        //<------------------------ Lecture des packets ----------------------------------------->
        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));
                content = state.sb.ToString();

                Form.FormUpdate.addlistbox($"Read {content.Length} bytes from socket. \nData : {content}");
                if (content.Contains("THISISALLINFOFUNC"))
                {
                    Form.FormUpdate.updateSlave(content);
                }
                Send(handler, Slaves.SlaveHelper.GetNextOrder(Slaves.SlaveHelper.GetSlaveID(content)));
            }

        }
        //<------------------------ Début de l'envoie de la réponse---------------------------------------->
        private static void Send(Socket handler, String data)
        {
            if (data != "ok")
            {
                WaitForInteract = false;
            }
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }
        //<------------------------ Envoie de la réponse---------------------------------------->
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception)
            {
            }
        }
        //<----------------------------------------------------------------->
    }
    
}
