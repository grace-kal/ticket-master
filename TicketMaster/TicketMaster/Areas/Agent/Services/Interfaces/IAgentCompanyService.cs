using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Models;

namespace TicketMaster.Areas.Agent.Services.Interfaces
{
    public interface IAgentCompanyService
    {
        public Task<Company> FindCompany(int id);
        public Task<User> FindUser(string id);
        public Task<User> FindUserIdByUserName(string username);
        public Task<User> FindUserUserNameById(string id);
        public Task<Company> MyCompany(string username);
        public List<Project> CompanyProjects(int id);
        public List<User> CompanyWorkers(int id); 
    }
}
