using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Areas.Agent.Services.Interfaces;
using TicketMaster.Data;
using TicketMaster.Models;

namespace TicketMaster.Areas.Agent.Services
{
    public class AgentCompanyService:IAgentCompanyService
    {
        private readonly TicketMasterDbContext dbContext;
        public AgentCompanyService(TicketMasterDbContext dbContext)
        {
            this.dbContext = dbContext; 
        }
        public async Task<Company> FindCompany(int id)
        {
            var company = await dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                throw new NullReferenceException($"No company with id:{id} exists."); 
            }
            return company; 
        }
        public async Task<User> FindUser(string id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NullReferenceException($"No user with id:{id} exist.");
            }
            return user; 
        }
        public async Task<User> FindUserIdByUserName(string username)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new NullReferenceException($"No user with username:{username} exist.");
            }
            return user; 
        }
        public async Task<User> FindUserUserNameById(string id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NullReferenceException($"No user with username:{id} exist.");
            }
            return user;
        }
        public async Task<Company> MyCompany(string username)
        {
            var company = await dbContext.Companies
                .Include(c=>c.Workers)
                .Include(c=>c.Projects)
                .FirstOrDefaultAsync(c => c.Workers.Any(w => w.UserName == username));
            if (company == null)
            {
                throw new NullReferenceException($"No worker in any company with this id:{username}");
            }
            return company;  
        }
        public List<Project> CompanyProjects(int id)
        {
            List<Project> list = dbContext.Projects
                .Where(p => p.CompanyId == id)
                .Include(p=>p.Workers)
                .Include(p=>p.IncomingTickets)
                .ToList();
            return list; 
        }
        public List<User> CompanyWorkers(int id)
        {
            List<User> list = dbContext.Users.Where(p => p.CompanyId == id)
                .Include(u=>u.Projects)
                .Include(u=>u.SendTickets)
                .Include(u=>u.AnsweredTickets)
                .ToList();
            return list;
        }








    }
}
