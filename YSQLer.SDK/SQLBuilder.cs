using Dapper;
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
    internal abstract class SQLBuilder
    {
        public HttpContext HttpContext { get; set; }
        public RouteData RouteData { get; set; }

        internal DynamicParameters Paramters { get; set; }

        private string json;

        public SQLBuilder()
        {
            this.Paramters = new DynamicParameters();
        }

        public string Json()
        {
            if (json!=null)
            {
                return json;
            }
            if (HttpContext.Request.ContentLength == 0 || HttpContext.Request.ContentLength == null)
            {
                return null;
            }

            //操作Request.Body之前加上EnableBuffering即可
            HttpContext.Request.EnableBuffering();
            StreamReader stream = new StreamReader(HttpContext.Request.Body);
            string body = stream.ReadToEndAsync().GetAwaiter().GetResult();
            //用完了我们尽量也重置一下，自己的坑自己填
            HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);

            return json = body;
        }

        /// <summary>
        /// 是否分页
        /// </summary>
        public bool Paging
        {
            get
            {
                return HttpContext.Request.Query.ContainsKey("offset");
            }
        }

        public int Page()
        {
            if (HttpContext.Request.Query.ContainsKey("offset"))
            {
                var offset = HttpContext.Request.Query["offset"].ToString();
                return Convert.ToInt32(offset);
            }

            return 1;
        }

        public int Size()
        {
            if (HttpContext.Request.Query.ContainsKey("limit"))
            {
                var limit = HttpContext.Request.Query["limit"].ToString();
                return Convert.ToInt32(limit);
            }
            return 999;
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
                var dt = DapperHelper.Query($"desc {r};", null);
                var primary = dt.Select("key='PRI'").ToList();
                if (primary.Count == 0)
                {
                    throw new Exception($"此表无主键:{r}，请使用filter节点处理");
                }
                Contansts.Add(r, primary[0]["Field"].ToString());
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
                if (!this.Paramters.ParameterNames.Any(f => f== keyColumName))
                {
                    this.Paramters.Add(keyColumName, routeData);
                }
                return $"{keyColumName}=@{keyColumName}";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(this.Json()))
                {
                    throw new Exception("找不到filter条件");
                }
                var doc = JsonDocument.Parse(this.Json());
                var enumerate = doc.RootElement
                    .GetProperty(root)
                    .GetProperty("filter")
                    .EnumerateObject();

                var fileds = new List<string>();
                while (enumerate.MoveNext())
                {
                    var cur = enumerate.Current;
                    fileds.Add(cur.Name+"=@"+cur.Name);
                    if (!this.Paramters.ParameterNames.Any(f => f == cur.Name))
                    {
                        this.Paramters.Add(cur.Name, cur.Value.ToString());
                    }
                }
                if (fileds.Count == 0)
                {
                    throw new Exception("获取filter条件为空");
                }
                return string.Join(" and ", fileds);
            }
        }

        public abstract String ToSql();
    }
}
