using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebBase.DapperHelper
{
    public class ConnectionOptions
    {
        private static IDbConnection _dbConnection = new SqlConnection();

        public static IDbConnection DbConnection
        {
            get
            {
                if (string.IsNullOrEmpty(_dbConnection.ConnectionString))
                { _dbConnection.ConnectionString = ConnectionString; }
                return _dbConnection;
            }
        }

        private static string _connectionString;

        public static string ConnectionString { get { return _connectionString; } set { _connectionString = value; } }
    }
}

