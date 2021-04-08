using Aries;

namespace Microsoft.AspNetCore.Mvc
{

    /// <summary>
    /// 更新路由 需要被继承
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class AriesModifyController<T> : AriesQueryController<T> where T : class
    {

        public AriesModifyController(IFreeSql freeSql):base(freeSql)
        {
           
        }


        /// <summary>
        /// 根据 Aries 操作模型更新实体
        /// </summary>
        /// <param name="opModel">Aries 操作模型</param>
        /// <returns></returns>
        [HttpPost("aries_modify")]
        public virtual AriesJsonResult Modify([FromBody] SqlModel<T> opModel)
        {
            var result = _freeSql.AriesModify(opModel).ExecuteAffrows();
            if (result>0)
            {
                return Result(result);
            }
            return BoolResult(false);

        }


        /// <summary>
        /// 根据实体中的主键更新整个实体
        /// </summary>
        /// <param name="model">要被更新的实体</param>
        /// <returns></returns>
        [HttpPost("aries_modify_all")]
        public virtual AriesJsonResult Modify(T model)
        {
            var result = _freeSql.UpdateAll(model).WhereByPrimaryKeyFromEntity(model).ExecuteAffrows();
            if (result > 0)
            {
                return Result(result);
            }
            return BoolResult(false);

        }
    }
}
