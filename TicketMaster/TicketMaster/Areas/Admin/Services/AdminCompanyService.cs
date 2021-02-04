using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Areas.Admin.Services.Interfaces;
using TicketMaster.Areas.Admin.ViewModels.Company;
using TicketMaster.Data;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Services
{
    public class AdminCompanyService:IAdminCompanyService
    {
        private readonly TicketMasterDbContext dbContext;
        public AdminCompanyService(TicketMasterDbContext dbContext)
        {
            this.dbContext = dbContext; 
        }

        //--to Include globally
        //public IEnumerable<Company> AllCompanies
        //{
        //    get
        //    {
        //        dbContext.Projects.Load();
        //        dbContext.Users.Load();
        //        dbContext.Tickets.Load();
        //        return dbContext.Companies;
        //    }
        //}

        public List<Company> DisplayAllCompanies()
        {
            List<Company> allCompanies = new List<Company>();
            //---for the global Include
            //allCompanies = AllCompanies.ToList();
            allCompanies = dbContext.Companies.Include(c => c.Projects).Include(c => c.Workers).ToList();

            return allCompanies; 
        }
        public async Task<Company> FindCompany(int id)
        {
            var company = await dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                throw new NullReferenceException($"No company with id: {id}");
            }
            else return company; 

        }

        public async Task CreateCompany(CreateCompanyBindingModel model)
        {
            var company = new Company();
            company.Name = model.Name;
            company.IsDeleted = model.IsDeleted;

            await dbContext.Companies.AddAsync(company);
            await dbContext.SaveChangesAsync(); 

        }
        public async Task EditCompany(EditCompanyBindingModel model)
        {
            var editedCompany = await FindCompany(model.Id);
            if (editedCompany == null)
            {
                throw new NullReferenceException($"No company with id:{model.Id} exists.");
            }
            else
            {
                editedCompany.Name = model.Name;
                editedCompany.IsDeleted = model.IsDeleted;
            }
            dbContext.Update(editedCompany);
            await dbContext.SaveChangesAsync(); 
            

        }
        public async Task DeleteCompany(DeleteCompanyViewModel model)
        {
            var companyToDelete = await FindCompany(model.Id); 
            if (companyToDelete == null)
            {
                throw new NullReferenceException($"No company with {model.Id} exists.");
            }
            else
            {
                companyToDelete.IsDeleted = true; 
            }
            dbContext.Companies.Update(companyToDelete);
            await dbContext.SaveChangesAsync(); 

        }


        public List<User> DisplayAllCompanyWorkers(int id)
        {
            List<User> companyWorkers = new List<User>();
            companyWorkers = dbContext.Users.Where(u=>u.CompanyId==id).Include(u => u.AnsweredTickets).Include(u => u.SendTickets).Include(u => u.Projects).ToList();
            
            return companyWorkers; 
        }

        public List<Project> DisplayAllCompanyProjects(int id)
        {
            List<Project> companyProjects = new List<Project>();
            companyProjects = dbContext.Projects.Where(p => p.CompanyId == id).Include(p => p.IncomingTickets).Include(p=>p.Workers).ToList();
            
            return companyProjects; 
        }
    }
}
