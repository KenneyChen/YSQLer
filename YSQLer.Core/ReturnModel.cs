using System;
using System.Collections.Generic;
using System.Text;

namespace YSQLer.Core
{
    internal class ReturnModel
    {

        public bool success { get; set; }

        public string msg { get; set; }

        public static ReturnModel Init(int r)
        {
            return new ReturnModel
            {
                success = r > 0,
                msg = r > 0 ? "请求成功" : "请求失败",
            };
        }
    }

    internal class PageObject
    {
        /// <summary>
        /// 当前总数
        /// </summary>
        public object TotalCount { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public Object Records { get; set; }
    }

    internal class ReturnModel<T> : ReturnModel
    {
        public T data { get; set; }

    }
}
