using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PgFreeSqlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : AriesCurdController<Test>
    {
        public TestController(IFreeSql freeSql) : base(freeSql)
        {

        }


        [HttpPost("join")]
        public ApiReturnPageResult GetJoinList(SqlModel<Test> sqlModel)
        {
            return Result( _freeSql
                .QueryFromSqlModel(sqlModel,out long total).ToJoinList(item => new
                {
                    item.Id,
                    TestName = item.Name,
                    DomainName = InnerJoin<Test2>.MapFrom(item => item.Name),
                    TypeName = InnerJoin<Test3>.MapFrom(item => item.TypeName)
                }),total);
        }
    }
}
