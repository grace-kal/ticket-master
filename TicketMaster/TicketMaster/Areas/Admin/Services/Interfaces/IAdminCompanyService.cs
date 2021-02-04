using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Areas.Admin.ViewModels.Company;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Services.Interfaces
{
    public interface IAdminCompanyService
    {
        //---for global Include
       // public IEnumerable<Company> AllCompanies { get; }
        public List<Company> DisplayAllCompanies();
        public Task<Company> FindCompany(int id);
        public Task CreateCompany(CreateCompanyBindingModel model);
        public Task DeleteCompany(DeleteCompanyViewModel model);
        public Task EditCompany(EditCompanyBindingModel model);

        public List<User> DisplayAllCompanyWorkers(int id);
        public List<Project> DisplayAllCompanyProjects(int id); 

    }
}
