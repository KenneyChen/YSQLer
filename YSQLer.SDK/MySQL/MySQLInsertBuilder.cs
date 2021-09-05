using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace YSQLer.Core
{
    internal class MySQLInsertBuilder : SQLQueryBuilderBase
    {
        public List<string> Fileds()
        {
            var doc = JsonDocument.Parse(this.Json());
            var enumerate = doc.RootElement
                .GetProperty("add")
                .EnumerateObject();

            var fileds = new List<string>();
            while (enumerate.MoveNext())
            {
                var cur = enumerate.Current;
                fileds.Add(cur.Name);
                this.Paramters.Add(cur.Name, cur.Value.ToString());
            }
            return fileds;
        }

        public override string ToSql()
        {
            var colums = string.Join(",", this.Fileds());
            var values = string.Join(",", this.Fileds().Select(f=>"@"+f).ToList());
            return $"insert into {this.Table()}({colums}) values ({values});SELECT LAST_INSERT_ID()";
        }
    }
}
