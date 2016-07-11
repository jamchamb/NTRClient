using ntrclient.Prog.CS;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ntrclient.Extra
{
    public class BugReporter
    {
        private readonly Exception _e;
        private readonly string _additionalInformation;
        private readonly string _ntrVersion;
        private readonly bool _showMessagebox;


        public BugReporter(Exception e, string additionalInformation, bool showMessagebox = true)
        {
            _e = e;
            _additionalInformation = additionalInformation;
            _ntrVersion = @"**todo**";
            _showMessagebox = showMessagebox;

            SaveLog();
        }

        private void SaveLog()
        {
            try
            {
                string log = @"--- NTR Debugger Bug report ---" + Environment.NewLine +
                             @"Please upload this bugreport to pastebin or similar and send it to Shadowtrance" +
                             Environment.NewLine + Environment.NewLine +
                             @"------------------------------" + Environment.NewLine + Environment.NewLine;

                log += @"Additional Information: " + _additionalInformation + Environment.NewLine;
                log += @"Version of NTR: " + _ntrVersion + Environment.NewLine + Environment.NewLine;
                log += @"------------------------------" + Environment.NewLine + Environment.NewLine;
                log += @"Exception stacktrace: " + Environment.NewLine + _e + Environment.NewLine;
                log += @"------------------------------" + Environment.NewLine + Environment.NewLine;
                if (Program.GCmdWindow != null)
                {
                    log += Program.GCmdWindow?.GetLog() ?? @"No log" + Environment.NewLine + Environment.NewLine;
                    log += @"------------------------------" + Environment.NewLine + Environment.NewLine;
                    log += @"System information: [PROCESSES]" + Environment.NewLine;

                    log = Program.GCmdWindow.Processes.Aggregate(log,
                        (current, process) =>
                            current +
                            (string.Format("{0} | {1:X} : {2} [{3:X}]",
                                Program.GCmdWindow.FillString(Program.GCmdWindow.CheckSystem(process.Name), 6),
                                process.Pid, process.Name, process.Tid) + Environment.NewLine));
                    log += Environment.NewLine + @"------------------------------" + Environment.NewLine +
                           Environment.NewLine;
                }

                log += @"This is the end of the bugreport. ^_^";

                Directory.CreateDirectory("bugreports");
                string name = @"bugreports/bugreport-" + GetTimestamp(DateTime.Now) + @".txt";
                File.WriteAllText(name, log);

                if (_showMessagebox)
                    MessageBox.Show(@"Saved bugreport to " + name);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
