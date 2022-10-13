using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace MVCClientC
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
            services.AddControllersWithViews();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //AddAuthentication �������֤������ӵ� DI
            services.AddAuthentication(options =>
            {
                //ʹ��cookie �ڱ��ص�¼�û���ͨ�� "Cookies" ��Ϊ DefaultScheme����
                //�� DefaultChallengeScheme����Ϊ oidc���û���ʹ�� OpenID Connect Э���¼
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                //ʹ�� AddCookie ����ӿ��Դ��� cookie �Ĵ������
                .AddCookie("Cookies")
                //AddOpenIdConnect ��������ִ�� OpenID Connect Э��Ĵ������
                .AddOpenIdConnect("oidc", options =>
                {
                    //Authority ָʾ�������Ʒ������ڵ�λ��
                    options.Authority = "https://localhost:5001";
                    // ͨ��ClientId �� ClientSecret ��ʶ������ͻ���
                    options.ClientId = "ClientC";
                    options.ClientSecret = "secret";
                    //ʹ����ν�� ��Ȩ��(authorization code) ������ PKCE ���ӵ� OpenID Connect �ṩ����
                    options.ResponseType = "code";
                    //SaveTokens ���ڽ����� IdentityServer �����Ƴ־ñ����� cookie �У���Ϊ�Ժ���Ҫ���ǣ�
                    options.SaveTokens = true;

                    //ͨ�� scope ��������������Դ
                    options.Scope.Add("SecretAPIScope");
                    options.Scope.Add("offline_access");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //Ϊ��ȷ����ÿ��������ִ�������֤����
            //�� UseAuthentication ��ӵ� Startup �е� Configure
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
                //Adds the default authorization policy to the endpoint(s).
                //RequireAuthorization �������ö�����Ӧ�ó������������
                //�������ÿ������������������Ļ�����ָ����Ȩ������ʹ�� [Authorize] ����
                         .RequireAuthorization();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
