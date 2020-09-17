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
    /// 更新路由 需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModifyController<T> : QueryController<T> where T : class
    {

        public ModifyController(IFreeSql freeSql):base(freeSql)
        {
           
        }


        /// <summary>
        /// 更新整个实体，实体再 query 参数中 Instance
        /// </summary>
        /// <param name="queryEntity">查询实体</param>
        /// <param name="queryModel">查询模型</param>
        /// <returns></returns>
        [HttpPost("modifybyquery")]
        public ApiReturnResult ModifyByCondition([FromBody] SqlModel<T> queryModel)
        {

            return Result(_freeSql.UpdateWithHttpModel(queryModel.ModifyInstance.Fields,queryModel.ModifyInstance.Instance)
                .QueryWithHttpEntity(queryModel.QueryInstance.Fields, queryModel.QueryInstance.Instance)
                .QueryWithModel(queryModel)
                .ExecuteAffrows());

        }

    }
}
