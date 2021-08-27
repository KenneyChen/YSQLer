using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace YSQLer.Core
{
    public class YSQLerIocContainer
    {
        public static SQLBuilder CreateInsertBuilder(HttpContext httpContext, RouteData routeData)
        {
            SQLBuilder builder = null;

            switch (YSQLerAppSettings.GetDbType())
            {
                case DbType.MSSQL:
                    break;
                default:
                    builder = new MySQLInsertBuilder()
                    {
                        HttpContext = httpContext,
                        RouteData = routeData,
                    };
                    break;
            }

            return builder;
        }

        public static SQLBuilder CreateUpdateBuilder(HttpContext httpContext, RouteData routeData)
        {
            SQLBuilder builder = null;

            switch (YSQLerAppSettings.GetDbType())
            {
                case DbType.MSSQL:
                    break;
                default:
                    builder = new MySQLUpdateBuilder()
                    {
                        HttpContext = httpContext,
                        RouteData = routeData,
                    };
                    break;
            }

            return builder;
        }


        public static SQLBuilder CreateDeleteBuilder(HttpContext httpContext, RouteData routeData)
        {
            SQLBuilder builder = null;

            switch (YSQLerAppSettings.GetDbType())
            {
                case DbType.MSSQL:
                    break;
                default:
                    builder = new MySQLDeleteBuilder()
                    {
                        HttpContext = httpContext,
                        RouteData = routeData,
                    };
                    break;
            }

            return builder;
        }


        public static SQLQueryBuilderBase CreateQueryBuilder(HttpContext httpContext, RouteData routeData)
        {
            SQLQueryBuilderBase builder = null;

            switch (YSQLerAppSettings.GetDbType())
            {
                case DbType.Mysql:
                    builder = new MySQLQueryBuilder()
                    {
                        HttpContext = httpContext,
                        RouteData = routeData,
                    };
                    break;
                case DbType.MSSQL:
                    break;
                default:
                    builder = new MySQLQueryBuilder()
                    {
                        HttpContext = httpContext,
                        RouteData = routeData,
                    };
                    break;
            }

            return builder;
        }
    }
}
