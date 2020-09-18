using Aries;

namespace Microsoft.AspNetCore.Mvc
{

    /// <summary>
    /// 删除路由，需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AriesDeletedController<T> : AriesModifyController<T> where T : class
    {

        public AriesDeletedController(IFreeSql freeSql):base(freeSql)
        {
           
        }


        [HttpPost("aries_delete")]
        public ApiReturnResult DeleteByCondition([FromBody] SqlModel<T> model)
        {
            return Result(_freeSql.DeleteFromSqlModel(model).ExecuteAffrows());
        }

        [HttpPost("aries_deletebyid")]
        public ApiReturnResult DeleteById([FromQuery] T entity)
        {
            return BoolResult(_freeSql
                .Delete<T>()
                .WherePrimaryKeyFromEntity(entity)
                .ExecuteAffrows() != 0);
        }

    }
}
