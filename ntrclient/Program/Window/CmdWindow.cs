using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ntrclient
{
    public partial class CmdWindow : Form

    {

        // Refactoring code

        //________________________________________________________________
        // System

        void updateProgress()
        {
            string text = "";
            if (Program.ntrClient.progress != -1)
            {
                text = String.Format("{0}%", Program.ntrClient.progress);
            }
            toolStripStatusLabel1.Text = text;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                updateProgress();
                Program.ntrClient.sendHeartbeatPacket();

                if (hbc != null)
                {
                    if (hbc.status())
                        label_heart_status.Text = "Heartbeat status: Running";
                    else
                        label_heart_status.Text = "Heartbeat status: OFFLINE";
                } else
                    label_heart_status.Text = "Heartbeat status: OFFLINE";
            }
            catch (Exception)
            {
            }
        }

        private void CmdWindow_Load(object sender, EventArgs e)
        {
            resetLog();

            textBox_Ip.Text = Program.sm.ip_address;
            if (Program.sm.ip_address != "Nintendo 3DS IP")
            {
                Addlog("Loaded your last IP address!");
            }

            // Start Heartbeat
            hbc = new Heartbeat_controller();
            hbc.start();

            // Start Octo
            Octo.init();
        }

        private void CmdWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.saveConfig();
            Program.ntrClient.disconnect();
            if (hbc != null)
            {
                hbc.stop();
            }
        }


        // END of System

        // Logging
        public delegate void LogDelegate(string l);
        public LogDelegate delAddLog;

        public CmdWindow()
        {
            delAddLog = new LogDelegate(Addlog);

            InitializeComponent();
        }

        public delegate void delegate_logAppend(String l);
        public void Addlog(string l)
        {
            if (!l.Contains("\r\n"))
            {
                l = l.Replace("\n", "\r\n");
            }
            if (!l.EndsWith("\n"))
            {
                l += "\r\n";
            }

            // Test for multithreading. This is horrible

            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new delegate_logAppend(txtLog.AppendText), l);
            }
            else
            {
                txtLog.AppendText(l);
            }
        }

        // END of Logging

        //________________________________________________________________

        // Memregions

        public List<Memregion> memregions = new List<Memregion>();

        public bool isMemValid(int v)
        {
            Memregion[] mems = memregions.ToArray();
            foreach (Memregion m in mems)
            {
                if (m.contains(v))
                    return true;
            }
            return false;
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
                    )
                    {
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

        // END of Memregions

        //________________________________________________________________

        // Handle Commands

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
            while (read_value == -1 && retry < 300000)
            {
                Task.Delay(25);
                retry++;
            }
            if (retry > 300000)
                Addlog("[READ ERROR] COULDN'T READ FAST ENOUGH!");
            if (read_value == -1)
                read_value = 0;
            v = read_value;
            read_value = -1;
            return v;
        }

        private void txtCmd_TextChanged(object sender, EventArgs e)
        {

        }

        public String runCmd(String cmd)
        {
            try
            {
                Addlog("> " + cmd);
                object ret = Program.pyEngine.CreateScriptSourceFromString(cmd).Execute(Program.globalScope);
                if (ret != null)
                {
                    Addlog(ret.ToString());
                    return ret.ToString();
                }
                else
                {
                    Addlog("null");
                    return "";
                }
            }
            catch (Exception ex)
            {
                Addlog(ex.Message);
                Addlog(ex.StackTrace);
                return "";
            }
        }

        private void txtCmd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string cmd = txtCmd.Text;
                txtCmd.Clear();
                runCmd(cmd);

            }
        }

        // END of Handle Commands

        //________________________________________________________________

        // Utilities

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
            else if (hex_le.Length == 2)
            {
                temp = Convert.ToInt32(hex_le, 16);
                return temp;
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
            short le = 0;
            int ret = 0;

            Array.Reverse(bytes);
            if (len == 2)
            {
                le = BitConverter.ToInt16(bytes, 2);
                if (le >= 0) return le;
                ret = le + 0x10000;
                //MessageBox.Show(String.Format("LE: {0:X}", ret));
                return ret;

            }
            if (len == 1)
                return bytes[bytes.Length - 1];
            
            return BitConverter.ToInt32(bytes, 0);
        }

        public void resetLog()
        {
            txtLog.Text = "";

            Addlog("NTR debugger by cell9 - Mod by imthe666st");
            runCmd("import sys;sys.path.append('.\\python\\Lib')");
            runCmd("for n in [n for n in dir(nc) if not n.startswith('_')]: globals()[n] = getattr(nc,n)    ");
            Addlog("Commands available: ");
            runCmd("repr([n for n in dir(nc) if not n.startswith('_')])");
        }

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

        // END of Utilities

        //________________________________________________________________

        // Tool Strip

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void CommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new QuickCmdWindow()).Show();
        }

        private void CmdWindow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {


        }

        private void CmdWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                int t = e.KeyValue;
                if (t >= 48 && t <= 57)
                {
                    runCmd(Program.sm.quickCmds[t - 48]);
                    e.SuppressKeyPress = true;

                }
            }

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void asmScratchPadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AsmEditWindow()).Show();
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

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetLog();
        }

        private void clearHeartbeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hbc != null)
            {
                hbc.setCode("");
                Addlog("Cleared Heartbeat command");
            } else
            {
                Addlog("Unable to clear Heartbear command");
            }
        }

        private void redditThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.openURL("https://www.reddit.com/r/3dshacks/comments/45iz4o/ntr_improved_ntr_debugger_client_with_gateshark/");
        }

        private void gbaTempThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser.openURL("http://gbatemp.net/threads/modified-ntr-client-with-gateshark-support.418294/");
        }

        // END of ToolStrip

        //________________________________________________________________

        // BASIC Tabpage

        private void button_Connect_Click(object sender, EventArgs e)
        {
            //textBox_Ip.Text = "192.168.0.11";
            runCmd("connect('" + textBox_Ip.Text + "', 8000)");
            Program.sm.ip_address = textBox_Ip.Text;
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

        private void txt_memlayout_TextChanged(object sender, EventArgs e)
        {
            generateMemregions();
        }

        private void button_dummy_read_Click(object sender, EventArgs e)
        {
            int addr = Convert.ToInt32(textBox_dummy_addr.Text, 16);
            int v = readValue(addr, (int)numericUpDown_dummy_length.Value);
            textBox_dummy_value_hex.Text = String.Format("{0:X}", v);
            textBox_dummy_value_hex_le.Text = String.Format("{0:X}", fromLE(v, (int)numericUpDown_dummy_length.Value));
            textBox_dummy_value_dec.Text = String.Format("{0}", fromLE(v, (int)numericUpDown_dummy_length.Value));
        }

        private void button_dummy_write_Click(object sender, EventArgs e)
        {
            int addr = Convert.ToInt32(textBox_dummy_addr.Text, 16);
            int v = fromLE(Convert.ToInt32(textBox_dummy_value_hex.Text, 16), (int)numericUpDown_dummy_length.Value);
            runCmd(generateWriteString(addr, v, (int)numericUpDown_dummy_length.Value));
        }

        private void button_dummy_write_hex_le_Click(object sender, EventArgs e)
        {
            int addr = Convert.ToInt32(textBox_dummy_addr.Text, 16);
            int v = Convert.ToInt32(textBox_dummy_value_hex_le.Text, 16);
            runCmd(generateWriteString(addr, v, (int)numericUpDown_dummy_length.Value));
        }

        private void button_dummy_write_dec_Click(object sender, EventArgs e)
        {
            int addr = Convert.ToInt32(textBox_dummy_addr.Text, 16);
            int v = Convert.ToInt32(textBox_dummy_value_dec.Text, 10);
            runCmd(generateWriteString(addr, v, (int)numericUpDown_dummy_length.Value));
        }

        // END of Basic
        
        //________________________________________________________________

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
        private void button_gateshark_heartbeat_Click(object sender, EventArgs e)
        {
            if (hbc != null)
                hbc.setCode(textBox_gateshark.Text);
        }

        private void button_browser_fort42_Click(object sender, EventArgs e)
        {
            Browser.openURL("http://www.fort42.com/gateshark");
        }

        //________________________________________________________________

        // Debug Tab

        private Heartbeat_controller hbc;
        private void button_heart_test_start_Click(object sender, EventArgs e)
        {
            hbc = new Heartbeat_controller();
            hbc.start();
        }

        private void button_heart_test_stop_Click(object sender, EventArgs e)
        {
            if (hbc != null)
                hbc.stop();
        }

        private void button_heart_test_inject_Click(object sender, EventArgs e)
        {
            if (hbc != null)
                hbc.setCode(
                "D3000000 00000470\r\n" +
                "481C7CD0 08800000\r\n" +
                "381C7CD0 08D00000\r\n" +
                "B81C7CD0 00000000\r\n" +
                "00001184 32003200\r\n" +
                "10001188 00003200\r\n" +
                "0000118C 44610000\r\n" +
                "00001190 44610000\r\n" +
                "10001194 00003200\r\n" +
                "D2000000 00000000\r\n" +

                "D3000000 081C8170\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 00000E28\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 000003E8\r\n" +
                "10000000 00000001\r\n" +
                "D2000000 00000000\r\n" +

                "D3000000 081C8174\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 00000E28\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 000003E8\r\n" +
                "10000000 00000001\r\n" +
                "D2000000 00000000"
            );
        }
        
        private void button_heart_test_toggle_Click(object sender, EventArgs e)
        {
            if (hbc != null)
            {
                hbc.toggle();
            }
        }

        private void button_cfg_set_dummy_Click(object sender, EventArgs e)
        {
            Program.sm.ip_address = textBox_cfg_set_dummy.Text;
        }

        private void button_cfg_read_dummy_Click(object sender, EventArgs e)
        {
            textBox_cfg_read_dummy.Text = Program.sm.ip_address;
        }

        // Little Endian test | Issue #11

        private void button_debug_conv_hex_Click(object sender, EventArgs e)
        {
            int v = Convert.ToInt32(textBox_debug_conv_hex.Text, 16);

            textBox_debug_conv_hex_le.Text = String.Format("{0:X}", fromLE(v, (int)numericUpDown_debug_hextest.Value));
            textBox_debug_conv_dec.Text = String.Format("{0}", v);
        }

        private void button_debug_conv_hex_le_Click(object sender, EventArgs e)
        {
            int v = fromLE(Convert.ToInt32(textBox_debug_conv_hex_le.Text, 16), (int)numericUpDown_debug_hextest.Value);

            textBox_debug_conv_hex.Text = String.Format("{0:X}", v);
            textBox_debug_conv_dec.Text = String.Format("{0}", v);
        }

        private void button_debug_conv_dec_Click(object sender, EventArgs e)
        {
            int v = Convert.ToInt32(textBox_debug_conv_dec.Text, 10);

            textBox_debug_conv_hex.Text = String.Format("{0:X}", v);
            textBox_debug_conv_hex_le.Text = String.Format("{0:X}", fromLE(v, (int)numericUpDown_debug_hextest.Value));

        }

        //________________________________________________________________

        // Starting with the Cheats section! 

        // Mario Kart 7 (US) [1.1] Codes
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

        //________________________________________________________________

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

        //________________________________________________________________

        // Monster Hunter 4 Ultimate (EU) 

        private void button_mh4u_eu_name_Click(object sender, EventArgs e)
        {
            String wCmd = "";
            char[] name = textBox_mh4u_eu_name.Text.ToCharArray();
            for (int i = 0; i < 11; i++)
            {
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
                //"0x082839D0", "0x08284AA6"
                "0x0836984C", "0x083EF258",
                "0x083FF258", "0x089417E8",
                "0x08941915", "0x08948A50"

            };
            foreach (String a in addr)
            {
                runCmd(String.Format("write({0}, ({1}), pid=0x{2})", a, wCmd, textBox_pid.Text));
            }
        }

        // Using my Gateshark system.. It's already in here, so why not use it?
        private void button_mh4u_eu_mon1_kill_Click(object sender, EventArgs e)
        {
            String gs_code =
                "D3000000 081C8170\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 00000E28\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 000003E8\r\n" +
                "10000000 00000001\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000";

            gateshark code = new gateshark(gs_code);
            code.execute();
        }

        private void button_mh4u_eu_mon2_kill_Click(object sender, EventArgs e)
        {
            String gs_code =
                "D3000000 081C8174\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 00000E28\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 000003E8\r\n" +
                "10000000 00000001\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000";

            gateshark code = new gateshark(gs_code);
            code.execute();
        }

        private void button_mh4u_eu_monb_kill_Click(object sender, EventArgs e)
        {
            String gs_code =
                "D3000000 081C8170\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 00000E28\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 000003E8\r\n" +
                "10000000 00000001\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000\r\n" +

                "D3000000 081C8174\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 00000E28\r\n" +
                "40000000 08000000\r\n" +
                "30000000 0B13EFFF\r\n" +
                "B0000000 00000000\r\n" +
                "DC000000 000003E8\r\n" +
                "10000000 00000001\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000\r\n" +
                "D0000000 00000000";

            gateshark code = new gateshark(gs_code);
            code.execute();
        }

        private void button_mh4u_eu_hb_godmode_Click(object sender, EventArgs e)
        {
            if (hbc != null)
                hbc.setCode(
                    "D3000000 00000470\r\n" +
                    "481C7CD0 08800000\r\n" +
                    "381C7CD0 08D00000\r\n" +
                    "B81C7CD0 00000000\r\n" +
                    "00001184 32003200\r\n" +
                    "10001188 00003200\r\n" +
                    "0000118C 44610000\r\n" +
                    "00001190 44610000\r\n" +
                    "10001194 00003200\r\n" +
                    "D2000000 00000000"
                );
        }

        private async void button_update_Click(object sender, EventArgs e)
        {
            if (await Octo.isUpdate()) MessageBox.Show("A new version has been released!");
            else MessageBox.Show("No new release found!");
        }
    }
}
