
using System;
using System.Collections.Generic;
using System.Text;

namespace YSQLer.Core
{
    internal abstract class SQLQueryBuilderBase : SQLBuilder
    {
        public string ToCountSql()
        {
            return $"select count(*) from {this.Table()} where {this.Where("query")} ";
        }
    }
}
