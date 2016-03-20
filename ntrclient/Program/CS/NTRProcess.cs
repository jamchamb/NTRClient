using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ntrclient
{
    public class NTRProcess
    {
        // 0: pid:
        // 1: 0xPID,
        // 2: pname:
        // 3: name,
        // 4: tid:
        // 5: ID,
        // 6: kpobj:
        // 7: OBJID

        public Int32 pid;
        public Int64 tid;
        public String name;

        public NTRProcess(String process)
        {
            try
            {

                String[] pParts = process.Split(' ');
                int len = pParts.Length;
                /*
                String test = "";
                int i = 0;
                foreach (var p_ in pParts)
                {
                    test += String.Format("{0}: {1}\r\n", i, p_);
                    i++;
                }
                MessageBox.Show(test);
                */
                pid = Convert.ToInt32(pParts[1].Substring(2, 8), 16);
                //MessageBox.Show("PID: OK!");
                name = pParts[len - 5].Substring(0, pParts[len - 5].Length - 1);
                //MessageBox.Show("NAME: OK!");
                tid = Convert.ToInt64(pParts[len - 3].Substring(0, pParts[len - 3].Length - 1), 16);
                //MessageBox.Show("TID: OK!");
                //MessageBox.Show("Process: OK!");
            } catch
            {
                pid = 0x00;
                name = "Invalid Message";
                tid = 0x00;
                MessageBox.Show("Invalid Process");
            }
        }
    }
}
