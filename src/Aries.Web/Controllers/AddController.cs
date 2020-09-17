using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aries.Web.Controllers
{

    /// <summary>
    /// 新增路由 需要被继承
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AddController<T> : ModifyController<T> where T : class
    {

        public AddController(IFreeSql freeSql):base(freeSql)
        {
           
        }


        [HttpPost("add")]
        public ApiReturnResult Add(T instance)
        {
            return Result(_freeSql.InsertWithInited(instance));
        }

    }
}
