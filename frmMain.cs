using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZabbixTray
{
    public partial class frmMain : Form
    {
        private static string ICON_OFF = "ZabbixTray.ztIcon_off.ico";
        private static string ICON_OK = "ZabbixTray.ztIcon_ok.ico";
        private static string ICON_ALERT = "ZabbixTray.ztIcon_alert.ico";

        private static Color COLOR_OK = Color.LimeGreen;
        private static Color COLOR_ALERT = Color.LightCoral;
        private static Color COLOR_ERROR = Color.Yellow;

        private static BindingSource bindingSource1 = new BindingSource();
        private DataTable dtAlerts;
        private int numAlerts = 0;

        private string dbServer = ZabbixTray.Properties.Settings.Default.dbServer;
        private string dbDatabase = ZabbixTray.Properties.Settings.Default.dbDatabase;
        private string dbUsername = ZabbixTray.Properties.Settings.Default.dbUsername;
        private string dbPassword = ZabbixTray.Properties.Settings.Default.dbPassword;
        private int checkInterval = ZabbixTray.Properties.Settings.Default.checkInterval;
        private bool showAck = ZabbixTray.Properties.Settings.Default.showAck;

        private MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
        private MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
        private MySql.Data.MySqlClient.MySqlDataAdapter myAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            setIcon(ICON_OFF);
            lblCheckInterval.Text = checkInterval.ToString();
            tmrMySQL.Interval = (checkInterval * 1000);
            lblAlerts.Text = "";
            lblLastCheck.Text = "";
            updateAlerts();
            tmrMySQL.Enabled = true;
        }

        public string DbServer
        {
            set { dbServer = value; }
            get { return dbServer; }
        }

        public string DbDatabase
        {
            set { dbDatabase = value; }
            get { return dbDatabase; }
        }

        public string DbUsername
        {
            set { dbUsername = value; }
            get { return dbUsername; }
        }

        public string DbPassword
        {
            set { dbPassword = value; }
            get { return dbPassword; }
        }

        public int CheckInterval
        {
            set
            {
                checkInterval = value;
                tmrMySQL.Interval = (checkInterval * 1000);
                tmrMySQL.Start();
                lblCheckInterval.Text = value.ToString();
            }

            get { return checkInterval; }
        }

        public bool ShowAck
        {
            set { showAck = value; }
            get { return showAck; }
        }

        public void saveSettings()
        {
            ZabbixTray.Properties.Settings.Default.dbServer = dbServer;
            ZabbixTray.Properties.Settings.Default.dbDatabase = dbDatabase;
            ZabbixTray.Properties.Settings.Default.dbUsername = dbUsername;
            ZabbixTray.Properties.Settings.Default.dbPassword = dbPassword;
            ZabbixTray.Properties.Settings.Default.checkInterval = checkInterval;
            ZabbixTray.Properties.Settings.Default.Save();
        }

        private void setIcon(string strName)
        {
            systemTrayIcon.Icon = new Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(strName));
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Hide();
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

        private void CloseApplication_Click(object sender, EventArgs e)
        {
            Close();
        }

        private DataTable getData()
        {
            conn.ConnectionString = String.Format("server={0};uid={1};pwd={2};database={3};", dbServer, dbUsername, dbPassword, dbDatabase);

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;

            string commandString = "SELECT DISTINCT h.host,t.priority,t.description,e.acknowledged FROM triggers t LEFT JOIN functions f ON f.triggerid = t.triggerid LEFT JOIN items i ON i.itemid = f.itemid LEFT JOIN hosts h ON h.hostid = i.hostid LEFT JOIN events e ON e.clock = t.lastchange WHERE t.value = 1 AND t.status = 0";
            if (showAck)
            {
                commandString += " AND e.acknowledged > -1";
            }
            else
            {
                commandString += " AND e.acknowledged = 0";
            }

            try
            {
                cmd.CommandText = commandString;
                cmd.Connection = conn;

                myAdapter.SelectCommand = cmd;
                myAdapter.Fill(table);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                systemTrayIcon.ShowBalloonTip(5000, null, "Error: " + ex.Message.ToString(), ToolTipIcon.Error);
                return null;
            }
                        
            return table;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateAlerts();
        }

        public void updateAlerts()
        {
            dtAlerts = getData();
            if (dtAlerts == null)
            {
                lblAlerts.BackColor = COLOR_ERROR;
                lblLastCheck.Text = "ERROR";
            }
            else
            {
                bindingSource1.DataSource = dtAlerts;
                dgvAlerts.DataSource = bindingSource1;
                numAlerts = dtAlerts.Rows.Count;
                lblAlerts.Text = numAlerts.ToString();

                if (numAlerts > 0)
                {
                    lblAlerts.BackColor = COLOR_ALERT;
                    showBalloon();
                    setIcon(ICON_ALERT);
                }
                else
                {
                    lblAlerts.BackColor = COLOR_OK;
                    setIcon(ICON_OK);
                }

                lblLastCheck.Text = DateTime.Now.ToLocalTime().ToString();
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
            Close();
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
    }
}
