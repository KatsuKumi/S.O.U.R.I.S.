using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOURIS_Server.Slaves
{
    class SlaveHelper
    {
        internal static int GetSlaveID(string content)
        {
            string[] SlaveContent = content.Split('|');
            for (int i = 0; i < SlaveList.List.Count; i++)
            {
                if (SlaveList.List[i].Name.Contains(SlaveContent[0]))
                {
                    return i;
                }
            }
            return 0;
        }
        internal static string GetNextOrder(int ID)
        {
            try
            {
                if (!String.IsNullOrEmpty(SlaveList.List[ID].NextOrder))
                {
                    string tosend = SlaveList.List[ID].NextOrder;
                    SlaveList.List[ID].NextOrder = "";
                    return tosend;
                }
            }
            catch (ArgumentOutOfRangeException)
            {

            }
            return "ok";
        }
    }
}
