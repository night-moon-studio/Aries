using Aries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TestLib;

namespace PgFreeSqlWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFreeSql _freeSql;
        public static string Sql;
        public WeatherForecastController(IFreeSql freeSql)
        {
             _freeSql = freeSql;
        }


        static WeatherForecastController()
        {
            ////显示返回的字段
            //PropertiesCache<Test>.SetSelectBlockFields("Domain", "Address");
            ////限制查询条件
            //PropertiesCache<Test>.SetWhereBlockFields("Type");
            ////允许更新的字段
            //PropertiesCache<Test>.SetUpdateAllowFields("Name", "Address");
            ////初始化更新字段
            ////PropertiesCache<Test>.SetUpdateInit(item => item.Address = "null");
            ////初始化插入字段
            //PropertiesCache<Test>.SetInsertInit(item => item.Domain = 2);

        }

        [HttpGet]
        public string ShowSql()
        {
            return Sql;
        }

        [HttpPost("normal")]
        public IEnumerable<object> Post5([FromQuery] Test instance, [FromBody] QueryModel query)
        {

            return _freeSql
                .Select<Test>()
                .WhereWithEntity(Request.Query.Keys, instance)
                .WhereWithModel(query)
                .ToLimitList();

        }

        //[HttpPost]
        //public IEnumerable<object> Post([FromQuery] Test instance, [FromBody] QueryModel query)
        //{
           
        //    return _freeSql
        //        .Select<Test>()
        //        .QueryWithHttpEntity(Request.Query.Keys, instance)
        //        .QueryWithModel(query)
        //        .ToJoinList(item => new
        //        {
        //            item.Id,
        //            TestName = item.Name,
        //            DomainName = InnerJoin<Test2>.MapFrom(item => item.Name),
        //            TypeName = InnerJoin<Test3>.MapFrom(item => item.TypeName)
        //        });

        //}



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
        /// 按需更新
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <returns></returns>
        [HttpPost("modifybyid")]
        public long ModifyById([FromQuery] Test updateEntity)
        {
            return _freeSql
                .UpdateWithEntity(Request.Query.Keys, updateEntity)
                .WherePrimaryKeyFromEntity(updateEntity)
                .ExecuteAffrows();
        }

        /// <summary>
        /// 按需更新 主键 作为更新条件，只更新参数中的字段
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [HttpPost("updatebyfields")]
        public bool Post4([FromQuery]Test instance)
        {
            
            return _freeSql.UpdateWithEntity(Request.Query.Keys, instance).WherePrimaryKeyFromEntity(instance).ExecuteAffrows()!=0;

        }


        /// <summary>
        /// 设置初始化插入
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        public bool Post3(Test instance)
        {

            return _freeSql.AriesInsert(instance);

        }

    }
}
