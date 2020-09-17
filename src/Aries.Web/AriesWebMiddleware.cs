using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Aries.Web
{
    public static class AriesWebMiddleware
    {

        public static void UseAries(this IServiceCollection services, IFreeSql freeSql, string path)
        {

            NatashaInitializer.InitializeAndPreheating();

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("没有发现实体类文件！");
            }

            var assembly = Assembly.LoadFrom(path);
            var types = assembly.GetTypes();

            TableInfomation.Initialize(freeSql, types);

            //var repositoryJoinQueryType = typeof(QEJoinQueryRepository<>);
            //var repositoryAuditType = typeof(QEStatusRepository<>);
            //var repositoryWriteType = typeof(QEWriteRepository<>);
            //var repositoryPublishType = typeof(QEPublishRepository<>);


            //foreach (var item in types)
            //{
                
            //    if (item.IsClass)
            //    {
            //        services.AddScoped(repositoryAuditType.MakeGenericType(item));
            //        services.AddScoped(repositoryPublishType.MakeGenericType(item));
            //        services.AddScoped(repositoryJoinQueryType.MakeGenericType(item));
            //        services.AddScoped(repositoryWriteType.MakeGenericType(item));
            //    }

            //}

        }
    }
}
