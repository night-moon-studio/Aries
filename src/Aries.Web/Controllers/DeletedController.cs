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
    /// 删除路由，需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DeletedController<T> : ModifyController<T> where T : class
    {

        public DeletedController(IFreeSql freeSql):base(freeSql)
        {
           
        }


        [HttpPost("delete")]
        public ApiReturnResult DeleteByCondition([FromQuery] T entity, [FromBody] QueryModel query)
        {
            return Result(_freeSql
                .Delete<T>().QueryWithHttpEntity(Request.Query.Keys, entity)
                .QueryWithModel(query)
                .ExecuteAffrows());
        }

        [HttpPost("deletebyid")]
        public ApiReturnResult DeleteById([FromQuery] T entity)
        {
            return BoolResult(_freeSql
                .Delete<T>()
                .WherePrimaryKeyFromEntity(entity)
                .ExecuteAffrows() != 0);
        }

    }
}
