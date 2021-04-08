using Aries;

namespace Microsoft.AspNetCore.Mvc
{

    /// <summary>
    /// 新增路由 需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AriesWriteController<T> : AriesModifyController<T> where T : class
    {

        public AriesWriteController(IFreeSql freeSql):base(freeSql)
        {
           
        }

        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [HttpPost("aries_add")]
        public virtual AriesJsonResult Add(T instance)
        {
            return BoolResult(_freeSql.AriesInsert(instance));
        }

        /// <summary>
        /// 增加实体集合
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        [HttpPost("aries_add_list")]
        public virtual AriesJsonResult AddList(T[] instances)
        {
            return BoolResult(_freeSql.AriesInsert(instances));
        }

    }
}
