using System;
using System.Collections.Generic;
using System.Text;

namespace YSQLer.Core
{
    internal static class Contansts
    {
        public static Dictionary<string, string> TablePrimaryKey = new Dictionary<string, string>();

        public static void Add(string table,string primaryKey)
        {
            if (primaryKey==null)
            {
                return;
            }
            if (!TablePrimaryKey.ContainsKey(table))
            {
                 TablePrimaryKey.Add(table, primaryKey);
            }
        }

        public static string Get(string table)
        {
            if (TablePrimaryKey.ContainsKey(table))
            {
                return TablePrimaryKey[table];
            }
            return null;
        }
    }
}
