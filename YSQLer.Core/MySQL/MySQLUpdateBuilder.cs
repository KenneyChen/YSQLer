using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace YSQLer.Core
{
    public class MySQLUpdateBuilder : SQLQueryBuilderBase, ISQLBuilder
    {
        public List<string> Fileds()
        {
            return null;
        }

        public string ToSql()
        {
            var setFileds = this.Fileds()
                .Select(f => f + "=@" + f)
                .ToList();

            return $"update {this.Table()} " +
                $"set {string.Join(",", setFileds)} " +
                $"where {this.Where("update")}";
        }
    }
}
