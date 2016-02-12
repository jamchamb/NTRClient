namespace ntrclient
{
    partial class CmdWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asmScratchPadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button_dummy_write = new System.Windows.Forms.Button();
            this.textBox_dummy_value = new System.Windows.Forms.TextBox();
            this.numericUpDown_dummy_length = new System.Windows.Forms.NumericUpDown();
            this.textBox_dummy_addr = new System.Windows.Forms.TextBox();
            this.button_dummy_read = new System.Windows.Forms.Button();
            this.comboBox_memregions = new System.Windows.Forms.ComboBox();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.checkBox_debug = new System.Windows.Forms.CheckBox();
            this.txt_memlayout = new System.Windows.Forms.TextBox();
            this.textBox_dump_file = new System.Windows.Forms.TextBox();
            this.button_dump = new System.Windows.Forms.Button();
            this.button_hello = new System.Windows.Forms.Button();
            this.button_memlayout = new System.Windows.Forms.Button();
            this.button_processes = new System.Windows.Forms.Button();
            this.textBox_pid = new System.Windows.Forms.TextBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.textBox_Ip = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_mk7_coins_write = new System.Windows.Forms.Button();
            this.button_mk7_coins_read = new System.Windows.Forms.Button();
            this.textBox_mk7_coins = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button_aceu_fossil1 = new System.Windows.Forms.Button();
            this.button_aceu_setSlot1 = new System.Windows.Forms.Button();
            this.textBox_aceu_itemid = new System.Windows.Forms.TextBox();
            this.button_aceu_openIds = new System.Windows.Forms.Button();
            this.button_aceu_fossil2 = new System.Windows.Forms.Button();
            this.button_aceu_fossil3 = new System.Windows.Forms.Button();
            this.button_aceu_fossil4 = new System.Windows.Forms.Button();
            this.button_aceu_fossil5 = new System.Windows.Forms.Button();
            this.button_aceu_fossil6 = new System.Windows.Forms.Button();
            this.button_aceu_clear_slot1 = new System.Windows.Forms.Button();
            this.button_aceu_clear_all = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_dummy_length)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.txtLog, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtCmd, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(680, 430);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.MaxLength = 32767000;
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(674, 370);
            this.txtLog.TabIndex = 0;
            // 
            // txtCmd
            // 
            this.txtCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCmd.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCmd.Location = new System.Drawing.Point(3, 379);
            this.txtCmd.Name = "txtCmd";
            this.txtCmd.Size = new System.Drawing.Size(674, 26);
            this.txtCmd.TabIndex = 1;
            this.txtCmd.TextChanged += new System.EventHandler(this.txtCmd_TextChanged);
            this.txtCmd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCmd_KeyDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 408);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(680, 22);
            this.statusStrip1.TabIndex = 2;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(680, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CommandToolStripMenuItem,
            this.asmScratchPadToolStripMenuItem});
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.ToolStripMenuItem.Text = "Tools";
            this.ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // CommandToolStripMenuItem
            // 
            this.CommandToolStripMenuItem.Name = "CommandToolStripMenuItem";
            this.CommandToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.CommandToolStripMenuItem.Text = "Hotkey Commands";
            this.CommandToolStripMenuItem.Click += new System.EventHandler(this.CommandToolStripMenuItem_Click);
            // 
            // asmScratchPadToolStripMenuItem
            // 
            this.asmScratchPadToolStripMenuItem.Name = "asmScratchPadToolStripMenuItem";
            this.asmScratchPadToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.asmScratchPadToolStripMenuItem.Text = "Asm ScratchPad";
            this.asmScratchPadToolStripMenuItem.Click += new System.EventHandler(this.asmScratchPadToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 454);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(680, 250);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button_dummy_write);
            this.tabPage1.Controls.Add(this.textBox_dummy_value);
            this.tabPage1.Controls.Add(this.numericUpDown_dummy_length);
            this.tabPage1.Controls.Add(this.textBox_dummy_addr);
            this.tabPage1.Controls.Add(this.button_dummy_read);
            this.tabPage1.Controls.Add(this.comboBox_memregions);
            this.tabPage1.Controls.Add(this.button_disconnect);
            this.tabPage1.Controls.Add(this.checkBox_debug);
            this.tabPage1.Controls.Add(this.txt_memlayout);
            this.tabPage1.Controls.Add(this.textBox_dump_file);
            this.tabPage1.Controls.Add(this.button_dump);
            this.tabPage1.Controls.Add(this.button_hello);
            this.tabPage1.Controls.Add(this.button_memlayout);
            this.tabPage1.Controls.Add(this.button_processes);
            this.tabPage1.Controls.Add(this.textBox_pid);
            this.tabPage1.Controls.Add(this.button_Connect);
            this.tabPage1.Controls.Add(this.textBox_Ip);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(672, 224);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button_dummy_write
            // 
            this.button_dummy_write.Location = new System.Drawing.Point(476, 109);
            this.button_dummy_write.Name = "button_dummy_write";
            this.button_dummy_write.Size = new System.Drawing.Size(150, 20);
            this.button_dummy_write.TabIndex = 18;
            this.button_dummy_write.Text = "Write to Addr";
            this.button_dummy_write.UseVisualStyleBackColor = true;
            this.button_dummy_write.Click += new System.EventHandler(this.button_dummy_write_Click);
            // 
            // textBox_dummy_value
            // 
            this.textBox_dummy_value.Location = new System.Drawing.Point(320, 110);
            this.textBox_dummy_value.Name = "textBox_dummy_value";
            this.textBox_dummy_value.Size = new System.Drawing.Size(150, 20);
            this.textBox_dummy_value.TabIndex = 17;
            this.textBox_dummy_value.Text = "Value";
            // 
            // numericUpDown_dummy_length
            // 
            this.numericUpDown_dummy_length.Location = new System.Drawing.Point(632, 83);
            this.numericUpDown_dummy_length.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDown_dummy_length.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_dummy_length.Name = "numericUpDown_dummy_length";
            this.numericUpDown_dummy_length.Size = new System.Drawing.Size(32, 20);
            this.numericUpDown_dummy_length.TabIndex = 16;
            this.numericUpDown_dummy_length.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textBox_dummy_addr
            // 
            this.textBox_dummy_addr.Location = new System.Drawing.Point(320, 84);
            this.textBox_dummy_addr.Name = "textBox_dummy_addr";
            this.textBox_dummy_addr.Size = new System.Drawing.Size(150, 20);
            this.textBox_dummy_addr.TabIndex = 15;
            this.textBox_dummy_addr.Text = "Addr";
            // 
            // button_dummy_read
            // 
            this.button_dummy_read.Location = new System.Drawing.Point(476, 83);
            this.button_dummy_read.Name = "button_dummy_read";
            this.button_dummy_read.Size = new System.Drawing.Size(150, 20);
            this.button_dummy_read.TabIndex = 14;
            this.button_dummy_read.Text = "Read at Addr";
            this.button_dummy_read.UseVisualStyleBackColor = true;
            this.button_dummy_read.Click += new System.EventHandler(this.button_dummy_read_Click);
            // 
            // comboBox_memregions
            // 
            this.comboBox_memregions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_memregions.FormattingEnabled = true;
            this.comboBox_memregions.Location = new System.Drawing.Point(8, 58);
            this.comboBox_memregions.Name = "comboBox_memregions";
            this.comboBox_memregions.Size = new System.Drawing.Size(306, 21);
            this.comboBox_memregions.TabIndex = 13;
            // 
            // button_disconnect
            // 
            this.button_disconnect.Location = new System.Drawing.Point(476, 6);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(150, 20);
            this.button_disconnect.TabIndex = 12;
            this.button_disconnect.Text = "Disconnect";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.button_disconnect_Click);
            // 
            // checkBox_debug
            // 
            this.checkBox_debug.AutoSize = true;
            this.checkBox_debug.Location = new System.Drawing.Point(476, 31);
            this.checkBox_debug.Name = "checkBox_debug";
            this.checkBox_debug.Size = new System.Drawing.Size(91, 17);
            this.checkBox_debug.TabIndex = 11;
            this.checkBox_debug.Text = "Debug output";
            this.checkBox_debug.UseVisualStyleBackColor = true;
            // 
            // txt_memlayout
            // 
            this.txt_memlayout.Location = new System.Drawing.Point(8, 83);
            this.txt_memlayout.Multiline = true;
            this.txt_memlayout.Name = "txt_memlayout";
            this.txt_memlayout.ReadOnly = true;
            this.txt_memlayout.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_memlayout.Size = new System.Drawing.Size(306, 133);
            this.txt_memlayout.TabIndex = 10;
            this.txt_memlayout.TextChanged += new System.EventHandler(this.txt_memlayout_TextChanged);
            // 
            // textBox_dump_file
            // 
            this.textBox_dump_file.Location = new System.Drawing.Point(320, 57);
            this.textBox_dump_file.Name = "textBox_dump_file";
            this.textBox_dump_file.Size = new System.Drawing.Size(150, 20);
            this.textBox_dump_file.TabIndex = 9;
            this.textBox_dump_file.Text = "filename";
            // 
            // button_dump
            // 
            this.button_dump.Location = new System.Drawing.Point(476, 56);
            this.button_dump.Name = "button_dump";
            this.button_dump.Size = new System.Drawing.Size(150, 20);
            this.button_dump.TabIndex = 8;
            this.button_dump.Text = "Dump memory";
            this.button_dump.UseVisualStyleBackColor = true;
            this.button_dump.Click += new System.EventHandler(this.button_dump_Click);
            // 
            // button_hello
            // 
            this.button_hello.Location = new System.Drawing.Point(320, 6);
            this.button_hello.Name = "button_hello";
            this.button_hello.Size = new System.Drawing.Size(150, 20);
            this.button_hello.TabIndex = 5;
            this.button_hello.Text = "Hello! - Test connection";
            this.button_hello.UseVisualStyleBackColor = true;
            this.button_hello.Click += new System.EventHandler(this.button_hello_Click);
            // 
            // button_memlayout
            // 
            this.button_memlayout.Location = new System.Drawing.Point(320, 31);
            this.button_memlayout.Name = "button_memlayout";
            this.button_memlayout.Size = new System.Drawing.Size(150, 20);
            this.button_memlayout.TabIndex = 4;
            this.button_memlayout.Text = "Memlayout";
            this.button_memlayout.UseVisualStyleBackColor = true;
            this.button_memlayout.Click += new System.EventHandler(this.button_memlayout_Click);
            // 
            // button_processes
            // 
            this.button_processes.Location = new System.Drawing.Point(164, 31);
            this.button_processes.Name = "button_processes";
            this.button_processes.Size = new System.Drawing.Size(150, 20);
            this.button_processes.TabIndex = 3;
            this.button_processes.Text = "List processes";
            this.button_processes.UseVisualStyleBackColor = true;
            this.button_processes.Click += new System.EventHandler(this.button_processes_Click);
            // 
            // textBox_pid
            // 
            this.textBox_pid.Location = new System.Drawing.Point(8, 32);
            this.textBox_pid.Name = "textBox_pid";
            this.textBox_pid.Size = new System.Drawing.Size(150, 20);
            this.textBox_pid.TabIndex = 2;
            this.textBox_pid.Text = "Process PID";
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(164, 6);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(150, 20);
            this.button_Connect.TabIndex = 1;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // textBox_Ip
            // 
            this.textBox_Ip.Location = new System.Drawing.Point(8, 6);
            this.textBox_Ip.Name = "textBox_Ip";
            this.textBox_Ip.Size = new System.Drawing.Size(150, 20);
            this.textBox_Ip.TabIndex = 0;
            this.textBox_Ip.Text = "Nintendo 3DS IP";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button_mk7_coins_write);
            this.tabPage2.Controls.Add(this.button_mk7_coins_read);
            this.tabPage2.Controls.Add(this.textBox_mk7_coins);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(672, 224);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Mario Kart 7 (US)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_mk7_coins_write
            // 
            this.button_mk7_coins_write.Location = new System.Drawing.Point(320, 6);
            this.button_mk7_coins_write.Name = "button_mk7_coins_write";
            this.button_mk7_coins_write.Size = new System.Drawing.Size(150, 20);
            this.button_mk7_coins_write.TabIndex = 5;
            this.button_mk7_coins_write.Text = "Write";
            this.button_mk7_coins_write.UseVisualStyleBackColor = true;
            this.button_mk7_coins_write.Click += new System.EventHandler(this.button_mk7_coins_write_Click);
            // 
            // button_mk7_coins_read
            // 
            this.button_mk7_coins_read.Location = new System.Drawing.Point(164, 6);
            this.button_mk7_coins_read.Name = "button_mk7_coins_read";
            this.button_mk7_coins_read.Size = new System.Drawing.Size(150, 20);
            this.button_mk7_coins_read.TabIndex = 4;
            this.button_mk7_coins_read.Text = "Read";
            this.button_mk7_coins_read.UseVisualStyleBackColor = true;
            this.button_mk7_coins_read.Click += new System.EventHandler(this.button_mk7_coins_read_Click);
            // 
            // textBox_mk7_coins
            // 
            this.textBox_mk7_coins.Location = new System.Drawing.Point(8, 6);
            this.textBox_mk7_coins.Name = "textBox_mk7_coins";
            this.textBox_mk7_coins.Size = new System.Drawing.Size(150, 20);
            this.textBox_mk7_coins.TabIndex = 3;
            this.textBox_mk7_coins.Text = "Coins";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button_aceu_clear_all);
            this.tabPage3.Controls.Add(this.button_aceu_clear_slot1);
            this.tabPage3.Controls.Add(this.button_aceu_fossil6);
            this.tabPage3.Controls.Add(this.button_aceu_fossil5);
            this.tabPage3.Controls.Add(this.button_aceu_fossil4);
            this.tabPage3.Controls.Add(this.button_aceu_fossil3);
            this.tabPage3.Controls.Add(this.button_aceu_fossil2);
            this.tabPage3.Controls.Add(this.button_aceu_fossil1);
            this.tabPage3.Controls.Add(this.button_aceu_setSlot1);
            this.tabPage3.Controls.Add(this.textBox_aceu_itemid);
            this.tabPage3.Controls.Add(this.button_aceu_openIds);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(672, 224);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Animal Crossing (EU)";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button_aceu_fossil1
            // 
            this.button_aceu_fossil1.Location = new System.Drawing.Point(8, 32);
            this.button_aceu_fossil1.Name = "button_aceu_fossil1";
            this.button_aceu_fossil1.Size = new System.Drawing.Size(150, 20);
            this.button_aceu_fossil1.TabIndex = 5;
            this.button_aceu_fossil1.Text = "Fossil pack 1";
            this.button_aceu_fossil1.UseVisualStyleBackColor = true;
            this.button_aceu_fossil1.Click += new System.EventHandler(this.button_aceu_fossil1_Click);
            // 
            // button_aceu_setSlot1
            // 
            this.button_aceu_setSlot1.Location = new System.Drawing.Point(162, 5);
            this.button_aceu_setSlot1.Name = "button_aceu_setSlot1";
            this.button_aceu_setSlot1.Size = new System.Drawing.Size(150, 20);
            this.button_aceu_setSlot1.TabIndex = 4;
            this.button_aceu_setSlot1.Text = "Overwrite Slot 1";
            this.button_aceu_setSlot1.UseVisualStyleBackColor = true;
            this.button_aceu_setSlot1.Click += new System.EventHandler(this.button_aceu_setSlot1_Click);
            // 
            // textBox_aceu_itemid
            // 
            this.textBox_aceu_itemid.Location = new System.Drawing.Point(6, 6);
            this.textBox_aceu_itemid.Name = "textBox_aceu_itemid";
            this.textBox_aceu_itemid.Size = new System.Drawing.Size(150, 20);
            this.textBox_aceu_itemid.TabIndex = 3;
            this.textBox_aceu_itemid.Text = "Item ID (HEX)";
            // 
            // button_aceu_openIds
            // 
            this.button_aceu_openIds.Location = new System.Drawing.Point(514, 6);
            this.button_aceu_openIds.Name = "button_aceu_openIds";
            this.button_aceu_openIds.Size = new System.Drawing.Size(150, 20);
            this.button_aceu_openIds.TabIndex = 2;
            this.button_aceu_openIds.Text = "Item IDs";
            this.button_aceu_openIds.UseVisualStyleBackColor = true;
            this.button_aceu_openIds.Click += new System.EventHandler(this.button_aceu_openIds_Click);
            // 
            // button_aceu_fossil2
            // 
            this.button_aceu_fossil2.Location = new System.Drawing.Point(8, 58);
            this.button_aceu_fossil2.Name = "button_aceu_fossil2";
            this.button_aceu_fossil2.Size = new System.Drawing.Size(150, 20);
            this.button_aceu_fossil2.TabIndex = 6;
            this.button_aceu_fossil2.Text = "Fossil pack 2";
            this.button_aceu_fossil2.UseVisualStyleBackColor = true;
            this.button_aceu_fossil2.Click += new System.EventHandler(this.button_aceu_fossil2_Click);
            // 
            // button_aceu_fossil3
            // 
            this.button_aceu_fossil3.Location = new System.Drawing.Point(6, 84);
            this.button_aceu_fossil3.Name = "button_aceu_fossil3";
            this.button_aceu_fossil3.Size = new System.Drawing.Size(150, 20);
            this.button_aceu_fossil3.TabIndex = 7;
            this.button_aceu_fossil3.Text = "Fossil pack 3";
            this.button_aceu_fossil3.UseVisualStyleBackColor = true;
            this.button_aceu_fossil3.Click += new System.EventHandler(this.button_aceu_fossil3_Click);
            // 
            // button_aceu_fossil4
            // 
            this.button_aceu_fossil4.Location = new System.Drawing.Point(8, 110);
            this.button_aceu_fossil4.Name = "button_aceu_fossil4";
            this.button_aceu_fossil4.Size = new System.Drawing.Size(150, 20);
            this.button_aceu_fossil4.TabIndex = 8;
            this.button_aceu_fossil4.Text = "Fossil pack 4";
            this.button_aceu_fossil4.UseVisualStyleBackColor = true;
            this.button_aceu_fossil4.Click += new System.EventHandler(this.button_aceu_fossil4_Click);
            // 
            // button_aceu_fossil5
            // 
            this.button_aceu_fossil5.Location = new System.Drawing.Point(8, 136);
            this.button_aceu_fossil5.Name = "button_aceu_fossil5";
            this.button_aceu_fossil5.Size = new System.Drawing.Size(150, 20);
            this.button_aceu_fossil5.TabIndex = 9;
            this.button_aceu_fossil5.Text = "Fossil pack 5";
            this.button_aceu_fossil5.UseVisualStyleBackColor = true;
            this.button_aceu_fossil5.Click += new System.EventHandler(this.button_aceu_fossil5_Click);
            // 
            // button_aceu_fossil6
            // 
            this.button_aceu_fossil6.Location = new System.Drawing.Point(8, 162);
            this.button_aceu_fossil6.Name = "button_aceu_fossil6";
            this.button_aceu_fossil6.Size = new System.Drawing.Size(150, 20);
            this.button_aceu_fossil6.TabIndex = 10;
            this.button_aceu_fossil6.Text = "Fossil pack 6";
            this.button_aceu_fossil6.UseVisualStyleBackColor = true;
            this.button_aceu_fossil6.Click += new System.EventHandler(this.button_aceu_fossil6_Click);
            // 
            // button_aceu_clear_slot1
            // 
            this.button_aceu_clear_slot1.Location = new System.Drawing.Point(318, 5);
            this.button_aceu_clear_slot1.Name = "button_aceu_clear_slot1";
            this.button_aceu_clear_slot1.Size = new System.Drawing.Size(150, 20);
            this.button_aceu_clear_slot1.TabIndex = 11;
            this.button_aceu_clear_slot1.Text = "Clear Slot 1";
            this.button_aceu_clear_slot1.UseVisualStyleBackColor = true;
            this.button_aceu_clear_slot1.Click += new System.EventHandler(this.button_aceu_clear_slot1_Click);
            // 
            // button_aceu_clear_all
            // 
            this.button_aceu_clear_all.Location = new System.Drawing.Point(8, 188);
            this.button_aceu_clear_all.Name = "button_aceu_clear_all";
            this.button_aceu_clear_all.Size = new System.Drawing.Size(150, 20);
            this.button_aceu_clear_all.TabIndex = 12;
            this.button_aceu_clear_all.Text = "Clear Inventory";
            this.button_aceu_clear_all.UseVisualStyleBackColor = true;
            this.button_aceu_clear_all.Click += new System.EventHandler(this.button_aceu_clear_all_Click);
            // 
            // CmdWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 704);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CmdWindow";
            this.Text = "NTR Debugger";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CmdWindow_FormClosed);
            this.Load += new System.EventHandler(this.CmdWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CmdWindow_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.CmdWindow_PreviewKeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_dummy_length)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TextBox txtCmd;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CommandToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem asmScratchPadToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button_processes;
        private System.Windows.Forms.TextBox textBox_pid;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.TextBox textBox_Ip;
        private System.Windows.Forms.Button button_memlayout;
        private System.Windows.Forms.Button button_hello;
        private System.Windows.Forms.TextBox textBox_dump_file;
        private System.Windows.Forms.Button button_dump;
        public System.Windows.Forms.CheckBox checkBox_debug;
        public System.Windows.Forms.TextBox txt_memlayout;
        private System.Windows.Forms.Button button_disconnect;
        public System.Windows.Forms.ComboBox comboBox_memregions;
        private System.Windows.Forms.Button button_mk7_coins_write;
        private System.Windows.Forms.Button button_mk7_coins_read;
        private System.Windows.Forms.TextBox textBox_mk7_coins;
        private System.Windows.Forms.TextBox textBox_dummy_addr;
        private System.Windows.Forms.Button button_dummy_read;
        private System.Windows.Forms.Button button_dummy_write;
        private System.Windows.Forms.TextBox textBox_dummy_value;
        private System.Windows.Forms.NumericUpDown numericUpDown_dummy_length;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button_aceu_openIds;
        private System.Windows.Forms.Button button_aceu_setSlot1;
        private System.Windows.Forms.TextBox textBox_aceu_itemid;
        private System.Windows.Forms.Button button_aceu_fossil1;
        private System.Windows.Forms.Button button_aceu_fossil5;
        private System.Windows.Forms.Button button_aceu_fossil4;
        private System.Windows.Forms.Button button_aceu_fossil3;
        private System.Windows.Forms.Button button_aceu_fossil2;
        private System.Windows.Forms.Button button_aceu_fossil6;
        private System.Windows.Forms.Button button_aceu_clear_slot1;
        private System.Windows.Forms.Button button_aceu_clear_all;
    }
}

