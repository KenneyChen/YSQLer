using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace YSQLer.Core
{
    public class SqlHelper
    {
        public static string ConnectionString
        {
            get { return YSQLerAppSettings.GetConnectionString(); }
        }

        public static IEnumerable<dynamic> QueryDynamic(string sql, object paramter = null)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                return SqlMapper.Query(connection, sql, paramter);
            }
        }

        public static IEnumerable<T> Query<T>(string sql, object paramter = null)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                return SqlMapper.Query<T>(connection, sql, paramter);
            }
        }

        public static DataTable Query(string sql, object paramter)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                using (var reader = SqlMapper.ExecuteReader(connection, sql, paramter))
                {
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public static int Excute(string sql, object paramter)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                return SqlMapper.Execute(connection, sql, paramter);
            }
        }

        public static object ExecuteScalar(string sql, object paramter)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                return SqlMapper.ExecuteScalar(connection, sql, paramter);
            }
        }
    }
}
