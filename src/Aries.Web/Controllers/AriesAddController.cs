using Aries;

namespace Microsoft.AspNetCore.Mvc
{

    /// <summary>
    /// 新增路由 需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AriesAddController<T> : AriesModifyController<T> where T : class
    {

        public AriesAddController(IFreeSql freeSql):base(freeSql)
        {
           
        }


        [HttpPost("add")]
        public ApiReturnResult Add(T instance)
        {
            return Result(_freeSql.InsertWithInited(instance));
        }

    }
}
