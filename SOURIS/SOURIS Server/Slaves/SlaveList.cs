using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOURIS_Server.Slaves
{
    class SlaveList
    {
        public static List<Slave> List = new List<Slave>();
        public class Slave
        {
            public string Name { get; set; }
            public string Country { get; set; }
            public string Ping { get; set; }
            public string CPU { get; set; }
            public string RAM { get; set; }
            public string Activity { get; set; }
            public string Front { get; set; }
            public string Nextinteract { get; set; }
        }
    }
}
