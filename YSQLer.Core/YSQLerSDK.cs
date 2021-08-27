using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Data;

namespace YSQLer.Core
{
    /// <summary>
    /// 对外暴露接口
    /// </summary>
    public class YSQLerSDK
    {
        public object Query(HttpContext httpContext, RouteData routeData)
        {
            var builder = YSQLerIocContainer.CreateQueryBuilder(httpContext, routeData);
            var sql = builder.ToSql();
            var countSql = builder.ToCountSql();
            var paramter = builder.Paramters;
            var result = new List<Dictionary<string, object>>();
            var count = SqlHelper.ExecuteScalar(countSql, paramter);
            if (count != null && Convert.ToInt32(count) > 0)
            {
                var dt = SqlHelper.Query(sql, paramter);
                foreach (DataRow item in dt.Rows)
                {
                    var dic = new Dictionary<string, object>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        dic.Add(column.ColumnName, item[column.ColumnName] == System.DBNull.Value
                            ? null
                            : item[column.ColumnName]);
                    }
                    result.Add(dic);
                }
            }

            return new ReturnModel<PageObject>
            {
                code = 200,
                msg = "请求成功",
                data = new PageObject()
                {
                    Records = result,
                    TotalCount = count,
                }
            };
        }

        public object Update(HttpContext httpContext, RouteData routeData)
        {
            var builder = YSQLerIocContainer.CreateUpdateBuilder(httpContext, routeData);
            var sql = builder.ToSql();
            var paramter = builder.Paramters;
            var r = SqlHelper.Excute(sql, paramter);             
            return ReturnModel.Init(r);
        }

        public object Delete(HttpContext httpContext, RouteData routeData)
        {
            var builder = YSQLerIocContainer.CreateDeleteBuilder(httpContext, routeData);
            var sql = builder.ToSql();
            var paramter = builder.Paramters;
            var r = SqlHelper.Excute(sql, paramter);
            return ReturnModel.Init(r);
        }

        public object Insert(HttpContext httpContext, RouteData routeData)
        {
            var builder = YSQLerIocContainer.CreateInsertBuilder(httpContext, routeData);
            var sql = builder.ToSql();
            var paramter = builder.Paramters;
            var r = SqlHelper.Excute(sql, paramter);
            return ReturnModel.Init(r);
        }
    }
}
