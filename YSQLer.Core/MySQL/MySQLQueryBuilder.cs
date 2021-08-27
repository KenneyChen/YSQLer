using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.Json;

namespace YSQLer.Core
{
    internal class MySQLQueryBuilder : SQLQueryBuilderBase
    {
        public List<string> Fileds()
        {
            var doc = JsonDocument.Parse(this.Json());
            var enumerate = doc.RootElement
                .GetProperty("query")
                .GetProperty("fileds")
                .EnumerateObject();

            var fileds = new List<string>();
            while (enumerate.MoveNext())
            {
                var cur = enumerate.Current;
                fileds.Add(cur.Name);
                this.Paramters.Add(new SqlParameter(cur.Name, cur.Value));
            }
            return fileds;
        }

        public override string ToSql()
        {
            return $"select {string.Join(",", this.Fileds())} " +
                $"from {this.Table()} " +
                $"where {this.Where("query")} " +
                $"limit {this.Page()},{this.Size()}";
        }
    }
}
