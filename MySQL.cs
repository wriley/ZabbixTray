using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

namespace ZabbixTray
{
    class MySQL
    {

        private OdbcConnection connection;

        public MySQL(string strConn)
        {
           connection = new OdbcConnection(strConn);          
        }

        public OdbcConnection Connection
        {
            get
            {
                return connection;
            }
        }
    }
}
