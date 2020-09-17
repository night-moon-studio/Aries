using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aries.Web.Controllers
{

    public class QueryController<T> : ResultController where T : class
    {

        protected readonly IFreeSql _freeSql;
        public QueryController(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }


        [HttpPost("query")]
        public ApiReturnPageResult Query([FromQuery] T instance, [FromBody] QueryModel query)
        {
            return Result(_freeSql
                .Select<T>()
                .QueryWithHttpEntity(Request.Query.Keys, instance)
                .QueryWithModel(query,out long totle)
                .ToLimitList(), totle);
        }

        [HttpPost("modify")]
        public ApiReturnPageResult Modify([FromQuery] T instance, [FromBody] QueryModel query)
        {
            return Result(_freeSql
                .Select<T>()
                .QueryWithHttpEntity(Request.Query.Keys, instance)
                .QueryWithModel(query, out long totle)
                .ToLimitList(), totle);
        }

    }
}
