using System;
using System.Windows.Forms;
using ntrclient.Prog.CS;
using ntrclient.Prog.CS.GitHub;

namespace ntrclient.Prog.Window
{
    public partial class DebugConsole : Form
    {
        public DebugConsole()
        {
            InitializeComponent();
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            ExecuteCommand(textBox_cmd.Text);
        }

        private void textBox_cmd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ExecuteCommand(textBox_cmd.Text);
            }
        }

        public delegate void DelegateLogAppend(string l);

        public void Addlog(string l)
        {
            l = "> " + l;
            if (!l.Contains("\r\n"))
            {
                l = l.Replace("\n", "\r\n");
            }
            if (!l.EndsWith("\n"))
            {
                l += "\r\n";
            }

            if (textBox_log.InvokeRequired)
            {
                textBox_log.Invoke(new DelegateLogAppend(textBox_log.AppendText), l);
            }
            else
            {
                textBox_log.AppendText(l);
            }
        }

        public void ExecuteCommand(string n)
        {
            Addlog(n);
            textBox_cmd.Clear();
            string[] args = n.Split(' ');
            string cmd = args[0];

            if (cmd == "close")
            {
                Hide();
            }
            else if (cmd == "gs_use")
            {
                if (args.Length == 1)
                {
                    Addlog(string.Format("GS_USE: {0}", Program.Sm.GsUsed));
                }
                else if (args.Length >= 3)
                {
                    try
                    {
                        int a = Convert.ToInt32(args[2]);

                        if (args[1] == "set")
                        {
                            Program.Sm.GsUsed = a;
                            Addlog(string.Format("GS_USE: {0}", Program.Sm.GsUsed));
                        }
                        else if (args[1] == "add")
                        {
                            Program.Sm.GsUsed += a;
                            if (Program.Sm.GsUsed < 0)
                            {
                                Program.Sm.GsUsed = 0;
                            }
                            Addlog(string.Format("GS_USE: {0}", Program.Sm.GsUsed));
                        }
                        else
                        {
                            Addlog("USAGE: gs_use <cmd> <amount>");
                        }
                    }
                    catch (Exception)
                    {
                        Addlog("USAGE: gs_use " + args[1] + " <amount>");
                    }
                }
                else
                {
                    Addlog("USAGE: gs_use <cmd> <amount>");
                }
            }
            else if (cmd == "update")
            {
                string nVersion = Octo.GetLastVersionName();
                string nBody = Octo.GetLastVersionBody();
                MessageBox.Show(
                    @"A new Update has been released!" + Environment.NewLine +
                    nVersion + Environment.NewLine + Environment.NewLine +
                    nBody
                    );
            }
        }

        private void DebugConsole_Shown(object sender, EventArgs e)
        {
            textBox_cmd.Focus();
        }

        private void DebugConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}