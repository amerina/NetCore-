using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace JWTAuthentication
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

            // For Entity Framework
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnStr")));

            // For Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.AddJwtBearer(Configuration);

            // Adding Authentication����
            services.AddAuthentication(options =>
            {
                //����Ĭ����֤����
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //����Ĭ����ս����
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //����Ĭ�Ϸ���
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                ////����Ĭ����֤����
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                ////����Ĭ����ս����
                //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                ////����Ĭ�Ϸ���
                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
             //cookie �����֤����
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                // Defines whether the bearer token should be stored in the Microsoft.AspNetCore.Authentication.AuthenticationProperties
                // after a successful authorization. ��JWT���浽��ǰ��HttpContext, �����ڿ��Ի�ȡ��ͨ��await HttpContext.GetTokenAsync("Bearer","access_token")
                // ���������Ϊfalse, ��token������claim��, Ȼ���ȡͨ��User.FindFirst("access_token")?.value.
                options.SaveToken = true;
                //Ϊmetadata����authority��֤����https
                // Gets or sets if HTTPS is required for the metadata address or authority. The
                // default is true. This should be disabled only in development environments.
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //Gets or sets a boolean to control if the issuer will be validated during token
                    // validation.�Ƿ���֤Issuer
                    ValidateIssuer = true,
                    // Gets or sets a boolean to control if the audience will be validated during token
                    // validation.�Ƿ���֤Audience
                    ValidateAudience = true,
                    //Gets or sets a string that represents a valid audience that will be used to check
                    //     against the token's audience.
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    // Gets or sets a System.String that represents a valid issuer that will be used
                    //     to check against the token's issuer.�������ǰ��ǩ��jwt������һ��
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                    // Gets or sets a boolean to control if the lifetime will be validated during token
                    //     validation.�Ƿ���֤ʧЧʱ��
                    ValidateLifetime = true,
                    // Gets or sets the clock skew to apply when validating a time.
                    ClockSkew = TimeSpan.FromMinutes(1)//��token����ʱ����֤������ʱ��

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

            app.UseRouting();

            //������֤
            app.UseAuthentication();

            //������Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
