namespace ntrclient.Prog.Window
{
    partial class DebugConsole
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBox_log = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBox_cmd = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_log
            // 
            this.textBox_log.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox_log.Location = new System.Drawing.Point(0, 0);
            this.textBox_log.Multiline = true;
            this.textBox_log.Name = "textBox_log";
            this.textBox_log.Size = new System.Drawing.Size(337, 229);
            this.textBox_log.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // textBox_cmd
            // 
            this.textBox_cmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_cmd.Location = new System.Drawing.Point(0, 0);
            this.textBox_cmd.Name = "textBox_cmd";
            this.textBox_cmd.Size = new System.Drawing.Size(288, 20);
            this.textBox_cmd.TabIndex = 0;
            this.textBox_cmd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_cmd_KeyDown);
            // 
            // button_send
            // 
            this.button_send.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_send.Location = new System.Drawing.Point(288, 0);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(49, 24);
            this.button_send.TabIndex = 3;
            this.button_send.Text = "SEND";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox_cmd);
            this.panel1.Controls.Add(this.button_send);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 235);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(337, 24);
            this.panel1.TabIndex = 4;
            // 
            // DebugConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 259);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox_log);
            this.Name = "DebugConsole";
            this.Text = "Debug Console";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugConsole_FormClosing);
            this.Shown += new System.EventHandler(this.DebugConsole_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBox_log;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox textBox_cmd;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Panel panel1;
    }
}