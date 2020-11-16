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
        /// 根据 Aries 操作模型更新实体
        /// </summary>
        /// <param name="queryEntity">查询实体</param>
        /// <param name="queryModel">查询模型</param>
        /// <returns></returns>
        [HttpPost("aries_modify")]
        public virtual ApiReturnResult Modify([FromBody] SqlModel<T> opModel)
        {
            var result = _freeSql.AriesModify(opModel).ExecuteAffrows();
            if (result>0)
            {
                return Result(result);
            }
            return BoolResult(false);

        }

    }
}
