namespace DarkLiteUO
{
    partial class myTabPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtScriptBox = new System.Windows.Forms.TextBox();
            this.btn_clearlog = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_stopscript = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.vartree = new System.Windows.Forms.TreeView();
            this.tboxSend = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.updateVarsTimer = new System.Windows.Forms.Timer(this.components);
            this.Close = new System.Windows.Forms.Button();
            this.btnDisplayGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtScriptBox
            // 
            this.txtScriptBox.Location = new System.Drawing.Point(3, 32);
            this.txtScriptBox.Multiline = true;
            this.txtScriptBox.Name = "txtScriptBox";
            this.txtScriptBox.Size = new System.Drawing.Size(409, 82);
            this.txtScriptBox.TabIndex = 22;
            this.txtScriptBox.Text = "Paste Script Here";
            // 
            // btn_clearlog
            // 
            this.btn_clearlog.Location = new System.Drawing.Point(169, 3);
            this.btn_clearlog.Name = "btn_clearlog";
            this.btn_clearlog.Size = new System.Drawing.Size(75, 23);
            this.btn_clearlog.TabIndex = 18;
            this.btn_clearlog.Text = "Clear Log";
            this.btn_clearlog.UseVisualStyleBackColor = true;
            this.btn_clearlog.Click += new System.EventHandler(this.btn_clearlog_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(250, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Run Script";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_stopscript
            // 
            this.btn_stopscript.Location = new System.Drawing.Point(337, 3);
            this.btn_stopscript.Name = "btn_stopscript";
            this.btn_stopscript.Size = new System.Drawing.Size(75, 23);
            this.btn_stopscript.TabIndex = 19;
            this.btn_stopscript.Text = "Stop Script";
            this.btn_stopscript.UseVisualStyleBackColor = true;
            this.btn_stopscript.Click += new System.EventHandler(this.btn_stopscript_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(3, 120);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(406, 124);
            this.txtOutput.TabIndex = 0;
            // 
            // vartree
            // 
            this.vartree.Location = new System.Drawing.Point(418, 3);
            this.vartree.Name = "vartree";
            this.vartree.Size = new System.Drawing.Size(153, 284);
            this.vartree.TabIndex = 16;
            // 
            // tboxSend
            // 
            this.tboxSend.Location = new System.Drawing.Point(0, 250);
            this.tboxSend.Name = "tboxSend";
            this.tboxSend.Size = new System.Drawing.Size(331, 20);
            this.tboxSend.TabIndex = 20;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(350, 250);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(59, 23);
            this.btnSend.TabIndex = 21;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // updateVarsTimer
            // 
            this.updateVarsTimer.Enabled = true;
            this.updateVarsTimer.Interval = 1000;
            this.updateVarsTimer.Tick += new System.EventHandler(this.updateVarsTimer_Tick);
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(3, 3);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 23);
            this.Close.TabIndex = 23;
            this.Close.Text = "Run built in script";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // btnDisplayGame
            // 
            this.btnDisplayGame.Location = new System.Drawing.Point(84, 3);
            this.btnDisplayGame.Name = "btnDisplayGame";
            this.btnDisplayGame.Size = new System.Drawing.Size(75, 23);
            this.btnDisplayGame.TabIndex = 24;
            this.btnDisplayGame.Text = "render";
            this.btnDisplayGame.UseVisualStyleBackColor = true;
            this.btnDisplayGame.Click += new System.EventHandler(this.btnDisplayGame_Click);
            // 
            // myTabPage
            // 
            this.Controls.Add(this.btnDisplayGame);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.txtScriptBox);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btn_clearlog);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tboxSend);
            this.Controls.Add(this.btn_stopscript);
            this.Controls.Add(this.vartree);
            this.Name = "myTabPage";
            this.Size = new System.Drawing.Size(581, 295);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtScriptBox;
        private System.Windows.Forms.Button btn_clearlog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_stopscript;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.TreeView vartree;
        private System.Windows.Forms.TextBox tboxSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Timer updateVarsTimer;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.Button btnDisplayGame;
    }
}
