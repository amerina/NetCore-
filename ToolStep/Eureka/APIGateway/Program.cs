﻿namespace APIGateway
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Ocelot.DependencyInjection;
    using Ocelot.Middleware;
  
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseUrls("http://*:9000")
               .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)                      
                        .AddJsonFile("ocelot.json")
                        .AddEnvironmentVariables();
                })
               .ConfigureServices(s =>
                {
                    //注入Ocelot服务
                    s.AddOcelot();
                })
                .Configure(a =>
                {
                    //使用Ocelot中间件
                    a.UseOcelot().Wait();
                })
                .Build();
    }
}
