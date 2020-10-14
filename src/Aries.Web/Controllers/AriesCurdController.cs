using Aries;

namespace Microsoft.AspNetCore.Mvc
{

    /// <summary>
    /// CURD路由
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AriesCurdController<T> : AriesDeletedController<T> where T : class
    {

        public AriesCurdController(IFreeSql freeSql):base(freeSql)
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
            return BoolResult(_freeSql.AriesInsert(instance));
        }

        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [HttpPost("aries_add_list")]
        public virtual ApiReturnResult AddList(params T[] instance)
        {
            return BoolResult(_freeSql.AriesInsert(instance));
        }

    }
}
