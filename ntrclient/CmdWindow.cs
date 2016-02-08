using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ntrclient
{
    public partial class CmdWindow : Form
    {
		public delegate void LogDelegate(string l);
        public LogDelegate delAddLog;
        public List<Memregion> memregions = new List<Memregion>();


        public CmdWindow()
        {
            delAddLog = new LogDelegate(Addlog);

            InitializeComponent();
        }

        public void Addlog(string l) {
			if (!l.Contains("\r\n")) {
				l = l.Replace("\n", "\r\n");
			}
            if (!l.EndsWith("\n")) {
                l += "\r\n";
            }
            txtLog.AppendText(l);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

		private void txtCmd_TextChanged(object sender, EventArgs e) {

		}
	    String runCmd(String cmd) {
			try {
				Addlog("> " + cmd);
				object ret = Program.pyEngine.CreateScriptSourceFromString(cmd).Execute(Program.globalScope);
				if (ret != null) {
					Addlog(ret.ToString());
                    return ret.ToString();
				} else {
					Addlog("null");
                    return "";
				}
			}
			catch (Exception ex) {
				Addlog(ex.Message);
				Addlog(ex.StackTrace);
                return "";
			}
		}
		private void txtCmd_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				string cmd =  txtCmd.Text  ;
				txtCmd.Clear();
				runCmd(cmd);

			}
		}

		void updateProgress() {
			string text = "";
			if (Program.ntrClient.progress != -1) {
				text = String.Format("{0}%", Program.ntrClient.progress);
			}
			toolStripStatusLabel1.Text = text;
		}
		private void timer1_Tick(object sender, EventArgs e) {
			try {
				updateProgress();
				Program.ntrClient.sendHeartbeatPacket();
				
			} catch(Exception ex) {
			}
		}

		private void CmdWindow_Load(object sender, EventArgs e) {
            Addlog("NTR debugger by cell9 - Mod by imthe666st");
			runCmd("import sys;sys.path.append('.\\python\\Lib')");
			runCmd("for n in [n for n in dir(nc) if not n.startswith('_')]: globals()[n] = getattr(nc,n)    ");
			Addlog("Commands available: ");
			runCmd("repr([n for n in dir(nc) if not n.startswith('_')])");
		}

		private void CmdWindow_FormClosed(object sender, FormClosedEventArgs e) {
			Program.saveConfig();
			Program.ntrClient.disconnect();
		}

		private void ToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void CommandToolStripMenuItem_Click(object sender, EventArgs e) {
			(new QuickCmdWindow()).Show();
		}

		private void CmdWindow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
		

		}

		private void CmdWindow_KeyDown(object sender, KeyEventArgs e) {
			if (e.Control) {
				int t = e.KeyValue;
				if (t >= 48 && t <= 57) {
					runCmd(Program.sm.quickCmds[t-48]);
					e.SuppressKeyPress = true;

				}
			}
			
		}

		private void toolStripStatusLabel1_Click(object sender, EventArgs e) {

		}

        private void asmScratchPadToolStripMenuItem_Click(object sender, EventArgs e) {
            (new AsmEditWindow()).Show();
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            textBox_Ip.Text = "192.168.0.11";
            runCmd("connect('" + textBox_Ip.Text + "', 8000)");
        }

        private void button_processes_Click(object sender, EventArgs e)
        {
            runCmd("listprocess()");
        }

        private void button_memlayout_Click(object sender, EventArgs e)
        {
            // I'll edit this method later.
            // Don't change this yet! 
            String memlayout = runCmd("memlayout(pid=0x" + textBox_pid.Text + ")");

        }

        private void button_hello_Click(object sender, EventArgs e)
        {
            runCmd("sayhello()");
        }

        private void button_dump_Click(object sender, EventArgs e)
        {
            String filename = textBox_dump_file.Text;
            if (filename.Contains(".")) {
                filename = filename.Split('.')[0];
            }
            Memregion mem = memregions[comboBox_memregions.SelectedIndex];
            runCmd(String.Format("data(0x{0:X}, 0x{1:X}, filename='{2}', pid=0x{3})", mem.start, mem.length, filename, textBox_pid.Text));
        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            runCmd("disconnect()");
        }

        public void setMemregions(String memlayout)
        {
            if (!memlayout.Contains("\r\n"))
            {
                memlayout = memlayout.Replace("\n", "\r\n");
            }
            txt_memlayout.Text = memlayout;
        }
        public delegate void setMemregionsCallback(String memlayout);

        private void button_dummy_memregion_Click(object sender, EventArgs e)
        {
            // This is debug only
            // This code will be removed later
            // This code is horrible and should be removed as soon as possible
            // I like cakes


            // Init memregion
            Memregion mem = new Memregion(textBox_memregion.Text);

            // read all the new values.
            int start = mem.start;
            int end = mem.end;
            int length = mem.length;

            // Recreate the original string
            // 00100000 - 00847fff, size: 00748000

            // I know it's not hex yet. 
            String memregion = String.Format("{0:X} - {1:X} , size: {2:X}", start, end, length);
            textBox_memdebug.AppendText("\r\n" + memregion);
        }

        private void button_clear_memdebug_Click(object sender, EventArgs e)
        {
            textBox_memdebug.Text = "";
        }

        public void generateMemregions()
        {
            String layout = txt_memlayout.Text;
            Regex regex = new Regex("\r\n");
            String[] lines = regex.Split(layout);

            memregions.Clear();
            comboBox_memregions.Items.Clear();

            foreach (String mem in lines)
            {

                String[] mem_parts = mem.Split(' ');
                if (mem_parts.Length == 6)
                {
                    if (
                        mem_parts[1] == "-" &&
                        mem_parts[3] == "," &&
                        mem_parts[4] == "size:"
                    ) {
                        Memregion memregion = new Memregion(mem);
                        memregions.Add(memregion);
                        int start = memregion.start;
                        int end = memregion.end;
                        int length = memregion.length;
                        comboBox_memregions.Items.Add(String.Format("{0:X} -> {1:X} [{2:X}]", start, end, length));
                    }
                }
            }

            comboBox_memregions.SelectedIndex = 0;
        }

        private void button_dummy_memregion2_Click(object sender, EventArgs e)
        {
            generateMemregions();
            foreach (Memregion mem in memregions)
            {
                // read all the new values.
                int start = mem.start;
                int end = mem.end;
                int length = mem.length;
 
                String memregion = String.Format("\r\n{0:X} - {1:X} , size: {2:X}", start, end, length);
                textBox_memdebug.AppendText(memregion);


            }
        }

        private void txt_memlayout_TextChanged(object sender, EventArgs e)
        {
            generateMemregions();
        }
    }
}
