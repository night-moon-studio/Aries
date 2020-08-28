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
        static WeatherForecastController()
        {
            PropertiesCache<Test>.SetSelectBlockFields("Domain", "Address");
            PropertiesCache<Test>.SetWhereBlockFields("Type");
            PropertiesCache<Test>.SetUpdateAllowFields("Name");
            PropertiesCache<Test>.SetUpdateInit(item => item.Address = "null");
            PropertiesCache<Test>.SetInsertInit(item => item.Domain = 2);

        }

        [HttpPost("normal")]
        public IEnumerable<object> Post5([FromQuery] Test instance, [FromBody] QueryModel query)
        {

            return _freeSql
                .Select<Test>()
                .QueryWithHttpEntity(Request.Query.Keys, instance)
                .QueryWithModel(query)
                .ToLimitList();

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


        /// <summary>
        /// 整体扫描的增量更新
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [HttpPost("increment")]
        public bool Post2(Test instance)
        {
            
            return _freeSql.UpdateAll(instance).ExecuteAffrows() == 1;

        }



        /// <summary>
        /// 按需更新 主键 作为更新条件，只更新参数中的字段
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [HttpPost("updatebyfields")]
        public bool Post4([FromQuery]Test instance)
        {

            return _freeSql.UpdateWithHttpModel(Request.Query.Keys, instance).WherePrimaryKeyFromEntity(instance).ExecuteAffrows()!=0;

        }


        /// <summary>
        /// 设置初始化插入
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        public Test Post3(Test instance)
        {

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
        public string Address { get; set; }
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
