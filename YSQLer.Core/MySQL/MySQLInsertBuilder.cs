using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace YSQLer.Core
{
    public class MySQLInsertBuilder : SQLQueryBuilderBase, ISQLBuilder
    {
        public List<string> Fileds()
        {
            return null;
        }

        public string ToSql()
        {
            var colums = string.Join(",", this.Fileds());
            var values = string.Join(",", this.Fileds().Select(f=>"@"+f).ToList());
            return $"insert into{this.Table()}(colums) values ({values})";
        }
    }
}
