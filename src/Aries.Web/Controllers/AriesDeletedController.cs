using Aries;

namespace Microsoft.AspNetCore.Mvc
{

    /// <summary>
    /// 删除路由，需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AriesDeletedController<T> : AriesQueryController<T> where T : class
    {

        public AriesDeletedController(IFreeSql freeSql):base(freeSql)
        {
           
        }


        /// <summary>
        /// 根据 Aries 操作模型删除实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("aries_delete")]
        public virtual ApiReturnResult DeleteByCondition([FromBody] SqlModel<T> model)
        {
            return Result(_freeSql.AriesDelete(model).ExecuteAffrows());
        }


        /// <summary>
        /// 根据实体主键删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("aries_deletebyid")]
        public virtual ApiReturnResult DeleteById(long id)
        {
            return BoolResult(_freeSql
                .Delete<T>()
                .WherePrimaryKey(id)
                .ExecuteAffrows() != 0);
        }

    }
}
