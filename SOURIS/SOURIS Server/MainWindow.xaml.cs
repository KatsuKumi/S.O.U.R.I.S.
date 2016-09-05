using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SOURIS_Server
{
    public class Slave
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Ping { get; set; }
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string Activity { get; set; }
        public string Front { get; set; }
    }
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
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

    public class AsynchronousSocketListener
    {
    }
    public partial class MainWindow : MetroWindow
    {
        public List<Slave> Slavelist = new List<Slave>();
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public void StartListening()
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
                    addlistbox("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                addlistbox(e.ToString());
            }

            addlistbox("\nPress ENTER to continue...");
            Console.Read();

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
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
                addlistbox($"Read {content.Length} bytes from socket. \n Data : {content}");
                Send(handler, content);
            }
        }

        private void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
                addlistbox($"Sent {bytesSent} bytes to client.");
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                addlistbox(e.ToString());
            }
        }
        public void addlistbox(string message)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listBox.Items.Add(message)));
        }
        public MainWindow()
        {
            InitializeComponent();
            var CheckerStart = Task.Factory.StartNew(() => { StartListening(); });
            addlistbox(" >> Server Started");
            listView1.ItemsSource = Slavelist;
            Slavelist.Add(new Slave() { Name = "Test/Desktop", Country = "France", Ping = "88 ms", CPU = "70%", RAM = "20%", Activity = "Idle : 01:20:12", Front = "Cs:go" });
            listView1.Items.Refresh();
        }
        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedIndex == -1)
            {
                return;
            }
            //Delete Func
        }
    }
}
