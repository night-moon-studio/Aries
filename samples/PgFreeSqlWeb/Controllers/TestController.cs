using Aries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestLib;

namespace PgFreeSqlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : AriesCurdController<Test>
    {
        //AriesOptimisticLock _lock;
        public TestController(IFreeSql freeSql) : base(freeSql)
        {
            //_lock = new AriesOptimisticLock(freeSql);
            //_lock.SpecifyLock(1,"test");
        }

        [HttpGet("testLock")]
        public int TestLock()
        {
            Parallel.For(0, 100, (_,state) => {

                AriesOptimisticLock _lock = new AriesOptimisticLock(_freeSql);
                _lock.SpecifyLock(uid:1, name:"test");
                _lock.Execute(() =>
                {
                    //var score =  _freeSql.Select<TestLock>().First();
                    //System.Diagnostics.Debug.WriteLine("TEST:当前分数\t" + score.Score);
                    //_freeSql.Ado.ExecuteNonQuery("UPDATE public.\"TestLock\" SET \"Score\" = \"Score\" + 1 where \"Score\"=" + score.Score);

                    System.Diagnostics.Debug.WriteLine("进入函数");
                    _freeSql.Ado.ExecuteNonQuery("UPDATE public.\"TestLock\" SET \"Score\" = \"Score\" + 1");

                });

            });
            return 0;
            // });
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
                DomainId = item.Domain.AriesInnerJoin<Test21>(c => c.Id).Id,
                DomainName = item.Domain.AriesInnerJoin<Test21>(c => c.Id).Name,
                TypeName = item.Type.AriesInnerJoin<Test21>(c => c.Id).Name
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
