using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

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

        }
    }
}
