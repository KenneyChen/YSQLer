using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YSQLer.Core
{
    public static class YSQLerAppSettings
    {
        public static IConfiguration Configuration;

        internal static string GetConnectionString()
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

        internal static DbType GetDbType()
        {
            var value = Configuration.GetConnectionString("YSQLerDbType");
            if (value == null)
            {
                return DbType.MySQL;
            }
            return (DbType)Enum.Parse(typeof(DbType), value);
        }

        /// <summary>
        /// 获取操作权限
        /// </summary>
        /// <returns></returns>
        public static List<string> AllowOperation()
        {
            var r = Configuration.GetConnectionString("YSQLerAllowOperation");
            if (string.IsNullOrWhiteSpace(r))
            {
                //只允许查询
                return new List<string> { "query" };
            }
            if (r=="*")
            {
                return new List<string> { "query", "update", "delete", "insert" };
            }

            return r.Split(',').ToList();
        }

        /// <summary>
        /// 获取操作哪些表
        /// </summary>
        /// <returns></returns>
        public static List<string> AllowTables()
        {
            var r = Configuration.GetConnectionString("YSQLerAllowTables");
            if (string.IsNullOrWhiteSpace(r))
            {
                //只允许查询
                throw new Exception("请在节点YSQLerAllowTables配置允许哪些表使用此SDK");
            }
            if (r == "*")
            {
                return new List<string> { "*" };
            }
            return r.Split(',').ToList();
        }
    }
}
