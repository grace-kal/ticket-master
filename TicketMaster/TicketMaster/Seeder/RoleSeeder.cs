using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Data;
using TicketMaster.Models;

namespace TicketMaster.Seeder
{
    public class RoleSeeder:ISeeder
    {
        public async Task SeedAsync(TicketMasterDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            await SeedRolesAsync(roleManager, "Admin");
            await SeedRolesAsync(roleManager, "Agent");

            await SeedCompanyAsync(dbContext);
            await SeedUserWithRoleAdminAsync(userManager);
            await SeedUserWithRoleAgentAsync(userManager);
        }
        private async Task SeedUserWithRoleAdminAsync(UserManager<User> userManager)
        {
            var user = await userManager.FindByNameAsync("Admin");
            if (user == null)
            {
                var result = await userManager.CreateAsync(new User
                {
                    UserName = "Admin",
                    Email = "kalinina.grace@gmail.com",
                    CompanyId = 1,
                    EmailConfirmed = true,
                }, "Admin_12345");

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
                else
                {
                    user = await userManager.FindByNameAsync("Admin");

                    await userManager.AddToRoleAsync(user, "Admin");
                }

            }
        }
        private async Task SeedUserWithRoleAgentAsync(UserManager<User> userManager)
        {
            var user = await userManager.FindByNameAsync("Agent");
            if (user == null)
            {
                var result = await userManager.CreateAsync(new User
                {
                    UserName = "Agent",
                    Email = "agent.kalinina.grace@gmail.com",
                    CompanyId = 1,
                    EmailConfirmed = true,
                }, "Agent_12345");

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
                else
                {
                    user = await userManager.FindByNameAsync("Agent");

                    await userManager.AddToRoleAsync(user, "Agent");
                }

            }
        }
        private async Task SeedCompanyAsync(TicketMasterDbContext dbContext)
        {
            var company = dbContext.Companies.FirstOrDefault(c => c.Name == "GlobalInc");
            if (company == null)
            {
                var result = dbContext.Companies.Add(new Company
                {
                    Name = "GlobalInc",
                    IsDeleted = false,

                });
                await dbContext.SaveChangesAsync();
            }
        }
        private async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, string v)
        {
            var role = await roleManager.FindByNameAsync(v);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(v));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }


    }
}
