using System;
using System.Collections.Generic;
using System.Linq;
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
            var CheckerStart = Task.Factory.StartNew(() => { Tpcserver(); });
        }
        public void Tpcserver()
        {

            TcpListener serverSocket = new TcpListener(8888);
            int requestCount = 0;
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>  listBox.Items.Add(" >> Server Started")));
            clientSocket = serverSocket.AcceptTcpClient();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listBox.Items.Add(" >> Accept connection from client")));
            
            requestCount = 0;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>listBox.Items.Add(" >> Data from client - " + dataFromClient)));
                    string serverResponse = "Last Message from client" + dataFromClient;
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listBox.Items.Add(" >> " + serverResponse)));
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listBox.Items.Add(ex.ToString())));
                }
            }
            clientSocket.Close();
            serverSocket.Stop();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listBox.Items.Add(" >> exit")));
            Console.ReadLine();
        }
    }
}
