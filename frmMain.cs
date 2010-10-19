using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IniFile;

namespace ZabbixTray
{
    public partial class frmMain : Form
    {
        private static string ICON_OFF = "ZabbixTray.ztIcon_off.ico";
        private static string ICON_OK = "ZabbixTray.ztIcon_ok.ico";
        private static string ICON_ALERT = "ZabbixTray.ztIcon_alert.ico";

        private static string myIniFileName = "ZabbixTray.ini";
        private static string myIniFileSectionName = "ZabbixTray";
        private IniFileReader ifr;

        private static Color COLOR_OK = Color.LimeGreen;
        private static Color COLOR_ALERT = Color.LightCoral;
        private static Color COLOR_ERROR = Color.Yellow;

        private static BindingSource bindingSource1 = new BindingSource();
        private DataTable dtAlerts;
        private int numAlerts = 0;

        private string dbServer = null;
        private string dbDatabase = null;
        private string dbUsername = null;
        private string dbPassword = null;
        private int checkInterval = 60;
        private int minPriority = 3;
        private bool showAck = true;
        private bool showPopup = true;

        Hashtable priorityValues = new Hashtable();
        Hashtable priorityColors = new Hashtable();

        private MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
        private MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
        private MySql.Data.MySqlClient.MySqlDataAdapter myAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter();

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

            priorityColors.Add(1, "bbe2bb");
            priorityColors.Add(2, "efefcc");
            priorityColors.Add(3, "ddaaaa");
            priorityColors.Add(4, "ff8888");
            priorityColors.Add(5, "ff0000");

            ifr = new IniFileReader(myIniFileName);
            ifr.OutputFilename = myIniFileName;
            loadSettings();
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

        public int MinPriority
        {
            set { minPriority = value; }
            get { return minPriority; }
        }

        public bool ShowAck
        {
            set { showAck = value; }
            get { return showAck; }
        }

        public bool ShowPopup
        {
            set { showPopup = value; }
            get { return showPopup; }
        }

        private void loadSettings()
        {
            try
            {
                dbServer = ifr.GetIniValue(myIniFileSectionName, "dbServer");
                dbDatabase = ifr.GetIniValue(myIniFileSectionName, "dbDatabase");
                dbUsername = ifr.GetIniValue(myIniFileSectionName, "dbUsername");
                dbPassword = ifr.GetIniValue(myIniFileSectionName, "dbPassword");
                checkInterval = Int32.Parse(ifr.GetIniValue(myIniFileSectionName, "checkInterval"));
                minPriority = Int32.Parse(ifr.GetIniValue(myIniFileSectionName, "minPriority"));
                showAck = bool.Parse(ifr.GetIniValue(myIniFileSectionName, "showAck"));
                showPopup = bool.Parse(ifr.GetIniValue(myIniFileSectionName, "showPopup"));
            }
            catch (Exception)
            {
            }
        }

        public void saveSettings()
        {
            ifr.SetIniValue(myIniFileSectionName, "dbServer", dbServer);
            ifr.SetIniValue(myIniFileSectionName, "dbDatabase", dbDatabase);
            ifr.SetIniValue(myIniFileSectionName, "dbUsername", dbUsername);
            ifr.SetIniValue(myIniFileSectionName, "dbPassword", dbPassword);
            ifr.SetIniValue(myIniFileSectionName, "checkInterval", checkInterval.ToString());
            ifr.SetIniValue(myIniFileSectionName, "minPriority", minPriority.ToString());
            ifr.SetIniValue(myIniFileSectionName, "showAck", showAck.ToString());
            ifr.SetIniValue(myIniFileSectionName, "showPopup", showPopup.ToString());
            ifr.Save();
        }

        private void setIcon(string strName)
        {
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

        private void CloseApplication_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private DataTable getData()
        {
            conn.ConnectionString = String.Format("server={0};uid={1};pwd={2};database={3};", dbServer, dbUsername, dbPassword, dbDatabase);

            DataSet results = new DataSet();

            //string commandString = "SELECT DISTINCT t.triggerid,t.description,t.priority FROM triggers t WHERE ((t.triggerid  BETWEEN 000000000000000 AND 099999999999999)) AND  NOT EXISTS ( SELECT ff.functionid FROM functions ff WHERE ff.triggerid=t.triggerid AND EXISTS ( SELECT ii.itemid FROM items ii, hosts hh WHERE ff.itemid=ii.itemid AND hh.hostid=ii.hostid AND ( ii.status<>0 OR hh.status<>0 ) ) ) AND t.status=0 AND t.value=1 AND t.priority >= " + minPriority.ToString() + " ORDER BY t.lastchange DESC";
            string commandString = "SELECT DISTINCT h.host,t.description,t.priority,t.triggerid FROM triggers t,functions f,items i,hosts h WHERE ((t.triggerid  BETWEEN 000000000000000 AND 099999999999999)) AND  NOT EXISTS ( SELECT ff.functionid FROM functions ff WHERE ff.triggerid=t.triggerid AND EXISTS ( SELECT ii.itemid FROM items ii, hosts hh WHERE ff.itemid=ii.itemid AND hh.hostid=ii.hostid AND ( ii.status<>0 OR hh.status<>0 ) ) ) AND t.status=0 AND t.value=1 AND f.triggerid=t.triggerid AND f.itemid=i.itemid AND h.hostid=i.hostid AND t.priority >= " + minPriority.ToString() + " ORDER BY t.lastchange DESC";

            try
            {
                cmd.CommandText = commandString;
                cmd.Connection = conn;

                myAdapter.SelectCommand = cmd;
                myAdapter.Fill(results);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                systemTrayIcon.ShowBalloonTip(5000, null, "Error: " + ex.Message.ToString(), ToolTipIcon.Error);
                return null;
            }

            results.Tables[0].Columns.Add("severity");

            foreach (DataRow dr in results.Tables[0].Rows)
            {
                if (!showAck)
                {
                    commandString = "SELECT e.acknowledged FROM events e WHERE e.object=0 AND e.objectid=" + dr["triggerid"].ToString() + " AND e.value=1 ORDER by e.object DESC, e.objectid DESC, e.eventid DESC LIMIT 1 OFFSET 0";
                    cmd.CommandText = commandString;
                    cmd.Connection = conn;

                    myAdapter.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    myAdapter.Fill(ds);
                    foreach (DataRow dsdr in ds.Tables[0].Rows)
                    {
                        if (dsdr[0].ToString() == "1")
                        {
                            dr.Delete();
                        }
                    }
                }

                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["description"] = dr["description"].ToString().Replace("{HOSTNAME}", dr["host"].ToString());
                    dr["severity"] = getPriorityValue(Int32.Parse(dr["priority"].ToString()));
                }
            }
            results.Tables[0].Columns.Remove("triggerid");
            results.Tables[0].Columns.Remove("host");
            results.Tables[0].Columns.Remove("priority");
            results.AcceptChanges();
            return results.Tables[0];
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
                setIcon(ICON_OFF);
            }
            else
            {
                bindingSource1.DataSource = dtAlerts;
                dgvAlerts.DataSource = bindingSource1;
                DataView dv = new DataView(dtAlerts);
                dv.RowStateFilter = DataViewRowState.CurrentRows;
                numAlerts = dv.Count;
                lblAlerts.Text = numAlerts.ToString();

                if (numAlerts > 0)
                {
                    lblAlerts.BackColor = COLOR_ALERT;
                    if (showPopup)
                    {
                        showBalloon();
                    }
                    setIcon(ICON_ALERT);
                }
                else
                {
                    lblAlerts.BackColor = COLOR_OK;
                    setIcon(ICON_OK);
                }

                lblLastCheck.Text = DateTime.Now.ToLocalTime().ToString();
            }

            lblMinPriority.Text = getPriorityValue(minPriority);
            Int32 ir, ig, ib;
            String colorString = priorityColors[minPriority].ToString();
            ir = Int32.Parse(colorString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            ig = Int32.Parse(colorString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            ib = Int32.Parse(colorString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            lblMinPriority.BackColor = Color.FromArgb(ir, ig, ib);
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
            Application.Exit();
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

        private void checkNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateAlerts();
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
    }
}
