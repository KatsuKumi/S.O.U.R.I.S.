using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOURIS_Server
{
    class TestFunc
    {
        public static Byte[] Screenshot()
        {
            Bitmap resolution;
            resolution = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Size size = new Size(resolution.Width, resolution.Height);
            Graphics memoryGraphics = Graphics.FromImage(resolution);
            memoryGraphics.CopyFromScreen(0, 0, 0, 0, size);
            ImageConverter converter = new ImageConverter();
            Byte[] sendBytes = (byte[])converter.ConvertTo(resolution, typeof(byte[]));
            return sendBytes;
        } 
    }
}
