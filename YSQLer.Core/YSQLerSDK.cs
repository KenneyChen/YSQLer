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
    public static class YSQLerSDK
    {
        public static ReturnModel<PageObject> Query(HttpContext httpContext, RouteData routeData)
        {
            var builder = YSQLerIocContainer.CreateQueryBuilder(httpContext, routeData);
            var sql = builder.ToSql();
            var countSql = builder.ToCountSql();
            var paramter = builder.Paramters;
            var result = new List<Dictionary<string, object>>();
            var count = DapperHelper.ExecuteScalar(countSql, paramter);
            if (count != null && Convert.ToInt32(count) > 0)
            {
                var dt = DapperHelper.Query(sql, paramter);
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
                msg = "请求成功",
                data = new PageObject()
                {
                    Records = result,
                    TotalCount = count,
                }
            };
        }

        public static ReturnModel Update(HttpContext httpContext, RouteData routeData)
        {
            var builder = YSQLerIocContainer.CreateUpdateBuilder(httpContext, routeData);
            var sql = builder.ToSql();
            var paramter = builder.Paramters;
            var r = DapperHelper.Excute(sql, paramter);             
            return ReturnModel.Init(r);
        }

        public static ReturnModel Delete(HttpContext httpContext, RouteData routeData)
        {
            var builder = YSQLerIocContainer.CreateDeleteBuilder(httpContext, routeData);
            var sql = builder.ToSql();
            var paramter = builder.Paramters;
            var r = DapperHelper.Excute(sql, paramter);
            return ReturnModel.Init(r);
        }

        public static ReturnModel Insert(HttpContext httpContext, RouteData routeData)
        {
            var builder = YSQLerIocContainer.CreateInsertBuilder(httpContext, routeData);
            var sql = builder.ToSql();
            var paramter = builder.Paramters;
            var r = DapperHelper.Excute(sql, paramter);
            return ReturnModel.Init(r);
        }
    }
}
