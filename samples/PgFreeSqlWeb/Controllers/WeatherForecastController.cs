using FreeSql.Natasha.Extension;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PgFreeSqlWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFreeSql _freeSql;
        public WeatherForecastController(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }

        [HttpPost]
        public IEnumerable<object> Post([FromQuery] Test instance, [FromBody] QueryModel query)
        {
           
            return _freeSql
                .Select<Test>()
                .QueryWithHttpEntity(Request.Query.Keys, instance)
                .QueryWithModel(query)
                .ToJoinList(item => new
                {
                    item.Id,
                    TestName = item.Name,
                    DomainName = InnerJoin<Test2>.MapFrom(item => item.Name),
                    TypeName = InnerJoin<Test3>.MapFrom(item => item.TypeName)
                });

        }


        /// <summary>
        /// 返回 TestResult 类型，并对 TestResult 部分字段进行特殊映射
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("test")]
        public IEnumerable<TestResult> Post1([FromQuery] Test instance, [FromBody] QueryModel query)
        {

            return _freeSql
                .Select<Test>()
                .QueryWithHttpEntity(Request.Query.Keys, instance)
                .QueryWithModel(query)
                .UseMappingClass<Test,TestResult>()
                .ToJoinList(item=>new
                {

                    TESTName = item.Name,
                    DomainName = InnerJoin<Test2>.MapFrom(item => item.Name),
                    TypeName = InnerJoin<Test3>.MapFrom(item => item.TypeName),

                });

        }


        [HttpPost("increment")]
        public bool Post2(Test instance)
        {

            return _freeSql.UpdateIncrement(instance);

        }


        [HttpPost("insert")]
        public Test Post3(Test instance)
        {

            _freeSql.SetInsertInit<Test>(item => item.Domain = 2);
            return _freeSql.InsertWithInited(instance);

        }

    }
    public class TestResult 
    {
        public long Id { get; set; }
        public string TESTName { get; set; }
        public string DomainName { get; set; }
        public string TypeName { get; set; }
    }

    public class Test
    {
        public long Id { get; set; }
        public short Domain { get; set; }
        public short Type { get; set; }
        public string Name { get; set; }
    }

    public class Test2
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class Test3
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
    }
}
