using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YSQLer.Core
{
    public static class YSQLerAppSettings
    {
        public static IConfiguration Configuration;

        public static string GetConnectionString()
        {
            var connection = Configuration.GetConnectionString("YSQLerConnection");
            var dbType = Configuration.GetConnectionString("YSQLerDbType");
            if (string.IsNullOrEmpty(connection))
            {
                throw new Exception("请在appsettings.json配置数据库连接字符串 " +
                    "'ConnectionStrings':{'YSQLerConnection':'xxxxx'} " +
                    "节点");
            }

            return connection;
        }

        public static DbType GetDbType()
        {
            var value = Configuration.GetConnectionString("YSQLerDbType");
            if (value==null)
            {
                return DbType.Mysql;
            }
            return (DbType)Enum.Parse(typeof(DbType), value);
        }
    }
}
