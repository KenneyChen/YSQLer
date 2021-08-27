using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace YSQLer.Core
{
    public class MySQLDeleteBuilder : SQLBuilder
    {
        public override string ToSql()
        {
            return $"delete from {this.Table()} " +
                $"where {this.Where("delete")}";
        }
    }
}
