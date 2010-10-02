namespace DarkLiteUO

{
    partial class Form1
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
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtCharSlot = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.dissconnect = new System.Windows.Forms.Button();
            this.vartree = new System.Windows.Forms.TreeView();
            this.button3 = new System.Windows.Forms.Button();
            this.btn_clearlog = new System.Windows.Forms.Button();
            this.btn_stopscript = new System.Windows.Forms.Button();
            this.tboxSend = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.guiupdatetimer = new System.Windows.Forms.Timer(this.components);
            this.txtScriptBox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(12, 397);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(709, 134);
            this.txtOutput.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 163);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(92, 32);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 2;
            this.txtUsername.Text = "username";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(92, 58);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Text = "pass";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(92, 84);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 20);
            this.txtIP.TabIndex = 4;
            this.txtIP.Text = "pandorauo.zapto.org";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(92, 110);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 20);
            this.txtPort.TabIndex = 5;
            this.txtPort.Text = "2593";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "IP:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Port:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(888, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // txtCharSlot
            // 
            this.txtCharSlot.Location = new System.Drawing.Point(276, 32);
            this.txtCharSlot.Name = "txtCharSlot";
            this.txtCharSlot.Size = new System.Drawing.Size(52, 20);
            this.txtCharSlot.TabIndex = 12;
            this.txtCharSlot.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(198, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Character slot";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(474, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Run Script";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dissconnect
            // 
            this.dissconnect.Location = new System.Drawing.Point(92, 163);
            this.dissconnect.Name = "dissconnect";
            this.dissconnect.Size = new System.Drawing.Size(75, 23);
            this.dissconnect.TabIndex = 15;
            this.dissconnect.Text = "Disconnect";
            this.dissconnect.UseVisualStyleBackColor = true;
            // 
            // vartree
            // 
            this.vartree.Location = new System.Drawing.Point(727, 32);
            this.vartree.Name = "vartree";
            this.vartree.Size = new System.Drawing.Size(153, 528);
            this.vartree.TabIndex = 16;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(642, 134);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 17;
            this.button3.Text = "test";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btn_clearlog
            // 
            this.btn_clearlog.Location = new System.Drawing.Point(642, 163);
            this.btn_clearlog.Name = "btn_clearlog";
            this.btn_clearlog.Size = new System.Drawing.Size(75, 23);
            this.btn_clearlog.TabIndex = 18;
            this.btn_clearlog.Text = "Clear Log";
            this.btn_clearlog.UseVisualStyleBackColor = true;
            this.btn_clearlog.Click += new System.EventHandler(this.btn_clearlog_Click);
            // 
            // btn_stopscript
            // 
            this.btn_stopscript.Location = new System.Drawing.Point(561, 163);
            this.btn_stopscript.Name = "btn_stopscript";
            this.btn_stopscript.Size = new System.Drawing.Size(75, 23);
            this.btn_stopscript.TabIndex = 19;
            this.btn_stopscript.Text = "Stop Script";
            this.btn_stopscript.UseVisualStyleBackColor = true;
            this.btn_stopscript.Click += new System.EventHandler(this.btn_stopscript_Click);
            // 
            // tboxSend
            // 
            this.tboxSend.Location = new System.Drawing.Point(12, 537);
            this.tboxSend.Name = "tboxSend";
            this.tboxSend.Size = new System.Drawing.Size(640, 20);
            this.tboxSend.TabIndex = 20;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(662, 537);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(59, 23);
            this.btnSend.TabIndex = 21;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // guiupdatetimer
            // 
            this.guiupdatetimer.Enabled = true;
            this.guiupdatetimer.Interval = 1000;
            this.guiupdatetimer.Tick += new System.EventHandler(this.updateVarsTimer_Tick);
            // 
            // txtScriptBox
            // 
            this.txtScriptBox.Location = new System.Drawing.Point(12, 192);
            this.txtScriptBox.Multiline = true;
            this.txtScriptBox.Name = "txtScriptBox";
            this.txtScriptBox.Size = new System.Drawing.Size(708, 194);
            this.txtScriptBox.TabIndex = 22;
            this.txtScriptBox.Text = "Paste Script Here";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 572);
            this.Controls.Add(this.txtScriptBox);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tboxSend);
            this.Controls.Add(this.btn_stopscript);
            this.Controls.Add(this.btn_clearlog);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.vartree);
            this.Controls.Add(this.dissconnect);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCharSlot);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "drkUO";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox txtCharSlot;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button dissconnect;
        private System.Windows.Forms.TreeView vartree;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btn_clearlog;
        private System.Windows.Forms.Button btn_stopscript;
        private System.Windows.Forms.TextBox tboxSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Timer guiupdatetimer;
        private System.Windows.Forms.TextBox txtScriptBox;
    }
}


