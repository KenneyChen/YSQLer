using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSQLer.Core;

namespace YSQLer.Controllers
{
    [ApiController]
    [Route("{controller}/{table}")]
    [TypeFilter(typeof(PermissionActionFilterAttribute))]
    public class YSQLerController : ControllerBase
    {
        [Route("{action}/{id}")]
        [HttpGet]
        public object Query()
        {
            return YSQLerSDK.Query(HttpContext, RouteData);
        }

        [Route("{action}/{id?}")]
        [HttpPost]
        public ReturnModel<PageObject> Query(int id)
        {
            return YSQLerSDK.Query(HttpContext, RouteData);
        }

        [Route("{action}/{id?}")]
        [HttpPost]
        public object Update()
        {
            return YSQLerSDK.Update(HttpContext, RouteData);
        }

        [Route("{action}")]
        [HttpPost]
        public object Add()
        {
            return YSQLerSDK.Insert(HttpContext, RouteData);
        }

        [Route("{action}/{id?}")]
        [HttpPost]
        public object Delete()
        {
            return YSQLerSDK.Delete(HttpContext, RouteData);
        }
    }

    public class PermissionActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //表权限
            var table = context.RouteData.Values["table"];
            if (table==null)
            {
                throw new Exception("未传入数据表");
            }
            if (!YSQLerAppSettings.AllowTables().Any(f=>f.Equals(table.ToString(),StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new Exception("请在appsettings 节点YSQLerAllowTables配置支持哪些表允许使用此sdk");
            }
            //表操作权限
            var action= context.RouteData.Values["action"];
            if (action == null)
            {
                throw new Exception("获取操作权限为空，请检测路由是否有问题");
            }
            if (!YSQLerAppSettings.AllowOperation().Any(f => f.Equals(action.ToString(), StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new Exception("请在appsettings 节点YSQLerAllowOperation配置支持哪些操作[query,insert,update,delete]允许使用此sdk");
            }
        }
    }
}
