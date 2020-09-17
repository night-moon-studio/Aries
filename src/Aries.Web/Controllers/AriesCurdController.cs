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


        [HttpPost("add")]
        public ApiReturnResult Add(T instance)
        {
            return Result(_freeSql.InsertWithInited(instance));
        }

    }
}
