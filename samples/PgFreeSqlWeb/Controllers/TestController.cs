using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
