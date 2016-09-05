using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var CheckerStart = Task.Factory.StartNew(() => { StartListening(); });
        }

        const int PORT_NO = 8888;
        public TcpListener listener = new TcpListener(PORT_NO);
        public void StartListening()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listBox.Items.Add(" >> Server Started")));
            while (true)
            {
                //---listen at the specified IP and port no.---
                listener.Start();
                //---incoming client connected---
                var CheckerStart = Task.Factory.StartNew(() => { listenstream(listener.AcceptTcpClient()); });
            }
        }
        public void listenstream(TcpClient client)
        {
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];

            //---read incoming stream---
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            //---convert the data received into a string---
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listBox.Items.Add("Received : " + dataReceived)));

            //---write back the text to the client---

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listBox.Items.Add("Sending back : " + dataReceived)));
            nwStream.Write(buffer, 0, bytesRead);
            client.Close();
            listener.Stop();
        }
    }
}
