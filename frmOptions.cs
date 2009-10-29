using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ZabbixTray
{
    public partial class frmOptions : Form
    {
        frmMain parentForm;

        public frmOptions(frmMain frm)
        {
            InitializeComponent();
            parentForm = new frmMain();
            parentForm = frm;
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            tbServer.Text = parentForm.DbServer;
            tbDatabase.Text = parentForm.DbDatabase;
            tbUsername.Text = parentForm.DbUsername;
            tbPassword.Text = parentForm.DbPassword;
            cbInterval.SelectedIndex = cbInterval.Items.IndexOf(parentForm.CheckInterval.ToString());
            if (parentForm.ShowAck)
            {
                cbShowAck.Checked = true;
            }
            else
            {
                cbShowAck.Checked = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            parentForm.DbServer = tbServer.Text;
            parentForm.DbDatabase = tbDatabase.Text;
            parentForm.DbUsername = tbUsername.Text;
            parentForm.DbPassword = tbPassword.Text;
            parentForm.CheckInterval = Int32.Parse(cbInterval.SelectedItem.ToString());
            parentForm.ShowAck = cbShowAck.Checked;
            parentForm.saveSettings();
            parentForm.updateAlerts();
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
