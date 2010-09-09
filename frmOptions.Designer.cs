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
            this.lblServer = new System.Windows.Forms.Label();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbDatabase = new System.Windows.Forms.TextBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.gbDatabaseOptions = new System.Windows.Forms.GroupBox();
            this.cbShowAck = new System.Windows.Forms.CheckBox();
            this.cbPriority = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbDatabaseOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(96, 159);
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
            this.cbInterval.Location = new System.Drawing.Point(178, 156);
            this.cbInterval.Name = "cbInterval";
            this.cbInterval.Size = new System.Drawing.Size(73, 21);
            this.cbInterval.TabIndex = 9;
            // 
            // lblSecond
            // 
            this.lblSecond.AutoSize = true;
            this.lblSecond.Location = new System.Drawing.Point(257, 159);
            this.lblSecond.Name = "lblSecond";
            this.lblSecond.Size = new System.Drawing.Size(47, 13);
            this.lblSecond.TabIndex = 10;
            this.lblSecond.Text = "seconds";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(122, 264);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(203, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(76, 22);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 13;
            this.lblServer.Text = "Server";
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(120, 19);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(181, 20);
            this.tbServer.TabIndex = 14;
            // 
            // tbDatabase
            // 
            this.tbDatabase.Location = new System.Drawing.Point(120, 45);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Size = new System.Drawing.Size(118, 20);
            this.tbDatabase.TabIndex = 16;
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(61, 48);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(53, 13);
            this.lblDatabase.TabIndex = 15;
            this.lblDatabase.Text = "Database";
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(120, 71);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(118, 20);
            this.tbUsername.TabIndex = 18;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(59, 74);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 17;
            this.lblUsername.Text = "Username";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(120, 97);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(118, 20);
            this.tbPassword.TabIndex = 20;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(61, 100);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 19;
            this.lblPassword.Text = "Password";
            // 
            // gbDatabaseOptions
            // 
            this.gbDatabaseOptions.Controls.Add(this.tbServer);
            this.gbDatabaseOptions.Controls.Add(this.tbPassword);
            this.gbDatabaseOptions.Controls.Add(this.lblPassword);
            this.gbDatabaseOptions.Controls.Add(this.lblServer);
            this.gbDatabaseOptions.Controls.Add(this.lblDatabase);
            this.gbDatabaseOptions.Controls.Add(this.tbUsername);
            this.gbDatabaseOptions.Controls.Add(this.lblUsername);
            this.gbDatabaseOptions.Controls.Add(this.tbDatabase);
            this.gbDatabaseOptions.Location = new System.Drawing.Point(12, 12);
            this.gbDatabaseOptions.Name = "gbDatabaseOptions";
            this.gbDatabaseOptions.Size = new System.Drawing.Size(377, 138);
            this.gbDatabaseOptions.TabIndex = 21;
            this.gbDatabaseOptions.TabStop = false;
            this.gbDatabaseOptions.Text = "Database";
            // 
            // cbShowAck
            // 
            this.cbShowAck.AutoSize = true;
            this.cbShowAck.Location = new System.Drawing.Point(122, 210);
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
            this.cbPriority.Location = new System.Drawing.Point(177, 183);
            this.cbPriority.Name = "cbPriority";
            this.cbPriority.Size = new System.Drawing.Size(101, 21);
            this.cbPriority.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Minimum Severity";
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 299);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPriority);
            this.Controls.Add(this.cbShowAck);
            this.Controls.Add(this.gbDatabaseOptions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblSecond);
            this.Controls.Add(this.cbInterval);
            this.Controls.Add(this.lblInterval);
            this.Name = "frmOptions";
            this.Text = "ZabbixTray Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.gbDatabaseOptions.ResumeLayout(false);
            this.gbDatabaseOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.ComboBox cbInterval;
        private System.Windows.Forms.Label lblSecond;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.TextBox tbDatabase;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.GroupBox gbDatabaseOptions;
        private System.Windows.Forms.CheckBox cbShowAck;
        private System.Windows.Forms.ComboBox cbPriority;
        private System.Windows.Forms.Label label1;
    }
}