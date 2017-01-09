using System;
using System.Net.Sockets;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SOURIS_Server.Sockets
{
    class connect
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
        //<------------------------------- Début du sniffing de packet ---------------------------------->
        public static void StartListening()
        {
            byte[] bytes = new Byte[1024];
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
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
                    Form.FormUpdate.addlistbox("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Form.FormUpdate.addlistbox(e.ToString());
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
                int count = 0;
                if (content.Contains("|"))
                {
                    Form.FormUpdate.updateSlave(content);
                }
                string msgback = "Ok";
                if (!String.IsNullOrEmpty(Slaves.SlaveList.List[count].Nextinteract))
                {
                    msgback = Slaves.SlaveList.List[count].Nextinteract;
                    Slaves.SlaveList.List[count].Nextinteract = "";
                }
                Send(handler, msgback);
            }
        }
        //<--------------------------- Début du renvoie de la réponse -------------------------------------->

        private static void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        //<-------------------------Renvoie de la réponse---------------------------------------->
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
                Form.FormUpdate.addlistbox($"Sent {bytesSent} bytes to client.");
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Form.FormUpdate.addlistbox(e.ToString());
            }
        }
        //<----------------------------------------------------------------->
    }
}
