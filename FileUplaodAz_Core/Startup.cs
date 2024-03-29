using FileUplaodAz_Core.Services;
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
using FileUplaodAz_Core.Services;
using FileUplaodAz_Core.Models;
using Microsoft.EntityFrameworkCore;
using FileUplaodAz_Core.Repository;

namespace FileUplaodAz_Core
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
            //services.AddDbContext<azblobstorageContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); 
            services.AddSingleton<IDbService, DbService>();
            services.AddScoped<IAzureBlobClientService,AzureBlobClientService>();
            services.AddTransient<IRecaptchaService, RecaptchaService>();
            services.AddSingleton<IConfiguration>(Configuration);
            //string clientKey = Configuration.GetSection("SecretKey:recaptchaClient")?.Value;
            //string serverKey = Configuration.GetSection("SecretKey:recaptchaClient")?.Value;
            services.AddControllersWithViews();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
