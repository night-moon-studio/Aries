using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aries.Web.Controllers
{

    public class ModifyController<T> : QueryController<T> where T : class
    {

        public ModifyController(IFreeSql freeSql):base(freeSql)
        {
           
        }


        /// <summary>
        /// 整体扫描的增量更新
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [HttpPost("modifybyquery")]
        public ApiReturnResult ModifyAll(T instance, [FromQuery] T entity, [FromBody] QueryModel query)
        {

            return Result(_freeSql.UpdateAll(instance)
                .QueryWithHttpEntity(Request.Query.Keys,entity)
                .QueryWithModel(query)
                .ExecuteAffrows());

        }


        [HttpPost("modifybyid")]
        public ApiReturnResult Modify([FromQuery] T instance)
        {
            return BoolResult(_freeSql
                .UpdateWithHttpModel(Request.Query.Keys, instance)
                .WherePrimaryKeyFromEntity(instance)
                .ExecuteAffrows() != 0);
        }


        [HttpPost("delete")]
        public ApiReturnResult Modify([FromQuery] T instance)
        {
            return BoolResult(_freeSql
                .UpdateWithHttpModel(Request.Query.Keys, instance)
                .WherePrimaryKeyFromEntity(instance)
                .ExecuteAffrows() != 0);
        }
    }
}
