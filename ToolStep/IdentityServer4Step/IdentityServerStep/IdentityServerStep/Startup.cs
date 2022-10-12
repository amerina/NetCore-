using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerStep
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
            services.AddControllers();

            //������Դ�Ϳͻ��˶��巢���� Startup.cs ��
            var builder = services.AddIdentityServer()
               //���������û��֤�����ʹ�õĿ�������
               .AddDeveloperSigningCredential()        
               .AddInMemoryApiScopes(Config.ApiScopes)
               .AddInMemoryApiResources(Config.ApiResources)
               .AddInMemoryClients(Config.Clients)
               //��IdentityServer ע�������Դ
               .AddInMemoryIdentityResources(Config.IdentityResources)
               //��Ӳ����û�
               .AddTestUsers(Config.Users);

            builder.AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //ʹ��IdentityServer4 �м��
            app.UseIdentityServer();

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
