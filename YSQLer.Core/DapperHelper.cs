using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace YSQLer.Core
{
    internal class DapperHelper
    {
        
        public static IEnumerable<dynamic> QueryDynamic(string sql, object paramter = null)
        {
            using (IDbConnection connection = DbFactory.GetConnection())
            {
                return SqlMapper.Query(connection, sql, paramter);
            }
        }

        public static IEnumerable<T> Query<T>(string sql, object paramter = null)
        {
            using (IDbConnection connection = DbFactory.GetConnection())
            {
                return SqlMapper.Query<T>(connection, sql, paramter);
            }
        }

        public static DataTable Query(string sql, object paramter)
        {
            using (IDbConnection connection = DbFactory.GetConnection())
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
            using (IDbConnection connection = DbFactory.GetConnection())
            {
                return SqlMapper.Execute(connection, sql, paramter);
            }
        }

        public static object ExecuteScalar(string sql,object paramter)
        {
            using (IDbConnection connection = DbFactory.GetConnection())
            {
                return SqlMapper.ExecuteScalar(connection, sql, paramter);
            }
        }
    }
}
