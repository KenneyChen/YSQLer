using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

namespace YSQLer.Core
{
    internal static class DbFactory
    {
        public static IDbConnection GetConnection()
        {
            var connectionString=YSQLerAppSettings.GetConnectionString();
            var type = YSQLerAppSettings.GetDbType();
            switch (type)
            {
                case DbType.MySQL:
                    return new MySqlConnection(connectionString);
                case DbType.MSSQL:
                    return new SqlConnection(connectionString);
                default:
                    //默认mssql
                    return new SqlConnection(connectionString);
            }
        }
    }
    public enum DbType
    {
        MySQL = 2,
        MSSQL = 1,
    }
}
