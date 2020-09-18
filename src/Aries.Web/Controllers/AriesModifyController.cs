using Aries;

namespace Microsoft.AspNetCore.Mvc
{

    /// <summary>
    /// 更新路由 需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AriesModifyController<T> : AriesQueryController<T> where T : class
    {

        public AriesModifyController(IFreeSql freeSql):base(freeSql)
        {
           
        }


        /// <summary>
        /// 更新整个实体，实体再 query 参数中 Instance
        /// </summary>
        /// <param name="queryEntity">查询实体</param>
        /// <param name="queryModel">查询模型</param>
        /// <returns></returns>
        [HttpPost("aries_modify")]
        public ApiReturnResult ModifyByCondition([FromBody] SqlModel<T> queryModel)
        {

            return Result(_freeSql.ModifyFromSqlModel(queryModel).ExecuteAffrows());

        }

    }
}
