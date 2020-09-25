using System.IO;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AriesWebMiddleware
    {

        public static void AddAries(this IServiceCollection services, IFreeSql freeSql, string path)
        {

            NatashaInitializer.InitializeAndPreheating();
            var assembly = Assembly.Load(path);
            var types = assembly.GetTypes();

            TableInfomation.Initialize(freeSql, types);

        }
    }
}
