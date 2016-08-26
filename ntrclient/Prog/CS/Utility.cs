using ntrclient.Extra;
using System;
using System.Diagnostics;
using System.Linq;

namespace ntrclient.Prog.CS
{
    public class Utility
    {
        public static int RunCommandAndGetOutput(string exeFile, string args, ref string output)
        {
            try
            {
                Process proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = exeFile,
                        Arguments = args,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardError = true
                    }
                };
                proc.Start();
                proc.WaitForExit();
                var processOutput = proc.StandardError.ReadToEnd();
                processOutput += proc.StandardOutput.ReadToEnd();
                var ret = proc.ExitCode;
                output = processOutput;
                proc.Close();
                return ret;
            }
            catch (System.ComponentModel.Win32Exception e) {
                output = String.Format ("Could not open '{0}'. Make sure it is on your executable path.", exeFile);
                Console.Error.WriteLine (e);
                return -1;
            }
            catch (Exception e)
            {
                output = e.Message;
                BugReporter br = new BugReporter(e, "Run CMD exception", false);
                return -1;
            }
        }

        public static string ConvertByteArrayToHexString(byte[] arr)
        {
            return arr.Aggregate("", (current, t) => current + (t.ToString("X2") + " "));
        }
    }
}