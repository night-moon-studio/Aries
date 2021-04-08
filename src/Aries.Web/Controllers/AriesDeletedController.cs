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
        /// <param name="model">Aries 操作模型</param>
        /// <returns></returns>
        [HttpPost("aries_delete")]
        public virtual AriesJsonResult DeleteByCondition([FromBody] SqlModel<T> model)
        {
            return Result(_freeSql.AriesDelete(model).ExecuteAffrows());
        }


        /// <summary>
        /// 根据实体主键删除实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        [HttpGet("aries_deletebyid")]
        public virtual AriesJsonResult DeleteById(long id)
        {
            return BoolResult(_freeSql.AriesDeleteByPrimaryKey<T>(id));
        }

    }
}
