using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;

// Borrowed heavily from the Open DotNet Zabbix library https://github.com/p1nger/ODZL

namespace Zabbix
{
    public delegate void AlertEvent(string msg);
    public delegate void ThreadEvent(UpdateInfoMessage message);

    public class ZabbixAPI
    {
        #region Private section
        private string authHash = "";
        private int id = 0;
        private string _url;
        private string _username;
        private string _password;
        private string hostgroupID;
        private string minSeverity = "0";
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        private int sleep = 5000;
        private int hideAck = 0;
        #endregion

        #region Public section
        public AlertEvent onAlert { get; set; }
        public ThreadEvent onUpdate { get; set; }
        public Thread mainThread;
        public Hosts hosts;
        public HostGroups hostgroups;
        public Triggers triggers;
        public Events events;
        #endregion

        public ZabbixAPI(string url, string user, string pass)
        {
            serializer.MaxJsonLength = 16777216;
            _url = url;
            _username = user;
            _password = pass;
            triggers = new Triggers(this);
            hosts = new Hosts(this);
            hostgroups = new HostGroups(this);
            mainThread = new Thread(getInfo);
        }

        public void connect()
        {
            try
            {
                mainThread.Start();
            }
            catch (Exception ex)
            {
                Alert("Exception starting mainThread: " + ex.Message);
            }
        }

        public void stop()
        {
            try
            {
                Alert("stop()");
                mainThread.Abort();
                if (mainThread.IsAlive)
                {
                    Alert("Waiting for mainThread to stop");
                    mainThread.Join(5000);
                }
                Alert("mainThread stopped");
                onUpdate(new UpdateInfoMessage(this) { message = "Stopped", status = "OK" });
            }
            catch (Exception ex)
            {
                Alert("Exception stopping main thread: " + ex.Message);
            }
        }

        public bool login()
        {
            bool res = false;
            onUpdate(new UpdateInfoMessage(this) { message = "Trying to authenticate", status = "LOGIN" });
            try
            {
                var userinfo = new { user = _username, password = _password };
                string result = CallAPI("user.authenticate", userinfo);
                authHash = (serializer.Deserialize<simpleresult>(result)).result;
                if (authHash == null)
                {
                    res = false;
                    Update(new UpdateInfoMessage(this) { message = "Login failed", status = "LOGIN" });
                    Alert("Login failed for user: " + _username);
                }
                else
                {
                    res = true;
                    Update(new UpdateInfoMessage(this) { message = "Login was successful", status = "OK" });
                }
            }
            catch (Exception ex)
            {
                Update(new UpdateInfoMessage(this) { message = "Authorization Error:" + ex.Message, status = "LOGIN" });
                res = false;
            }

            return res;
        }

        public void getInfo()
        {
            int err = 0;
            int lasterr = err;
            bool authenticated = login();
            int i = 1;
            while (!authenticated)
            {
                Update(new UpdateInfoMessage(this) { message = "Login attempt " + i.ToString(), status = "LOGIN" });
                i++;
                Thread.Sleep(2000);
                authenticated = login();
            }

//            getHostgroups();
            getHosts();
            getTriggers();

            i = 1;
            while (true)
            {
                try
                {
                    if (err > lasterr)
                    {
                        lasterr = err;
                        login();
                    }
                    refreshTriggers();
                    this.Update(new UpdateInfoMessage(this) { message = i.ToString(), status = "REFRESH" });
                }
                catch (Exception ex)
                {
                    err++;
                    this.Update(new UpdateInfoMessage(this) { message = ex.Message, status = "REFRESH_ERROR" });
                }
                i++;
                Thread.Sleep(sleep);
            }
        }

        public string ApiVersion()
        {
            string result = CallAPI("apiinfo.version", null);
            try
            {
                return (serializer.Deserialize<simpleresult>(result)).result;
            }
            catch(Exception ex)
            {
                Alert("ApiVersion() exception: " + ex.Message);
                return null;
            }
        }

        public void Update(UpdateInfoMessage msg)
        {
            if (onUpdate != null)
            {
                onUpdate(msg);
            }
        }

        public void Alert(string message)
        {
            if (onAlert != null)
            {
                onAlert(message);
            }
        }

        public void refreshTriggers()
        {
            getTriggers();
        }

        private void getTriggers()
        {
            long elapsedTicks;
            var parms = new object { };
            if (hideAck == 0)
            {
                parms = new
                {
                    output = "extend",
                    select_hosts = "extend",
                    monitored = "1",
                    only_true = "1",
                    sortfield = "lastchange",
                    expandDescription = "1",
                    maintenance = "0",
                    min_severity = minSeverity,
                    filter = new { value = "1" }
                };
            }
            else
            {
                parms = new
                {
                    output = "extend",
                    select_hosts = "extend",
                    monitored = "1",
                    only_true = "1",
                    sortfield = "lastchange",
                    expandDescription = "1",
                    maintenance = "0",
                    min_severity = minSeverity,
                    filter = new { value = "1" },
                    withLastEventUnacknowledged = "1"
                };
            }

            DateTime start = DateTime.Now;
            triggers.getWithParam(parms);
            elapsedTicks = DateTime.Now.Ticks - start.Ticks;

            Update(new UpdateInfoMessage(this) { message = "getTriggers(" + hostgroupID + ") = " + triggers.Count().ToString(), status = "DEBUG" });
            Update(new UpdateInfoMessage(this) { message = elapsedTicks.ToString(), status = "TRIGGERS" });
        }

        private void getHosts()
        {
            DateTime start = DateTime.Now;
            hosts.get();
            long elapsedTicks = DateTime.Now.Ticks - start.Ticks;
            Update(new UpdateInfoMessage(this) { message = elapsedTicks.ToString(), status = "HOSTS" });
        }

        private void getHostgroups()
        {
            DateTime start = DateTime.Now;
            hostgroups.get();
            long elapsedTicks = DateTime.Now.Ticks - start.Ticks;
            Update(new UpdateInfoMessage(this) { message = elapsedTicks.ToString(), status = "HOSTGROUPS" });
        }

        private string GetWebRequest(string body)
        {
            string responseFromServer = string.Empty;

            try
            {
                WebRequest wb = WebRequest.Create(_url);
                wb.ContentType = @"application/json-rpc";
                wb.Credentials = CredentialCache.DefaultCredentials;
                ASCIIEncoding encoding = new ASCIIEncoding();
                string postData = body;
                byte[] data = encoding.GetBytes(postData);
                wb.Method = "POST";
                wb.ContentLength = data.Length;
                Stream newStream = wb.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)wb.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Update(new UpdateInfoMessage(this) { message = ex.Message, status = "DEBUG" });
            }
            
            return responseFromServer;
        }
        private Stream GetWebFile(string url)
        {

            WebRequest wb = WebRequest.Create(url);
            wb.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)wb.GetResponse();
            Stream dataStream = response.GetResponseStream();
            return dataStream;
        }

        public string obj2json(object obj)
        {
            return serializer.Serialize(obj);
        }

        public string CallAPI(string method, object param)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            object Query = new
            {
                jsonrpc = "2.0",
                auth = authHash,
                id = id.ToString(),
                method = method,
                Params = param
            };
            String qr = obj2json(Query);
            qr = qr.Replace("Params", "params");
            id++;
            Alert("request: \n" + qr);
            string result = GetWebRequest(qr);
            Alert("response: \n" + result);
            return result; 
        }

        public void setHostgroupID(string id)
        {
            hostgroupID = id;
        }

        public void setMinSeverity(string sev)
        {
            minSeverity = sev;
        }

        public void setInterval(int seconds)
        {
            sleep = seconds * 1000;
        }

        public void setHideAck(int hide)
        {
            hideAck = hide;
        }
    }

    public class UpdateInfoMessage
    {
        public string status { get; set; }
        public string message { get; set; }
        public object sender { get; set; }
        public UpdateInfoMessage(object Sender, string Status = "", string Message = "")
        {
            status = Status;
            message = Message;
            sender = Sender;
        }
    }

    public class Result<T> : IEnumerable<T>
    {
        public T[] result;
        public ZabbixAPI server;
        protected string method;
        protected object Params;
        public string stringResult;
        public object SyncRoot;
        protected virtual void init() { }

        public Result(ZabbixAPI Server)
        {
            init();
            server = Server;
            SyncRoot = new object();
        }

        public Result()
        {
            init();
        }

        public virtual void get()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 16777216;
            server.Update(new UpdateInfoMessage(this) { message = "Sending request to server...", status = "INFO" });
            lock (SyncRoot)
            {
                stringResult = (server.CallAPI(method, Params));
                server.Update(new UpdateInfoMessage(this) { message = "Processing query result", status = "INFO" });
                try
                {
                    result = serializer.Deserialize<Result<T>>(stringResult).result;
                }
                catch (ArgumentException ex)
                {
                    server.Update(new UpdateInfoMessage(this) { message = "ArgumentException: " + ex.Message, status = "INFO" });
                }
                if (result == null) { result = new T[1]; }
                server.Update(new UpdateInfoMessage(this) { message = "Copying result to collection", status = "INFO" });
                server.Update(new UpdateInfoMessage(this) { message = "", status = "INFO" });
            }
        }

        public virtual void getWithParams(object p)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 16777216;
            server.Update(new UpdateInfoMessage(this) { message = "Sending request to server...", status = "INFO" });
            lock (SyncRoot)
            {
                stringResult = (server.CallAPI(method, p));
                server.Update(new UpdateInfoMessage(this) { message = "Processing query result", status = "INFO" });
                try
                {
                    result = serializer.Deserialize<Result<T>>(stringResult).result;
                }
                catch (ArgumentException ex)
                {
                    server.Update(new UpdateInfoMessage(this) { message = "ArgumentException: " + ex.Message, status = "INFO" });
                }
                if (result == null) { result = new T[1]; }
                server.Update(new UpdateInfoMessage(this) { message = "Copying result to collection", status = "INFO" });
                server.Update(new UpdateInfoMessage(this) { message = "", status = "INFO" });
            }
        }

        public T this[int index]
        {
            get
            {
                try
                {
                    return result[index];
                }
                catch (IndexOutOfRangeException)
                {
                    return default(T);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (result != null)
            {
                foreach (T item in result)
                {
                    yield return item;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class simpleresult
    {
        public string result;
    }

    public class Triggers : Result<Trigger>
    {
        protected override void init()
        {
            method = "trigger.get";
            Params = new
            {
                output = "extend",
                select_hosts = "extend",
                monitored = "1",
                templated = "0",
                only_true = "1",
                sortfield = "lastchange",
            };
        }
        public Triggers(ZabbixAPI Server) : base(Server) { }
        public override void get()
        {
            base.get();
            foreach (Trigger tr in result)
            {
                if (tr != null)
                {
                    tr.host = tr.hosts[0];
                }
            }
        }
        public void getWithParam(object p)
        {
            base.getWithParams(p);
            foreach (Trigger tr in result)
            {
                if (tr != null)
                {
                    tr.host = tr.hosts[0];
                }
            }
        }
        public List<Trigger> getByHostid(String hostid)
        {
            var q = from res in result
                    where res.host.hostid == hostid
                    select res;
            List<Trigger> trgs = new List<Trigger>(q);
            return trgs;
        }
        public void refresh()
        {
            get();
        }
    }

    public class Hosts : Result<Host>
    {
        protected override void init()
        {
            method = "host.get";
            Params = new { output = "extend" };
        }
        public void getByGroupID(string GroupID)
        {
            Params = new { output = "extend", groupids = GroupID };
            base.get();

        }
        public Hosts(ZabbixAPI Server) : base(Server) { }
    }

    public class Host
    {
        public string host;
        public string hostid;
        public string ip;
        public override string ToString()
        {
            return host;
        }
    }

    public class Trigger
    {
        public Host[] hosts { get; set; }
        public string triggerid { get; set; }
        public string expression { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string status { get; set; }
        public string value { get; set; }
        public string priority { get; set; }
        public string lastchange { get; set; }
        public DateTime lastchangeDateTime
        {
            get
            {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return origin.AddSeconds(double.Parse(lastchange));
            }
        }
        public string dep_level { get; set; }
        public string comments { get; set; }
        public string error { get; set; }
        public string templateid { get; set; }
        public string type { get; set; }
        public Host host { get; set; }
    }

    public class HostGroups:Result<HostGroup>
    {

        protected override void init()
        {
            method = "hostgroup.get";
            Params = new { output = "extend" };
        }
        public override void get()
        {
            base.get();
            foreach (HostGroup h in result)
            {
                if (h != null)
                {
                    h.hosts = new Hosts(server);
                    h.hosts.getByGroupID(h.groupid);
                }
            }
        }
        public HostGroups(ZabbixAPI Server) : base(Server) { }

    }
    public class HostGroup
    {
        public string groupid;
        public string name;
        public int inter;
        public override string ToString()
        {
            return name;
        }
        public string getID()
        {
            return groupid;
        }
        public Hosts hosts;
    }

    public class Events : Result<Event>
    {
        protected override void init()
        {
            method = "event.get";
            Params = new
            {
                output = "extend",
                sortfield = "clock",
                sortorder = "ASC",
                select_hosts = "shorten",
                select_triggers = "shorten",
                limit = "1000"

            };
        }

        public Events(ZabbixAPI Server) : base(Server) { }
    }

    public class Event
    {
    }
}
