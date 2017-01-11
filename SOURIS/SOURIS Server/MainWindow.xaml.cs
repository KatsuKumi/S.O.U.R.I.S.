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
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>

    public partial class MainWindow : MetroWindow
    {
        internal static MainWindow main;
        public MainWindow()
        {
            InitializeComponent();
            main = this;
            var Listenstart = Task.Factory.StartNew(() => {Sockets.SocketServer.StartListening(); });
            Form.FormUpdate.startingtask();
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Form.FormUpdate.deleteline();
        }
        private void MenuItemScreenshot_Click(object sender, RoutedEventArgs e)
        {
            Slaves.SlaveFunc.Screenshot();
        }
    }
}
