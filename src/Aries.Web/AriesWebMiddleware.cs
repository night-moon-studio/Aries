using FreeSql;
using FreeSql.Aop;
using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AriesWebMiddleware
    {

        public static IFreeSql FreeSqlHandler;
        public static IServiceCollection AddAriesMySql(this IServiceCollection services, string connectionString, Action<IFreeSql> callBack = null)
        {
            return AddAriesFreeSql(services, DataType.MySql, connectionString, callBack);
        }
        public static IServiceCollection AddAriesSqlServer(this IServiceCollection services, string connectionString, Action<IFreeSql> callBack = null)
        {
            return AddAriesFreeSql(services, DataType.SqlServer, connectionString, callBack);
        }
        public static IServiceCollection AddAriesOracle(this IServiceCollection services, string connectionString, Action<IFreeSql> callBack = null)
        {
            return AddAriesFreeSql(services, DataType.Oracle, connectionString, callBack);
        }
        public static IServiceCollection AddAriesSqlite(this IServiceCollection services, string connectionString, Action<IFreeSql> callBack = null)
        {
            return AddAriesFreeSql(services, DataType.Sqlite, connectionString, callBack);
        }
        public static IServiceCollection AddAriesPgSql(this IServiceCollection services, string connectionString, Action<IFreeSql> callBack = null)
        {
            return AddAriesFreeSql(services, DataType.PostgreSQL, connectionString, callBack);
        }
        public static IServiceCollection AddAriesFreeSql(this IServiceCollection services, DataType sqlType,string connectionString,Action<IFreeSql> callBack=null)
        {
            FreeSqlHandler = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(sqlType, connectionString)
                .Build();
            callBack?.Invoke(FreeSqlHandler);
            services.AddSingleton(FreeSqlHandler);
            return services;
        }


        public static IServiceCollection AddAriesEntities(this IServiceCollection services, params Type[] types)
        {

            if (FreeSqlHandler == default)
            {
                throw new System.Exception("请优先注册 AriesFreeSql : services.AddAriesFreeSql(type,conn)");
            }
            TableInfomation.Initialize(FreeSqlHandler, types);
            return services;

        }


        public static IServiceCollection AddAriesAssembly(this IServiceCollection services, string path)
        {

            if (FreeSqlHandler == default)
	        {
               throw new System.Exception("请优先注册 AriesFreeSql : services.AddAriesFreeSql(type,conn)");
	        }   


            NatashaInitializer.InitializeAndPreheating();
            var assembly = Assembly.Load(path);
            var types = assembly.GetTypes();

            TableInfomation.Initialize(FreeSqlHandler, types);
            return services;

        }
    }
}
