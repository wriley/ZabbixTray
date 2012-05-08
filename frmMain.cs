using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Ini;
using Zabbix;
using System.IO;

namespace ZabbixTray
{
    delegate void simplefunc();

    public partial class frmMain : Form
    {
        #region Private
        private ZabbixAPI zApi;
        private static string[] icon_names = {
            "ZabbixTray.icon_off.ico",
            "ZabbixTray.icon_information.ico",
            "ZabbixTray.icon_warning.ico",
            "ZabbixTray.icon_average.ico",
            "ZabbixTray.icon_high.ico",
            "ZabbixTray.icon_disaster.ico",
            "ZabbixTray.icon_normal.ico",
        };
        private static BindingSource bsTriggers = new BindingSource();
        private int numAlerts = 0;
        private int highestPriority = 0;
        private string apiURL = null;
        private string apiUsername = null;
        private string apiPassword = null;
        private int checkInterval = 60;
        private int minPriority = 3;
        private bool showAck = true;
        private bool showPopup = true;
        private Hashtable priorityValues = new Hashtable();
        private Hashtable priorityColors = new Hashtable();
        private DataGridViewCellStyle[] cellStyles = new DataGridViewCellStyle[6];
        private string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        #endregion

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            priorityValues.Add(1, "Information");
            priorityValues.Add(2, "Warning");
            priorityValues.Add(3, "Average");
            priorityValues.Add(4, "High");
            priorityValues.Add(5, "Disaster");

            priorityColors.Add(0, "cecece");
            priorityColors.Add(1, "bbe2bb");
            priorityColors.Add(2, "efefcc");
            priorityColors.Add(3, "ddaaaa");
            priorityColors.Add(4, "ff8888");
            priorityColors.Add(5, "ff0000");
            priorityColors.Add(6, "aaffaa");

            for (int i = 1; i < 6; i++)
            {
                cellStyles[i] = new DataGridViewCellStyle();
                cellStyles[i].BackColor = priorityToColor(i);
            }

            loadSettings();
            setIcon(0);
            lblCheckInterval.Text = checkInterval.ToString();
            lblAlerts.Text = "";
            lblLastCheck.Text = "";

            Connect();
        }

        public string ApiURL
        {
            set { apiURL = value; }
            get { return apiURL; }
        }

        public string ApiUsername
        {
            set { apiUsername = value; }
            get { return apiUsername; }
        }

        public string ApiPassword
        {
            set { apiPassword = value; }
            get { return apiPassword; }
        }

        public int CheckInterval
        {
            set
            {
                checkInterval = value;
                lblCheckInterval.Text = value.ToString();
                if (zApi != null)
                {
                    zApi.setInterval(checkInterval);
                }
            }

            get { return checkInterval; }
        }

        public int MinPriority
        {
            set
            {
                minPriority = value;
                if (zApi != null)
                {
                    zApi.setMinSeverity(minPriority.ToString());
                }
            }
            get { return minPriority; }
        }

        public bool ShowAck
        {
            set {
                showAck = value;
                if (zApi != null)
                {
                    if (showAck)
                    {
                        zApi.setHideAck(0);
                    }
                    else
                    {
                        zApi.setHideAck(1);
                    }
                }
            }
            get { return showAck; }
        }

        public bool ShowPopup
        {
            set { showPopup = value; }
            get { return showPopup; }
        }

        private void loadSettings()
        {
            if (!File.Exists(exePath + "\\ZabbixTray.ini"))
            {
                return;
            }

            IniFile ini = new IniFile(exePath + "\\ZabbixTray.ini");
            apiURL = ini.IniReadValue("Options", "apiURL");
            apiUsername = ini.IniReadValue("Options", "apiUsername");
            apiPassword = ini.IniReadValue("Options", "apiPassword");

            try
            {
                checkInterval = Int32.Parse(ini.IniReadValue("Options", "checkInterval"));
                if (zApi != null)
                {
                    zApi.setInterval(checkInterval);
                }
            }
            catch (Exception ex)
            {
                Debug("Error parsing checkInterval: " + ex.Message);
            }

            try
            {
                minPriority = Int32.Parse(ini.IniReadValue("Options", "minPriority"));
                if (zApi != null)
                {
                    zApi.setMinSeverity(minPriority.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug("Error parsing minPriority: " + ex.Message);
            }

            try
            {
                showAck = bool.Parse(ini.IniReadValue("Options", "showAck"));
                if (zApi != null)
                {
                    if (showAck)
                    {
                        zApi.setHideAck(0);
                    }
                    else
                    {
                        zApi.setHideAck(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug("Error parsing showAck: " + ex.Message);
            }

            try
            {
                showPopup = bool.Parse(ini.IniReadValue("Options", "showPopup"));
            }
            catch (Exception ex)
            {
                Debug("Error parsing showPopup: " + ex.Message);
            }
        }

        public void saveSettings()
        {
            IniFile ini = new IniFile(exePath + "\\ZabbixTray.ini");
            ini.IniWriteValue("Options", "apiURL", apiURL);
            ini.IniWriteValue("Options", "apiUsername", apiUsername);
            ini.IniWriteValue("Options", "apiPassword", apiPassword);
            ini.IniWriteValue("Options", "checkInterval", checkInterval.ToString());
            ini.IniWriteValue("Options", "minPriority", minPriority.ToString());
            ini.IniWriteValue("Options", "showAck", showAck.ToString());
            ini.IniWriteValue("Options", "showPopup", showPopup.ToString());
        }

        private void setIcon(int p)
        {
            String strName = icon_names[p];
            systemTrayIcon.Icon = new Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(strName));
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                minimizeMe();
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                restoreMe();
            }
            else
            {
                minimizeMe();
            }
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            restoreMe();
        }

        private void restoreMe()
        {
            Show();
            WindowState = FormWindowState.Normal;
            tsmiRestore.Visible = false;
            tsmiMinimize.Visible = true;
        }

        private void minimizeMe()
        {
            Hide();
            WindowState = FormWindowState.Minimized;
            tsmiRestore.Visible = true;
            tsmiMinimize.Visible = false;
        }

        private void ExitApplication_Click(object sender, EventArgs e)
        {
            myExit();
        }

        private void myExit()
        {
            Disconnect();
            Application.Exit();
        }

        public Color priorityToColor(int p)
        {
            Int32 ir, ig, ib;
            String colorString = priorityColors[p].ToString();
            ir = Int32.Parse(colorString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            ig = Int32.Parse(colorString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            ib = Int32.Parse(colorString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return Color.FromArgb(ir, ig, ib);
        }

        private void updateAlerts()
        {
            DataTable dtTriggers = new DataTable();
            dtTriggers.Columns.Add("Host", typeof(string));
            dtTriggers.Columns.Add("Issue", typeof(string));
            dtTriggers.Columns.Add("Priority", typeof(string));
            dtTriggers.Columns.Add("Last Change", typeof(DateTime));

            if (zApi != null && zApi.triggers[0] != null && zApi.triggers.Count() > 0)
            {
                foreach (Trigger tr in zApi.triggers)
                {
                    string host = tr.host.ToString();
                    string issue = tr.description;
                    string priority = priorityValues[Int32.Parse(tr.priority)].ToString();
                    DateTime lastchange = tr.lastchangeDateTime;
                    dtTriggers.Rows.Add(host, issue, priority, lastchange);
                }
            }

            if (dtTriggers == null)
            {
                lblAlerts.BackColor = priorityToColor(0);
                lblLastCheck.Text = "ERROR";
                setIcon(0);
            }
            else
            {
                bsTriggers.DataSource = dtTriggers;
                dgvTriggers.DataSource = bsTriggers;
                dgvTriggers.ClearSelection();
                numAlerts = dgvTriggers.RowCount;
                lblAlerts.Text = numAlerts.ToString();

                for (int i = 0; i < dgvTriggers.RowCount; i++)
                {
                    try
                    {
                        int p = getPriorityKey(dgvTriggers.Rows[i].Cells["Priority"].Value.ToString());
                        if (p > highestPriority) { highestPriority = p; }
                        dgvTriggers.Rows[i].DefaultCellStyle = cellStyles[p];
                    }
                    catch (Exception ex)
                    {
                        Debug(ex.Message);
                    }

                }

                if (numAlerts > 0)
                {
                    lblAlerts.BackColor = priorityToColor(highestPriority);
                    if (showPopup)
                    {
                        showBalloon();
                    }
                    setIcon(highestPriority);
                }
                else
                {
                    lblAlerts.BackColor = priorityToColor(6);
                    setIcon(6);
                }

                lblLastCheck.Text = DateTime.Now.ToLocalTime().ToString();

                
            }

            lblMinPriority.Text = getPriorityValue(minPriority);
            lblMinPriority.BackColor = priorityToColor(minPriority);
        }

        public int getPriorityKey(string val)
        {
            if (priorityValues.ContainsValue(val))
            {
                IDictionaryEnumerator ide = priorityValues.GetEnumerator();
                while (ide.MoveNext())
                {
                    if (ide.Value.Equals(val))
                    {
                        return Int32.Parse(ide.Key.ToString());
                    }
                }
            }

            return 3;
        }

        public string getPriorityValue(int key)
        {
            if (priorityValues.ContainsKey(key))
            {
                return priorityValues[key].ToString();
            }
            else
            {
                return "Warning";
            }
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            restoreMe();
        }

        private void showBalloon()
        {
            systemTrayIcon.ShowBalloonTip(3000, null, String.Format("{0} alerts", numAlerts), ToolTipIcon.Warning);
        }

        private void tsmiMinimize_Click(object sender, EventArgs e)
        {
            minimizeMe();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myExit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout fa = new frmAbout();
            fa.ShowDialog(this);
        }

        private void tsmiOptions_Click(object sender, EventArgs e)
        {
            frmOptions fo = new frmOptions(this);
            fo.ShowDialog(this);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOptions fo = new frmOptions(this);
            fo.ShowDialog(this);
        }

        private void btnCheckNow_Click(object sender, EventArgs e)
        {
            updateAlerts();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                minimizeMe();
            }
        }

        private void cmsSystemTrayIcon_DoubleClick(object sender, EventArgs e)
        {
            restoreMe();
        }

        private void Connect()
        {
            if (zApi != null)
            {
                zApi.stop();
            }
            Debug("Creating API connection");
            zApi = new ZabbixAPI(apiURL, apiUsername, apiPassword);
            zApi.onUpdate += updateInfo;
            this.Cursor = Cursors.WaitCursor;
            zApi.setMinSeverity(minPriority.ToString());
            zApi.setInterval(checkInterval);
            if (showAck)
            {
                zApi.setHideAck(0);
            }
            else
            {
                zApi.setHideAck(1);
            }
            zApi.connect();
            lblAPIVersion.Text = "API Version: " + zApi.ApiVersion();
        }

        private void Disconnect()
        {
            if (zApi != null)
            {
                zApi.stop();
            }
        }

        public void reset()
        {
            Disconnect();
            Connect();
        }

        private void updateInfo(UpdateInfoMessage info)
        {
            if (!this.IsDisposed)
            {
                switch (info.status)
                {
                    case "OK":
                        this.Invoke(new simplefunc(() =>
                        {
                            tssMessage.Text = info.message;
                            this.Cursor = Cursors.Arrow;
                        }));
                        break;
                    case "DEBUG":
                        this.Invoke(new simplefunc(() =>
                        {
                            Debug(info.message);
                        }));
                        break;
                    case "TRIGGERS":
                        this.Invoke(new simplefunc(() =>
                        {
                            long ticks = long.Parse(info.message);
                            double ms = ticks / 10000;
                            Debug("Triggers fetched in: " + ms.ToString() + "ms");
                            updateAlerts();
                        }));
                        break;
                    case "HOSTS":
                        this.Invoke(new simplefunc(() =>
                        {
                            long ticks = long.Parse(info.message);
                            double ms = ticks / 10000;
                            Debug("Hosts fetched in: " + ms.ToString() + "ms");
                        }));
                        break;
                    case "REFRESH":
                        break;
                    default:
                        this.Invoke(new simplefunc(() =>
                        {
                            tssMessage.Text = info.message;
                        }));
                        break;
                }
            }
        }

        public void Debug(string m)
        {
            string temp = tbDebug.Text;
            temp += m;
            temp += "\r\n";
            tbDebug.Text = temp;
            tbDebug.SelectionStart = tbDebug.Text.Length;
            tbDebug.ScrollToCaret();
        }
    }
}
