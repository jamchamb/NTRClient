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
            int retry = 0;
            while (read_value == -1 && retry < 300000) // Decreases performance, but is now reliable
            {
                Task.Delay(25);
                retry++;
            }
            //MessageBox.Show(""+retry);
            if (read_value == -1)
                read_value = 0;
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

        public static int fromLE(String hex_le)
        {
            int temp = 0;
            if (hex_le.Length == 4)
            {
                temp = Convert.ToInt16(hex_le, 16);
                return fromLE(temp, 2);
            }
            else
            {
                temp = Convert.ToInt32(hex_le, 16);
                return fromLE(temp, 4);
            }
        }

        public static int fromLE(int temp, int len)
        {
            byte[] bytes = BitConverter.GetBytes(temp);

            Array.Reverse(bytes);
            if (len == 2)
                return BitConverter.ToInt16(bytes, 2);
            else
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
	    public String runCmd(String cmd) {
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

        // WARNING: This is a mess.. 

        // Generating Hex chunks

        String generateHexChunk(int value, int length)
        {
            String data = "(";
            byte[] bytes = BitConverter.GetBytes(value);
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                if (i < length - 1)
                {
                    data += String.Format("0x{0:X}, ", b);
                }
                else
                {
                    data += String.Format("0x{0:X}", b);
                    break;
                }
            }
            return data + ")";
        }

        String generateHexChunk(uint value, int length)
        {
            
            String data = "(";
            byte[] bytes = BitConverter.GetBytes(value);
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                if (i < length - 1)
                {
                    data += String.Format("0x{0:X}, ", b);
                }
                else
                {
                    data += String.Format("0x{0:X}", b);
                    break;
                }
            }
            return data + ")";
        }

        // gen Write Strings

        public String generateWriteString(int addr, int value, int length)
        {
            String data = generateHexChunk(value, length);

            return String.Format("write(0x{0:X}, {1}, pid=0x{2})", addr, data, textBox_pid.Text);
        }

        public String generateWriteString(uint addr, uint value, int length)
        {
            String data = generateHexChunk(value, length);
            return String.Format("write(0x{0:X}, {1}, pid=0x{2})", addr, data, textBox_pid.Text);
        }

        public String generateWriteString(int addr, uint value, int length)
        {
            String data = generateHexChunk(value, length);
            return String.Format("write(0x{0:X}, {1}, pid=0x{2})", addr, data, textBox_pid.Text);
        }

        // Other stuff

        private void button_dummy_read_Click(object sender, EventArgs e)
        {
            int addr = Convert.ToInt32(textBox_dummy_addr.Text, 16);
            int v = readValue(addr, (int)numericUpDown_dummy_length.Value);
            textBox_dummy_value_hex.Text = String.Format("{0:X}", v);
            textBox_dummy_value_dec.Text = String.Format("{0}", fromLE(v, (int)numericUpDown_dummy_length.Value));
        }

        private void button_dummy_write_Click(object sender, EventArgs e)
        {
            int addr = Convert.ToInt32(textBox_dummy_addr.Text, 16);
            int v = fromLE(Convert.ToInt32(textBox_dummy_value_hex.Text, 16), (int)numericUpDown_dummy_length.Value);
            runCmd(generateWriteString(addr, v, (int)numericUpDown_dummy_length.Value));
        }
        
        private void button_dummy_write_dec_Click(object sender, EventArgs e)
        {
            int addr = Convert.ToInt32(textBox_dummy_addr.Text, 16);
            int v = Convert.ToInt32(textBox_dummy_value_dec.Text, 10);
            runCmd(generateWriteString(addr, v, (int)numericUpDown_dummy_length.Value));
        }

        // Mario Kart 7 (US) [1.1] Codes

        // Coins 

        // 1413C540
        private void button_mk7_coins_read_Click(object sender, EventArgs e)
        {
            int addr = 0x1413C540;
            int v = fromLE(readValue(addr, 4), 4);
            textBox_mk7_coins.Text = String.Format("{0}", v);
        }

        private void button_mk7_coins_write_Click(object sender, EventArgs e)
        {
            int addr = 0x1413C540;
            int v = getInt(textBox_mk7_coins.Text);
            runCmd(generateWriteString(addr, v, 4));
        }

        // Animal Crossing (EU)

        private void button_aceu_openIds_Click(object sender, EventArgs e)
        {
            Browser.openURL("https://docs.google.com/spreadsheets/d/1NlfzvYM-dxsL3c6uP_089t8g5YNcT-AuUJvTE4COjmo/edit#gid=0");
        }

        private void button_aceu_setSlot1_Click(object sender, EventArgs e)
        {
            int addr = 0x15FBEDD0; 
            int id = fromLE(textBox_aceu_itemid.Text);
            if (id > 0xffff) // temporarily fixing an error in fromLE(string)
                id /= 0x10000;
            //MessageBox.Show(String.Format("{0:X}", id));
            runCmd(generateWriteString(addr, id, 4));

        }

        private void button_aceu_clear_slot1_Click(object sender, EventArgs e)
        {
            int addr = 0x15FBEDD0;
            int id = fromLE("FE7F");
            runCmd(generateWriteString(addr, id, 2));
        }

        private void button_aceu_clear_all_Click(object sender, EventArgs e)
        {
            clearInv();
        }

        // Gen items

        private void genItems(int value, int inv_size)
        {
            int addr = 0x15FBEDD0;
            //int value = fromLE("902E");
            //int inv_size = 16;

            for (int i = 0; i < inv_size; i++)
            {
                int addr_ = addr + i * 4;
                runCmd(generateWriteString(addr_, value + i, 4));
            }
        }

        private void clearInv()
        {
            int addr = 0x15FBEDD0;
            int clear_item = fromLE("FE7F");
            int inv_size = 16;

            for (int i = 0; i < inv_size; i++)
            {
                int addr_ = addr + i * 4;
                runCmd(generateWriteString(addr_, clear_item, 4));
            }
        }

        // Item packs

        private void button_aceu_fossil1_Click(object sender, EventArgs e)
        {
            int value = fromLE("902E");
            genItems(value, 16);

        }

        private void button_aceu_fossil2_Click(object sender, EventArgs e)
        {
            int value = fromLE("A02E");
            genItems(value, 16);
        }

        private void button_aceu_fossil3_Click(object sender, EventArgs e)
        {

            int value = fromLE("B02E");
            genItems(value, 16);
        }

        private void button_aceu_fossil4_Click(object sender, EventArgs e)
        {

            int value = fromLE("C02E");
            genItems(value, 16);
        }

        private void button_aceu_fossil5_Click(object sender, EventArgs e)
        {

            int value = fromLE("D02E");
            genItems(value, 16);
        }

        private void button_aceu_fossil6_Click(object sender, EventArgs e)
        {

            int value = fromLE("E02E");
            genItems(value, 7);
        }

        // Debug

        // Gateshark development
        
        private void button_gateshark_parse_Click(object sender, EventArgs e)
        {
            textBox_gateshark_parsed.Text = String.Empty;
            gateshark gs = new gateshark(textBox_gateshark.Text);
            foreach (gateshark_ar gs_ar in gs.getAllCodes())
            {
                Int32 cmd = gs_ar.getCMD();
                Int32 block_a = gs_ar.getBlock_A();
                UInt32 block_b = gs_ar.getBlock_B();
                String parsed = String.Format("{0:X} {1:X} {2:X}\r\n", cmd, block_a, block_b);
                textBox_gateshark_parsed.AppendText(parsed);

            }
        }

        private void button_gateshark_execute_Click(object sender, EventArgs e)
        {
            String code = textBox_gateshark.Text;
            gateshark gs = new gateshark(code);
            gs.execute();
        }

        private void button_browser_fort42_Click(object sender, EventArgs e)
        {
            Browser.openURL("http://www.fort42.com/gateshark");
        }

        private void moreCreditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Credits c = new Credits();
            c.Show();
            c.Focus();
        }

        private void githubRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.openURL("https://github.com/imthe666st/NTRClient");
        }

        private void toggleDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabPage_main_debug.Enabled = !tabPage_main_debug.Enabled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String wCmd = "";
            char[] name = textBox1.Text.ToCharArray();
            for (int i = 0; i < 11; i++)
            {
                char c = 'A';
                if (i < name.Length)
                {
                    wCmd += String.Format("ord('{0}'), 0", name[i]);
                }
                else
                {
                    wCmd += "0, 0";
                }

                if (i < 10)
                {
                    wCmd += ", ";
                }

            }

            String[] addr =
            {
                "0x0836984C", "0x083FF258", "0x089417E8", "0x08941915", "0x08948A50"
            };
            foreach (String a in addr)
            {
                runCmd(String.Format("write({0}, ({1}), pid=0x{2})", a, wCmd, textBox_pid.Text));
            }
        }
    }
}
