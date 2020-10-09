using Aries;

namespace Microsoft.AspNetCore.Mvc
{

    /// <summary>
    /// 新增路由 需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AriesAddAndQueryController<T> : AriesQueryController<T> where T : class
    {

        public AriesAddAndQueryController(IFreeSql freeSql):base(freeSql)
        {
           
        }

        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>

        [HttpPost("aries_add")]
        public virtual ApiReturnResult Add(T instance)
        {
            return Result(_freeSql.AriesInsert(instance));
        }

    }
}
