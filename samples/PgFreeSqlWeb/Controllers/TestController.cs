using Aries;
using Microsoft.AspNetCore.Mvc;
using TestLib;

namespace PgFreeSqlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : AriesCurdController<Test>
    {
        public TestController(IFreeSql freeSql) : base(freeSql)
        {

        }

        [HttpDelete]
        public void Test()
        {

            Student student = new Student();
            _freeSql.Select<Student>().ToJoinList(item => new { 
                Age = item.Age,
                Address = item.Address.AriesLeftJoin<Test2>().MapFrom(c => c.Name),
                Name = item.Name.AriesInnerJoin<Test>().MapFrom(c => c.Id) 
            });
        }

        //[HttpPost("join")]
        //public ApiReturnPageResult GetJoinList(SqlModel<Test> sqlModel)
        //{
        //    //return Result( _freeSql
        //    //    .QueryFromSqlModel(sqlModel,out long total).ToJoinList(item => new
        //    //    {
        //    //        item.Id,
        //    //        TestName = item.Name,
        //    //        DomainName = InnerJoin<Test2>.MapFrom(item => item.Name),
        //    //        TypeName = InnerJoin<Test3>.MapFrom(item => item.TypeName)
        //    //    }),total);
        //}
    }

    public class Student
    {
        public string Name;
        public string Age;
        public string Address;
    }


}
