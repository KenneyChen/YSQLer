using System;
using System.Collections.Generic;
using System.Text;

namespace YSQLer.Core.Model
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class QueryInfo
    {
        public QueryInfoBody Query { get; set; }
    }

    public class QueryInfoBody
    {
        public string fileds { get; set; }
    }
}
