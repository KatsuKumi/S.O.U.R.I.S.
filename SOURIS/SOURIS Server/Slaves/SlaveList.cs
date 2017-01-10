using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOURIS_Server.Slaves
{
    class SlaveList
    {
        public static ObservableCollection<Slave> List = new ObservableCollection<Slave>();
        public class Slave
        {
            public string Name { get; set; }
            public string Country { get; set; }
            public string Ping { get; set; }
            public string CPU { get; set; }
            public string RAM { get; set; }
            public string Activity { get; set; }
            public string Front { get; set; }
            public string IP { get; set; }
        }
    }
}
