using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ntrclient
{
    public class Memregion
    {
        public int start, end, length;

        public Memregion(String mem)
        {

            /**
                0 = start
                1 = -
                2 = end
                3 = ,
                4 = size:
                5 = length

                1, 3, 4 as checks?

            **/

            String[] memparts = mem.Split(' ');
            try
            {
                start = Convert.ToInt32(memparts[0], 16);
                end = Convert.ToInt32(memparts[2], 16);
                length = Convert.ToInt32(memparts[5], 16);
            }
            catch (Exception e)
            {
                start = 0;
                end = 0;
                length = 0;

                Program.gCmdWindow.textBox_memdebug.AppendText("\r\n" + e);
            }
        }
    }
}
