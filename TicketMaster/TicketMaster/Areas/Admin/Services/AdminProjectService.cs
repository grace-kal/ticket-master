using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Areas.Admin.Services.Interfaces;
using TicketMaster.Areas.Admin.ViewModels.Project;
using TicketMaster.Data;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Services
{
    public class AdminProjectService : IAdminProjectService
    {
        private readonly TicketMasterDbContext dbContext;
        public AdminProjectService(TicketMasterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Project> DisplayAllProjects()
        {
            List<Project> projects = new List<Project>();
            projects = dbContext.Projects
                .Include(p=>p.IncomingTickets)
                .Include(p=>p.Workers)
                .Include(p=>p.Company).ToList();
            return projects; 
        }
        public async Task<Project> FindProject(string id)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(u => u.Id == id);
            if (project == null)
            {
                throw new NullReferenceException($"No project with id:{id}.");
            }
            else return project; 
        }


        public async Task CreateProject(CreateProjectBindingModel model)
        {
            var newProject = new Project();
            newProject.Id = model.Id;
            newProject.Title = model.Title;
            newProject.Description = model.Description;
            newProject.CompanyId = model.CompanyId;
            newProject.WorkTime = model.WorkTime;
            newProject.IsDeleted = model.IsDelete;

            await dbContext.Projects.AddAsync(newProject);
            await dbContext.SaveChangesAsync(); 
        }
        public  IEnumerable<Company> CompaniesIdToSelect()
        {
            IEnumerable<Company> list = dbContext.Companies; 
            return list; 
        }
        public async Task EditProject(EditProjectBindingModel model)
        {
            var findProject = await FindProject(model.Id);
            if (findProject == null)
            {
                throw new NullReferenceException($"No project with id{model.Id} exists.");
            }
            findProject.Title = model.Title;
            findProject.Description = model.Description;
            findProject.CompanyId = model.CompanyId;
            findProject.WorkTime = model.WorkTime;
            findProject.IsDeleted = model.IsDelete;

            dbContext.Projects.Update(findProject);
            await dbContext.SaveChangesAsync(); 
        }
        public async Task DeleteProject(DeleteProjectViewModel model)
        {
            var projectToDelete = await FindProject(model.Id);
            if (projectToDelete == null)
            {
                throw new NullReferenceException($"No project with id:{model.Id} exists.");
            }
            projectToDelete.IsDeleted = true;

            dbContext.Projects.Update(projectToDelete);
            await dbContext.SaveChangesAsync(); 
        }

        public IEnumerable<User> UsersToSelect()
        {
            IEnumerable<User> list = dbContext.Users.ToList();
            return list;
        }
        public async Task<User> FindUserIdByName(string name)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == name);
            if (user == null)
            {
                throw new NullReferenceException(); 
            }
            return user; 
        }
        public async Task AddProjectToUser(AddProjectToUserViewModel model)
        {
            if (model.ProjectId == null)
            {
                throw new NullReferenceException();
            }
            var newCombo = new UserProject();
            newCombo.ProjectId = model.ProjectId;
            newCombo.UserId = model.UserId;

            await dbContext.AddAsync(newCombo);
            await dbContext.SaveChangesAsync(); 
        }

        public async Task RemoveProjectToUser(AddProjectToUserViewModel model)
        {
            var removeCombo = await dbContext.UserProjects.FirstOrDefaultAsync(up => up.ProjectId == model.ProjectId && up.UserId == model.UserId);
            if (removeCombo == null)
            {
                throw new NullReferenceException($" Not found.");
            }
            dbContext.UserProjects.Remove(removeCombo);
            await dbContext.SaveChangesAsync(); 
        }

        public List<Ticket> DisplayAllProjectIncomingTickets(string id)
        {
            List<Ticket> tickets = new List<Ticket>();
            tickets = dbContext.Tickets.Where(t => t.ProjectId == id)
                .Include(t=>t.WorkingTimes)
                .Include(t=>t.FilesToUpload)
                .Include(t=>t.Agent)
                .Include(t=>t.Author)
                .ToList();

            return tickets; 
        }

        public List<User> DisplayAllProjectWorkers(string id)
        {
            List<User> workers = new List<User>();
            workers = dbContext.Users.Where(u=>u.Projects.Any(p=>p.ProjectId==id))
                .Include(u=>u.AnsweredTickets)
                .Include(u=>u.SendTickets)
                .Include(u=>u.Projects)
                .ToList();

            return workers; 
        }

    }
}
