using Aries;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// 查询路由 需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AriesQueryController<T> : ResultController where T : class
    {

        protected readonly IFreeSql _freeSql;
        public AriesQueryController(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }


        /// <summary>
        /// 根据 Aries 操作模型 进行列表查询
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost("aries_query_list")]
        public virtual ApiReturnPageResult QueryList([FromBody] SqlModel<T> queryModel)
        {
            return Result(_freeSql.AriesQuery(queryModel, out var total).ToLimitList(), total);
        }


        /// <summary>
        /// 根据 Aries 操作模型 进行单个实体查询
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost("aries_query_single")]
        public virtual ApiReturnResult QuerySingle([FromBody] SqlModel<T> queryModel)
        {
            return Result(_freeSql.AriesQuery(queryModel, out _).ToLimitFirst());
        }


        /// <summary>
        /// 根据 Aries 操作模型 进行数量查询
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost("aries_query_count")]
        public virtual ApiReturnResult QueryCount([FromBody] SqlModel<T> queryModel)
        {
            return Result(_freeSql.AriesQuery(queryModel, out var total).Count());
        }

    }
}
