using ASPNETAOP.Aspect;
using ASPNETAOP.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//runs on localhost:44363
namespace ASPNETAOP
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
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {

                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/html";

                        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                        await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                        if (exceptionHandlerPathFeature?.Error is ASPNETAOP.Aspect.UserNotLoggedInException)
                        {
                            await context.Response.WriteAsync("You have to be logged in<br><br>\r\n");
                        }
                        else if (exceptionHandlerPathFeature?.Error is ASPNETAOP.Aspect.UserPermissionNotEnoughException)
                        {
                            await context.Response.WriteAsync("You don't have the necessary permission in<br><br>\r\n");
                        }

                        await context.Response.WriteAsync(
                                                      "<a href=\"/\">Login</a><br>\r\n");
                        await context.Response.WriteAsync("</body></html>\r\n");
                        await context.Response.WriteAsync(new string(' ', 512));
                    });
                });
            }
            else
            {

                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                       
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/html";

                        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                        await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                        if (exceptionHandlerPathFeature?.Error is ASPNETAOP.Aspect.UserNotLoggedInException)
                        {
                            await context.Response.WriteAsync("You have to be logged in<br><br>\r\n");
                        }else if (exceptionHandlerPathFeature?.Error is ASPNETAOP.Aspect.UserPermissionNotEnoughException)
                        {
                            await context.Response.WriteAsync("You don't have the necessary permission in<br><br>\r\n");
                        }

                            await context.Response.WriteAsync(
                                                      "<a href=\"/\">Login</a><br>\r\n");
                        await context.Response.WriteAsync("</body></html>\r\n");
                        await context.Response.WriteAsync(new string(' ', 512));
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            SessionList.listObject.count = 0;

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=UserLogin}/{action=Login}/{id?}");
            });
        }
    }
}

