using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SOURIS_Client
{
    class OrderClient
    {

        static Socket server = null;
        static MemoryStream ms;
        static IPEndPoint endpoint = null;
        public static void TCPClient(Bitmap bmp)
        {
            server = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
            endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            ms = new MemoryStream();
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, 5L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp.Save(@"swag.jpg", jpgEncoder, myEncoderParameters);
            bmp.Save(ms, jpgEncoder, myEncoderParameters);
            Console.WriteLine(ms.Length);
            byte[] byteArray = ms.ToArray();
            Task.Delay(1500).Wait();
            server.Connect(endpoint);
            Console.WriteLine("test");
            server.Send(byteArray);
            Console.WriteLine("test");
            server.Disconnect(true);
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
