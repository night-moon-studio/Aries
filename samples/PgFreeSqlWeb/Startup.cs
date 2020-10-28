using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PgFreeSqlWeb.Controllers;
using System;
using System.IO;
using TestLib;

namespace PgFreeSqlWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAriesPgSql(
                "Host=127.0.0.1;Port=5432;Username=postgres;Password=123456; Database=test;Pooling=true;Minimum Pool Size=1",
                freeSql => { 

                    freeSql.Aop.CurdBefore += Aop_CurdBefore;

                });

            //初始化扫描
            //services.AddAriesEntities(typeof(Test),typeof(Test21));
            services.AddAriesAssembly("TestLib");
           

            //配置 Join 关系
            //OrmNavigate<Test>.Connect<Test2>(test => test.Domain, test2 => test2.Id);
            //OrmNavigate<Test>.Connect<Test3>(test => test.Type, test3 => test3.Id);
            //OrmNavigate<Test>.Join<Test3>("Type", "Id");

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, SwaggerConfiguration.XmlName));
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        private void Aop_CurdBefore(object sender, FreeSql.Aop.CurdBeforeEventArgs e)
        {
            WeatherForecastController.Sql = e.Sql;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "";

            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
