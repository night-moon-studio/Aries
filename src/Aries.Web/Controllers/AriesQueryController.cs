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


        [HttpPost("query")]
        public ApiReturnPageResult Query([FromBody] SqlModel<T> query)
        {
            return Result(_freeSql.QueryFromSqlModel(query,out var total).ToLimitList(), total);
        }

    }
}
