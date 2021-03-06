﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RegistrationSystem.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace RegistrationSystem
{
    public class Startup
    {

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration["Data:RegistrationSystem:ConnectionString"]));
            services.AddDbContext<AppIdentityDbContext>(
                options => options.UseSqlServer(Configuration["Data:RegistrationSystemIdentity:ConnectionString"]));


            services.AddTransient<ISystemRepository, EFSystemRepository>();
            services.AddTransient<IModuleRepository, EFModuleRepository>();
            services.AddTransient<IVersionRepository, EFVersionRepository>();
            services.AddTransient<IUpdatesRepository, EFUpdatesRepository>();
            services.AddTransient<IEnhancementRepository, EFEnhancementRepository>();
            services.AddTransient<ICustomerRepository, EFCustomerRepository>();
            services.AddTransient<ISystemRegistrationRepository, EFSystemRegistrationRepository>();
            services.AddTransient<IModuleRegistrationRepository, EFModuleRegistrationRepository>();
            services.AddTransient<IEnhancementRegistrationReposity, EFEnhancementRegistrationRepository>();

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default",
                                template: "{controller=Home}/{action=Index}/{id?}");
            });

            AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
        }
    }
}
