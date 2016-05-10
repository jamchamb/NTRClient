using ntrclient.Prog.CS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ntrclient.Extra
{
    public class BugReporter
    {
        private Exception e;
        private String additionalInformation;
        private String NtrVersion;
        private bool showMessagebox;
        

        public BugReporter(Exception e, String additionalInformation, bool showMessagebox = true)
        {
            this.e = e;
            this.additionalInformation = additionalInformation;
            NtrVersion = @"**todo**";
            this.showMessagebox = showMessagebox;

            saveLog();
        }

        private void saveLog()
        {
            String log = @"--- NTR Debugger Bug report ---" + Environment.NewLine +
                @"Please upload this bugreport to pastebin or similar and send it to imthe666st" + Environment.NewLine + Environment.NewLine +
                @"------------------------------" + Environment.NewLine + Environment.NewLine;

            log += @"Additional Information: " + additionalInformation + Environment.NewLine;
            log += @"Version of NTR: " + NtrVersion + Environment.NewLine + Environment.NewLine;
            log += @"------------------------------" + Environment.NewLine + Environment.NewLine;
            log += @"Exception stacktrace: " + Environment.NewLine + e.ToString() + Environment.NewLine;
            log += @"------------------------------" + Environment.NewLine + Environment.NewLine;
            if (Program.GCmdWindow != null)
            {
                log += Program.GCmdWindow?.GetLog() ?? @"No log" + Environment.NewLine + Environment.NewLine;
                log += @"------------------------------" + Environment.NewLine + Environment.NewLine;
                log += @"System information: [PROCESSES]" + Environment.NewLine;
                foreach (NtrProcess process in Program.GCmdWindow.Processes)
                {
                    log += string.Format("{0} | {1:X} : {2} [{3:X}]", Program.GCmdWindow.FillString(Program.GCmdWindow.CheckSystem(process.Name), 6), process.Pid, process.Name, process.Tid) + Environment.NewLine;
                }
                log += Environment.NewLine + @"------------------------------" + Environment.NewLine + Environment.NewLine;
                log += @"Current process: " + Program.GCmdWindow.comboBox_processes.Text + Environment.NewLine + Environment.NewLine;
                log += @"------------------------------" + Environment.NewLine + Environment.NewLine;
            }

            log += @"This is the end of the bugreport. ^_^";

            Directory.CreateDirectory("bugreports");
            String name = @"bugreports/bugreport-" + GetTimestamp(DateTime.Now) + @".txt";
            File.WriteAllText(name, log);

            if (showMessagebox)
                MessageBox.Show(@"Saved bugreport to " + name);
        }


        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
