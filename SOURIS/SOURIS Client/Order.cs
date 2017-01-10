using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("screeen");
        }
        public static void error()
        {

        }
    }
}
