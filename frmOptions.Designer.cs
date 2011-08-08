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
            this.cbShowAck = new System.Windows.Forms.CheckBox();
            this.cbPriority = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbShowPopup = new System.Windows.Forms.CheckBox();
            this.gbAPIOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(103, 131);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(76, 13);
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
            this.cbInterval.Location = new System.Drawing.Point(185, 128);
            this.cbInterval.Name = "cbInterval";
            this.cbInterval.Size = new System.Drawing.Size(73, 21);
            this.cbInterval.TabIndex = 9;
            // 
            // lblSecond
            // 
            this.lblSecond.AutoSize = true;
            this.lblSecond.Location = new System.Drawing.Point(264, 131);
            this.lblSecond.Name = "lblSecond";
            this.lblSecond.Size = new System.Drawing.Size(47, 13);
            this.lblSecond.TabIndex = 10;
            this.lblSecond.Text = "seconds";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(129, 236);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(210, 236);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(18, 22);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(29, 13);
            this.lblURL.TabIndex = 13;
            this.lblURL.Text = "URL";
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(53, 19);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(305, 20);
            this.tbURL.TabIndex = 14;
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(160, 45);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(118, 20);
            this.tbUsername.TabIndex = 18;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(99, 48);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 17;
            this.lblUsername.Text = "Username";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(159, 71);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(118, 20);
            this.tbPassword.TabIndex = 20;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(100, 74);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 19;
            this.lblPassword.Text = "Password";
            // 
            // gbAPIOptions
            // 
            this.gbAPIOptions.Controls.Add(this.tbURL);
            this.gbAPIOptions.Controls.Add(this.tbPassword);
            this.gbAPIOptions.Controls.Add(this.lblPassword);
            this.gbAPIOptions.Controls.Add(this.lblURL);
            this.gbAPIOptions.Controls.Add(this.tbUsername);
            this.gbAPIOptions.Controls.Add(this.lblUsername);
            this.gbAPIOptions.Location = new System.Drawing.Point(12, 12);
            this.gbAPIOptions.Name = "gbAPIOptions";
            this.gbAPIOptions.Size = new System.Drawing.Size(377, 110);
            this.gbAPIOptions.TabIndex = 21;
            this.gbAPIOptions.TabStop = false;
            this.gbAPIOptions.Text = "API";
            // 
            // cbShowAck
            // 
            this.cbShowAck.AutoSize = true;
            this.cbShowAck.Location = new System.Drawing.Point(129, 182);
            this.cbShowAck.Name = "cbShowAck";
            this.cbShowAck.Size = new System.Drawing.Size(156, 17);
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
            this.cbPriority.Location = new System.Drawing.Point(184, 155);
            this.cbPriority.Name = "cbPriority";
            this.cbPriority.Size = new System.Drawing.Size(101, 21);
            this.cbPriority.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Minimum Severity";
            // 
            // cbShowPopup
            // 
            this.cbShowPopup.AutoSize = true;
            this.cbShowPopup.Location = new System.Drawing.Point(129, 205);
            this.cbShowPopup.Name = "cbShowPopup";
            this.cbShowPopup.Size = new System.Drawing.Size(133, 17);
            this.cbShowPopup.TabIndex = 25;
            this.cbShowPopup.Text = "Show Pop-up for alerts";
            this.cbShowPopup.UseVisualStyleBackColor = true;
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 276);
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
    }
}