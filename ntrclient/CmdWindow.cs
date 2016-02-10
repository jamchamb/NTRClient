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

        public int read_value = -1;
        

        public void setReadValue(int r)
        {
            if (r == -1)
                r = 0;
            read_value = r;
        }

        public int readValue(int addr, int size)
        {
            int v;
            if (size < 1)
                size = 1;

            runCmd(String.Format("read(0x{0:X}, 0x{1:X}, pid=0x{2})", addr, size, textBox_pid.Text));
            while (read_value == -1)
            {
                Task.Delay(25);
            }

            // always int32
            v = read_value;
            read_value = -1;
            return v;
        }
        
        int getInt(String l)
        {
            return Convert.ToInt32(l, 10);
        }

        String toHex(int v)
        {
            return String.Format("{0:X}", v);
        }

        int fromLE(String hex_le)
        {
            int temp = 0;
            if (hex_le.Length == 4)
                temp = Convert.ToInt16(hex_le, 16);
            else
                temp = Convert.ToInt32(hex_le, 16);
            return fromLE(temp);
        }

        int fromLE(int temp)
        {
            byte[] bytes = BitConverter.GetBytes(temp);

            Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }

        // Actual code

        public CmdWindow()
        {
            delAddLog = new LogDelegate(Addlog);

            InitializeComponent();
        }

        public delegate void delegate_logAppend(String l);
        public void Addlog(string l) {
			if (!l.Contains("\r\n")) {
				l = l.Replace("\n", "\r\n");
			}
            if (!l.EndsWith("\n")) {
                l += "\r\n";
            }

            // Test for multithreading. This is horrible

            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new delegate_logAppend(txtLog.AppendText), l);
            } else
            {
                txtLog.AppendText(l);
            }
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
            //textBox_Ip.Text = "192.168.0.11";
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
        
        private void txt_memlayout_TextChanged(object sender, EventArgs e)
        {
            generateMemregions();
        }

        // Experimental realtime RAM view

        // This code doesn't work.
        // Feel free to fork this and get this to work.
        //
        // Reference:
        //      NtrClient.cs:89 -> Program.gCmdWindow.rt_ram();


        /*
        delegate void delegate_setRT_RAM(String l);
        void setRT_RAM(String l)
        {
            textBox_rt_ram.Text = l;
        }
        
        public void rt_ram()
        {
            // Realtime ram stuff
            if (checkBox_rt.Checked)
            {
                // Running
                int addr = Convert.ToInt32(textBox_rt_addr.Text, 16);
                int size = 4;
                int value = readValue(addr, size);
                String l = String.Format("{0:X}", value);
                textBox_rt_ram.Invoke(new delegate_setRT_RAM(setRT_RAM), l);
            }
        }

        */

        String generateHexChunk(int value, int length)
        {
            String data = "(";
            byte[] bytes = BitConverter.GetBytes(value);
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                if (i < length-1)
                {
                    data += String.Format("0x{0:X}, ", b);
                } else
                {
                    data += String.Format("0x{0:X}", b);
                    break;
                }
            }
            return data + ")";
        }

        String generateWriteString(int addr, int value, int length)
        {
            String data = generateHexChunk(value, length);

            return String.Format("write(0x{0:X}, {1}, pid=0x{2})", addr, data, textBox_pid.Text);
        }

        private void button_dummy_read_Click(object sender, EventArgs e)
        {
            int addr = Convert.ToInt32(textBox_dummy_addr.Text, 16);
            int v = fromLE(readValue(addr, (int)numericUpDown_dummy_length.Value));
            textBox_dummy_value.Text = String.Format("{0}", v);
        }

        private void button_dummy_write_Click(object sender, EventArgs e)
        {
            int addr = Convert.ToInt32(textBox_dummy_addr.Text, 16);
            int v = getInt(textBox_dummy_value.Text);
            runCmd(generateWriteString(addr, v, (int)numericUpDown_dummy_length.Value));
        }

        // Mario Kart 7 (US) [1.1] Codes

        // Coins 

        // 1413C540
        private void button_mk7_coins_read_Click(object sender, EventArgs e)
        {
            int addr = 0x1413C540;
            int v = fromLE(readValue(addr, 4));
            textBox_mk7_coins.Text = String.Format("{0}", v);
        }

        private void button_mk7_coins_write_Click(object sender, EventArgs e)
        {
            int addr = 0x1413C540;
            int v = getInt(textBox_mk7_coins.Text);
            runCmd(generateWriteString(addr, v, 4));
        }
    }
}
