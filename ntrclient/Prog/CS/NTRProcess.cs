using System;
using System.Windows.Forms;

namespace ntrclient.Prog.CS
{
    public class NtrProcess
    {
        // 0: pid:
        // 1: 0xPID,
        // 2: pname:
        // 3: name,
        // 4: tid:
        // 5: ID,
        // 6: kpobj:
        // 7: OBJID

        public int Pid;
        public long Tid;
        public string Name;

        public NtrProcess(string process)
        {
            try
            {
                string[] pParts = process.Split(' ');
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
                Pid = Convert.ToInt32(pParts[1].Substring(2, 8), 16);
                //MessageBox.Show("PID: OK!");
                Name = pParts[len - 5].Substring(0, pParts[len - 5].Length - 1);
                //MessageBox.Show("NAME: OK!");
                Tid = Convert.ToInt64(pParts[len - 3].Substring(0, pParts[len - 3].Length - 1), 16);
                //MessageBox.Show("TID: OK!");
                //MessageBox.Show("Process: OK!");
            } catch
            {
                Pid = 0x00;
                Name = "Invalid Message";
                Tid = 0x00;
                MessageBox.Show(@"Invalid Process");
            }
        }
    }
}
