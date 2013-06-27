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
            tbURL.Text = parentForm.ApiURL;
            tbUsername.Text = parentForm.ApiUsername;
            tbPassword.Text = parentForm.ApiPassword;

            cbInterval.SelectedIndex = cbInterval.Items.IndexOf(parentForm.CheckInterval.ToString());
            
            cbPriority.SelectedIndex = cbPriority.Items.IndexOf(parentForm.getPriorityValue(parentForm.MinPriority));
            if (parentForm.ShowAck)
            {
                cbShowAck.Checked = true;
            }
            else
            {
                cbShowAck.Checked = false;
            }
            
            if (parentForm.ShowPopup)
            {
                cbShowPopup.Checked = true;
            }
            else
            {
                cbShowPopup.Checked = false;
            }
            
            if (parentForm.IgnoreSSLErrors)
            {
                cbIgnoreSSLErrors.Checked = true;
            }
            else
            {
                cbIgnoreSSLErrors.Checked = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            parentForm.ApiURL = tbURL.Text;
            parentForm.ApiUsername = tbUsername.Text;
            parentForm.ApiPassword = tbPassword.Text;
            parentForm.CheckInterval = Int32.Parse(cbInterval.SelectedItem.ToString());
            parentForm.MinPriority = parentForm.getPriorityKey(cbPriority.SelectedItem.ToString());
            parentForm.ShowAck = cbShowAck.Checked;
            parentForm.ShowPopup = cbShowPopup.Checked;
            parentForm.IgnoreSSLErrors = cbIgnoreSSLErrors.Checked;
            parentForm.saveSettings();
            parentForm.reset();
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
