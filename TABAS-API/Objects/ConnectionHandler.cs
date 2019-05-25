using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TABAS_API.Objects
{
    public class ConnectionHandler
    {
        //private static readonly string sql_str = "Data Source=.;Initial Catalog=UsersDB;User ID=MarceloLogin;Password=bases152";
        //private static readonly string npgsql_str = "Server=127.0.0.1;Port=5432;UserId=postgres;Password=marcelopass114;Database=TABAS-DB;";
        private static readonly string sql_str = "Data Source=tabas-sql.database.windows.net;" +
       "Initial Catalog=TABAS_SQL;User ID=Joseda8;Password=jo_se_da_8";
        private static readonly string npgsql_str = "Server=tabas.postgres.database.azure.com; " +
               "Port=5432; User Id=Joseda8@tabas; Password=jo_se_da_8; Database=TABAS; SslMode=Require";
        private static SqlConnection sql_conn = new SqlConnection(sql_str);
        private static NpgsqlConnection npgsql_conn = new NpgsqlConnection(npgsql_str); 

        public static SqlConnection GetSSMSConnection()
        {
            return sql_conn;
        }

        public static NpgsqlConnection GetPGConnection()
        {
            return npgsql_conn;
        }
    }
}