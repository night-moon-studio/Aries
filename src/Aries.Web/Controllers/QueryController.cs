using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aries.Web.Controllers
{
    /// <summary>
    /// 查询路由 需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryController<T> : ResultController where T : class
    {

        protected readonly IFreeSql _freeSql;
        public QueryController(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }


        [HttpPost("query")]
        public ApiReturnPageResult Query([FromBody] SqlModel<T> query)
        {
            return Result(_freeSql
                .Select<T>()
                .QueryWithHttpEntity(query.QueryInstance.Fields, query.QueryInstance.Instance)
                .QueryWithModel(query,out long totle)
                .ToLimitList(), totle);
        }

    }
}
