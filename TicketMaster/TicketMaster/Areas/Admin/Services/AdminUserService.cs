using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TicketMaster.Areas.Admin.Services.Interfaces;
using TicketMaster.Areas.Admin.ViewModels.User;
using TicketMaster.Data;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly TicketMasterDbContext dbContext;
        public AdminUserService(TicketMasterDbContext dbContext)
        {
            this.dbContext = dbContext; 
        }

        public List<User> DisplayAllUsers()
        {
            List<User> Users = new List<User>();
            Users= dbContext.Users.Include(u => u.Company).Include(u=>u.Projects).Include(u=>u.SendTickets).Include(u=>u.AnsweredTickets).ToList();
            return Users; 
        }
        public async Task<User> FindUser(string id)
        {
            var foundUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (foundUser == null)
            {
                throw new NullReferenceException($"No user witj id: {id} exists.");
            }
            return foundUser; 
        }
        //public async Task<Models.User> FindUserNameById(string id)
        //{

        //}
        //public async Task<Models.User> FindIdByUserName(string id)
        //{

        //}
        public IEnumerable<Company> CompaniesIdToSelect()
        {
            IEnumerable<Company> list = dbContext.Companies;
            return list; 
        }


        public async Task EditUser(EditUserBindingModel model)
        {
            var editUser = await FindUser(model.Id);
            if (editUser == null)
            {
                throw new NullReferenceException();
            }
            editUser.UserName = model.Username; 
            editUser.CompanyId = model.CompanyId;
            editUser.IsDelete = model.IsDeleted;
            editUser.IsGlobal = model.IsGlobal;

            dbContext.Update(editUser);
            await dbContext.SaveChangesAsync(); 
        }
        public async Task DeleteUser(DeleteUserViewModel model)
        {
            var userToDelete = await FindUser(model.Id);
            if (userToDelete == null)
            {
                throw new NullReferenceException($"No user with id: {model.Id} exists.");
            }
            else
            {
                userToDelete.IsDelete = true;
            }

            dbContext.Update(userToDelete);
            await dbContext.SaveChangesAsync(); 
        }

        public IEnumerable<Project> ProjectIdToSelect()
        {
            IEnumerable<Project> list = dbContext.Projects.ToList();
            return list;
        }
        public async Task AddUserToProject(AddUserToProjectViewModel model)
        {
            if (model.UserId == null)
            {
                throw new NullReferenceException($"No user with id:{model.UserId}");
            }
            var newCombo= new UserProject();
            newCombo.ProjectId = model.ProjectId;
            newCombo.UserId = model.UserId;

            await dbContext.UserProjects.AddAsync(newCombo);
            await dbContext.SaveChangesAsync(); 
        }
       public async Task RemoveUserToProject(AddUserToProjectViewModel model)
       {
            if (model.UserId == null)
            {
                throw new NullReferenceException($"No user with id:{model.UserId}");
            }
            var removeCombo = await dbContext.UserProjects.FirstOrDefaultAsync(up => up.UserId == model.UserId && up.ProjectId == model.ProjectId);
            if (removeCombo == null)
            {
                throw new NullReferenceException($"Not Found"); 
            }
            dbContext.UserProjects.Remove(removeCombo);
            await dbContext.SaveChangesAsync(); 
        }

        public async Task<List<Ticket>> DisplayAllUserAnsweredTickets(string id)
        {
            List <Ticket> allTickets= new List<Ticket>();
            allTickets = dbContext.Tickets.Where(t => t.AgentId == id).Include(t=>t.WorkingTimes).ToList();
            return allTickets; 
        }
        public async Task< List<Ticket>> DisplayAllUserSendTickets(string id)
        {
            List < Ticket > allTickets= new List<Ticket>();
            allTickets = dbContext.Tickets.Where(t => t.AuthorId == id).Include(t=>t.WorkingTimes).ToList();
            return allTickets; 
        }
        public List<Project> DisplayAllUserProjects(string id)
        {
            List<Project> allProjects = new List<Project>();
            allProjects = dbContext.Projects.Where(p => p.Workers.Any(w => w.UserId == id)).Include(p=>p.IncomingTickets).Include(t=>t.Workers).ToList();
            return allProjects; 
        }

    }
}
