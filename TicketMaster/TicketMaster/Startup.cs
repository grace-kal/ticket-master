using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using TicketMaster.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketMaster.Models;
using TicketMaster.Seeder;
using TicketMaster.Areas.Admin.Services.Interfaces;
using TicketMaster.Areas.Admin.Services;
using TicketMaster.Areas.Agent.Services.Interfaces;
using TicketMaster.Areas.Agent.Services;
using TicketMaster.Services.Interfaces;
using TicketMaster.Services;

namespace TicketMaster
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
            services.AddDbContext<TicketMasterDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TicketMasterDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<IAdminCompanyService, AdminCompanyService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IAdminProjectService, AdminProjectService>();
            services.AddScoped<IAdminTicketService, AdminTicketService>();
            services.AddScoped<IAgentCompanyService, AgentCompanyService>();
            services.AddScoped<IAgentProjectService, AgentProjectService>();
            services.AddScoped<IAgentTicketService, AgentTicketService>();
            services.AddScoped<IUserHomeService, UserHomeService>(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<TicketMasterDbContext>();
                dbContext.Database.Migrate();
                new RoleSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                
                endpoints.MapAreaControllerRoute(
                   name: "area", "Admin",
                   pattern: "{area:exists}/{cotroller}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                    name: "area", "Agent",
                    pattern: "{area:exists}/{cotroller}/{action=Index}/{id?}");
            });
        }
    }
}
