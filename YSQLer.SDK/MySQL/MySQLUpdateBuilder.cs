using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace YSQLer.Core
{
    internal class MySQLUpdateBuilder : SQLQueryBuilderBase
    {
        public List<string> Fileds()
        {
            var doc = JsonDocument.Parse(this.Json());
            var enumerate = doc.RootElement
                .GetProperty("update")
                .EnumerateObject();

            var fileds = new List<string>();
            while (enumerate.MoveNext())
            {
                var cur = enumerate.Current;
                if (cur.Name=="filter")
                {
                    continue;
                }
                fileds.Add(cur.Name);
                this.Paramters.Add(cur.Name, cur.Value.ToString());
            }
            return fileds;
        }

        public override string ToSql()
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
