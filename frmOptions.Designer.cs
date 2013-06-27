namespace ZabbixTray
{
    partial class frmOptions
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
            this.lblInterval = new System.Windows.Forms.Label();
            this.cbInterval = new System.Windows.Forms.ComboBox();
            this.lblSecond = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblURL = new System.Windows.Forms.Label();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.gbAPIOptions = new System.Windows.Forms.GroupBox();
            this.lblURLHint = new System.Windows.Forms.Label();
            this.cbShowAck = new System.Windows.Forms.CheckBox();
            this.cbPriority = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbShowPopup = new System.Windows.Forms.CheckBox();
            this.cbIgnoreSSLErrors = new System.Windows.Forms.CheckBox();
            this.gbAPIOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(138, 207);
            this.lblInterval.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(97, 17);
            this.lblInterval.TabIndex = 8;
            this.lblInterval.Text = "Check Interval";
            // 
            // cbInterval
            // 
            this.cbInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInterval.FormattingEnabled = true;
            this.cbInterval.Items.AddRange(new object[] {
            "5",
            "10",
            "30",
            "60",
            "90",
            "120",
            "300",
            "600"});
            this.cbInterval.Location = new System.Drawing.Point(248, 204);
            this.cbInterval.Margin = new System.Windows.Forms.Padding(4);
            this.cbInterval.Name = "cbInterval";
            this.cbInterval.Size = new System.Drawing.Size(96, 24);
            this.cbInterval.TabIndex = 9;
            // 
            // lblSecond
            // 
            this.lblSecond.AutoSize = true;
            this.lblSecond.Location = new System.Drawing.Point(353, 207);
            this.lblSecond.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSecond.Name = "lblSecond";
            this.lblSecond.Size = new System.Drawing.Size(61, 17);
            this.lblSecond.TabIndex = 10;
            this.lblSecond.Text = "seconds";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(173, 336);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 28);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(281, 336);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(24, 27);
            this.lblURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(36, 17);
            this.lblURL.TabIndex = 13;
            this.lblURL.Text = "URL";
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(71, 23);
            this.tbURL.Margin = new System.Windows.Forms.Padding(4);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(405, 22);
            this.tbURL.TabIndex = 14;
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(214, 80);
            this.tbUsername.Margin = new System.Windows.Forms.Padding(4);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(156, 22);
            this.tbUsername.TabIndex = 18;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(133, 84);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(73, 17);
            this.lblUsername.TabIndex = 17;
            this.lblUsername.Text = "Username";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(213, 112);
            this.tbPassword.Margin = new System.Windows.Forms.Padding(4);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(156, 22);
            this.tbPassword.TabIndex = 20;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(134, 116);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(69, 17);
            this.lblPassword.TabIndex = 19;
            this.lblPassword.Text = "Password";
            // 
            // gbAPIOptions
            // 
            this.gbAPIOptions.Controls.Add(this.cbIgnoreSSLErrors);
            this.gbAPIOptions.Controls.Add(this.lblURLHint);
            this.gbAPIOptions.Controls.Add(this.tbURL);
            this.gbAPIOptions.Controls.Add(this.tbPassword);
            this.gbAPIOptions.Controls.Add(this.lblPassword);
            this.gbAPIOptions.Controls.Add(this.lblURL);
            this.gbAPIOptions.Controls.Add(this.tbUsername);
            this.gbAPIOptions.Controls.Add(this.lblUsername);
            this.gbAPIOptions.Location = new System.Drawing.Point(16, 15);
            this.gbAPIOptions.Margin = new System.Windows.Forms.Padding(4);
            this.gbAPIOptions.Name = "gbAPIOptions";
            this.gbAPIOptions.Padding = new System.Windows.Forms.Padding(4);
            this.gbAPIOptions.Size = new System.Drawing.Size(503, 178);
            this.gbAPIOptions.TabIndex = 21;
            this.gbAPIOptions.TabStop = false;
            this.gbAPIOptions.Text = "API";
            // 
            // lblURLHint
            // 
            this.lblURLHint.AutoSize = true;
            this.lblURLHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblURLHint.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblURLHint.Location = new System.Drawing.Point(108, 49);
            this.lblURLHint.Name = "lblURLHint";
            this.lblURLHint.Size = new System.Drawing.Size(287, 17);
            this.lblURLHint.TabIndex = 26;
            this.lblURLHint.Text = "example: http://HOSTNAME/api_jsonrpc.php";
            // 
            // cbShowAck
            // 
            this.cbShowAck.AutoSize = true;
            this.cbShowAck.Location = new System.Drawing.Point(173, 270);
            this.cbShowAck.Margin = new System.Windows.Forms.Padding(4);
            this.cbShowAck.Name = "cbShowAck";
            this.cbShowAck.Size = new System.Drawing.Size(199, 21);
            this.cbShowAck.TabIndex = 22;
            this.cbShowAck.Text = "Show Acknowledged Alerts";
            this.cbShowAck.UseVisualStyleBackColor = true;
            // 
            // cbPriority
            // 
            this.cbPriority.BackColor = System.Drawing.SystemColors.Window;
            this.cbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPriority.FormattingEnabled = true;
            this.cbPriority.Items.AddRange(new object[] {
            "Information",
            "Warning",
            "Average",
            "High",
            "Disaster"});
            this.cbPriority.Location = new System.Drawing.Point(246, 237);
            this.cbPriority.Margin = new System.Windows.Forms.Padding(4);
            this.cbPriority.Name = "cbPriority";
            this.cbPriority.Size = new System.Drawing.Size(133, 24);
            this.cbPriority.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 240);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 17);
            this.label1.TabIndex = 24;
            this.label1.Text = "Minimum Severity";
            // 
            // cbShowPopup
            // 
            this.cbShowPopup.AutoSize = true;
            this.cbShowPopup.Location = new System.Drawing.Point(173, 298);
            this.cbShowPopup.Margin = new System.Windows.Forms.Padding(4);
            this.cbShowPopup.Name = "cbShowPopup";
            this.cbShowPopup.Size = new System.Drawing.Size(174, 21);
            this.cbShowPopup.TabIndex = 25;
            this.cbShowPopup.Text = "Show Pop-up for alerts";
            this.cbShowPopup.UseVisualStyleBackColor = true;
            // 
            // cbIgnoreSSLErrors
            // 
            this.cbIgnoreSSLErrors.AutoSize = true;
            this.cbIgnoreSSLErrors.Location = new System.Drawing.Point(180, 142);
            this.cbIgnoreSSLErrors.Margin = new System.Windows.Forms.Padding(4);
            this.cbIgnoreSSLErrors.Name = "cbIgnoreSSLErrors";
            this.cbIgnoreSSLErrors.Size = new System.Drawing.Size(142, 21);
            this.cbIgnoreSSLErrors.TabIndex = 27;
            this.cbIgnoreSSLErrors.Text = "Ignore SSL errors";
            this.cbIgnoreSSLErrors.UseVisualStyleBackColor = true;
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 388);
            this.Controls.Add(this.cbShowPopup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPriority);
            this.Controls.Add(this.cbShowAck);
            this.Controls.Add(this.gbAPIOptions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblSecond);
            this.Controls.Add(this.cbInterval);
            this.Controls.Add(this.lblInterval);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmOptions";
            this.Text = "ZabbixTray Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.gbAPIOptions.ResumeLayout(false);
            this.gbAPIOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.ComboBox cbInterval;
        private System.Windows.Forms.Label lblSecond;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.GroupBox gbAPIOptions;
        private System.Windows.Forms.CheckBox cbShowAck;
        private System.Windows.Forms.ComboBox cbPriority;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbShowPopup;
        private System.Windows.Forms.Label lblURLHint;
        private System.Windows.Forms.CheckBox cbIgnoreSSLErrors;
    }
}