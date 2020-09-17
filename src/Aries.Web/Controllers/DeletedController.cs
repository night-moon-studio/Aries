using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aries.Web.Controllers
{

    public class DeletedController<T> : ModifyController<T> where T : class
    {

        public DeletedController(IFreeSql freeSql):base(freeSql)
        {
           
        }


        [HttpPost("delete")]
        public ApiReturnResult Deleted([FromQuery] T instance)
        {
            return Result(_freeSql
                .Delete<T>().QueryWithHttpEntity(Request.Query.Keys, instance)
                .ExecuteAffrows());
        }

    }
}
