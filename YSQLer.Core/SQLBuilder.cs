using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace YSQLer.Core
{
    public abstract class SQLBuilder
    {
        public HttpContext HttpContext { get; set; }
        public RouteData RouteData { get; set; }

        internal List<SqlParameter> Paramters { get; }

        public SQLBuilder()
        {
            this.Paramters = new List<SqlParameter>();
        }

        public string Json()
        {
            using (var st = HttpContext.Request.Body)
            {
                var bytes = new byte[st.Length];
                st.Read(bytes, 0, (int)st.Length);
                return Encoding.UTF8.GetString(bytes);
            }
        }

        public int Page()
        {
            if (HttpContext.Request.Query.ContainsKey("offset"))
            {
                var offset = HttpContext.Request.Query["offset"].ToString();
                return Convert.ToInt32(offset);
            }

            return 0;
        }

        public int Size()
        {
            if (HttpContext.Request.Query.ContainsKey("limit"))
            {
                var limit = HttpContext.Request.Query["limit"].ToString();
                return Convert.ToInt32(limit);
            }
            return 0;
        }

        public string Table()
        {
            var table = RouteData.Values["table"];
            if (table == null || string.IsNullOrEmpty(table.ToString()))
            {
                throw new Exception("获取表名为空，请检查");
            }
            var r = table.ToString();

            //如果传入主键查询
            if (GetKeyValue() != null)
            {
                var dt = SqlHelper.Query($"desc {r};", null);
                var primary = dt.PrimaryKey;
                if (primary.Length == 0)
                {
                    throw new Exception($"此表无主键:{r}，请使用filter节点处理");
                }
                Contansts.Add(r, primary.ToList().Where(f=>f.Unique==true).Select(f=>f.ColumnName).FirstOrDefault());
            }
           
            return r;
        }

        public object GetKeyValue()
        {
            return RouteData.Values["id"];
        }

        public string Where(string root)
        {
            var routeData = GetKeyValue();
            if (routeData != null)
            {
                //$key$ 特殊标记，代表主键
                var keyColumName = $"{Contansts.Get(Table())}";
                this.Paramters.Add(new SqlParameter(keyColumName, routeData));
                return $"{keyColumName}=@{keyColumName}";
            }
            else
            {
                var doc = JsonDocument.Parse(this.Json());
                var enumerate = doc.RootElement
                    .GetProperty(root)
                    .GetProperty("filter")
                    .EnumerateObject();

                var fileds = new List<string>();
                while (enumerate.MoveNext())
                {
                    var cur = enumerate.Current;
                    fileds.Add(cur.Name);
                    this.Paramters.Add(new SqlParameter(cur.Name, cur.Value));
                }
                if (fileds.Count == 0)
                {
                    return "1=1";
                }
                return string.Join(" and ", fileds);
            }
        }

        public abstract String ToSql();
    }
}
