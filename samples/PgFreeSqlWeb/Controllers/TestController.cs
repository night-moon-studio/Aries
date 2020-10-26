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

        public override ApiReturnResult QuerySingle([FromBody] SqlModel<Test> query)
        {
            query.AddQueryCondition(item => item.Id == 1);
            return base.QuerySingle(query);
        }


        public override ApiReturnResult Modify([FromBody] SqlModel<Test> opModel)
        {
            opModel.ModifyInstance.Instance.Name = "test";
            opModel.AddModifyField(item => item.Name);
            return base.Modify(opModel);
        }

        [HttpDelete]
        public ApiReturnResult Test()
        {

            return Result(_freeSql.Select<Test>().ToJoinList(item => new {
                TestName = item.Name,
                DomainId = item.Domain.AriesInnerJoin<Test2>().MapFrom(c => c.Id).Id,
                DomainName = item.Domain.AriesInnerJoin<Test2>().MapFrom(c => c.Id).Name,
                TypeName = item.Type.AriesInnerJoin<Test2>().MapFrom(c => c.Id).Name,
            }));

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
