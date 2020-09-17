using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aries.Web.Controllers
{

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
