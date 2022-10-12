using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerClientA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMemoryCache();
            //���JWT��Ȩ
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                   {
                       // IdentityServer ��ַ
                       options.Authority = "https://localhost:5001";
                       //��Ҫhttps
                       options.RequireHttpsMetadata = true;
                       //����Ҫ�� IdentityServer �����APISource����һ��
                       //�������Ϊ��ǰ�ͻ�����IndentityServer�����APISource
                       options.Audience = "api";
                       //token Ĭ������5���ӹ���ʱ��ƫ�ƣ���������Ϊ0��
                       //�������Ϊʲô����ͻ��������˹���ʱ��Ϊ5�룬���ں��Կ��Է�������
                       options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                       options.Events = new JwtBearerEvents
                       {
                           //AccessToken ��֤ʧ��
                           OnChallenge = op =>
                           {
                               //��������Ĭ�ϲ���
                               op.HandleResponse();
                               //�������Զ��巵����Ϣ
                               //op.Response.Headers.Add("token", "401");
                               op.Response.ContentType = "application/json";
                               op.Response.StatusCode = StatusCodes.Status401Unauthorized;
                               op.Response.WriteAsync(JsonConvert.SerializeObject(new
                               {
                                   status = StatusCodes.Status401Unauthorized,
                                   msg = "token��Ч"
                               }));
                               return Task.CompletedTask;
                           }
                       };
                   });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //�����֤�м��
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //ʹ������·��
                endpoints.MapControllers();
            });
        }
    }
}
