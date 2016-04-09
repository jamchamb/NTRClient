using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ntrclient
{
    public partial class DebugConsole : Form
    {
        public DebugConsole()
        {
            InitializeComponent();
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            executeCommand(textBox_cmd.Text);
        }

        private void textBox_cmd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                executeCommand(textBox_cmd.Text);
            }
        }

        public delegate void delegate_logAppend(String l);
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
                textBox_log.Invoke(new delegate_logAppend(textBox_log.AppendText), l);
            }
            else
            {
                textBox_log.AppendText(l);
            }
        }

        public void executeCommand(String n)
        {
            Addlog(n);
            textBox_cmd.Clear();
            String[] args = n.Split(' ');
            String cmd = args[0];

            if (cmd == "close")
            {
                Hide();
            } else if (cmd == "gs_use") {
                if (args.Length == 1)
                {
                    Addlog(String.Format("GS_USE: {0}", Program.sm.gs_used));
                } else if (args.Length >= 3)
                {
                    try
                    {
                        int a = Convert.ToInt32(args[2]);

                        if (args[1] == "set")
                        {
                            Program.sm.gs_used = a;
                            Addlog(String.Format("GS_USE: {0}", Program.sm.gs_used));
                        } else if (args[1] == "add")
                        {
                            Program.sm.gs_used += a;
                            if (Program.sm.gs_used < 0)
                            {
                                Program.sm.gs_used = 0;
                            }
                            Addlog(String.Format("GS_USE: {0}", Program.sm.gs_used));
                        } else
                        {
                            Addlog("USAGE: gs_use <cmd> <amount>");
                        }
                    }
                    catch (Exception)
                    {
                        Addlog("USAGE: gs_use "+args[1]+" <amount>");
                    }
                } else
                {
                    Addlog("USAGE: gs_use <cmd> <amount>");
                }
            } else if (cmd == "update")
            {

                String n_version = Octo.getLastVersionName();
                String n_body = Octo.getLastVersionBody();
                MessageBox.Show(
                    "A new Update has been released!\r\n" +
                    n_version + "\r\n\r\n" +
                    n_body
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
