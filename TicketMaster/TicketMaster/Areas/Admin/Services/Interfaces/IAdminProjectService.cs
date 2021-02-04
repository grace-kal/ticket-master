using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Areas.Admin.ViewModels.Project;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Services.Interfaces
{
     public interface IAdminProjectService
    {
        public List<Project> DisplayAllProjects();
        public Task<Project> FindProject(string id);

        public Task CreateProject(CreateProjectBindingModel model);
        public IEnumerable<Company> CompaniesIdToSelect();
        public Task EditProject(EditProjectBindingModel model);
        public Task DeleteProject(DeleteProjectViewModel model);

        public IEnumerable<User> UsersToSelect(); 
        public Task AddProjectToUser(AddProjectToUserViewModel model);
        public Task RemoveProjectToUser(AddProjectToUserViewModel model); 
        public Task<User> FindUserIdByName(string name); 
        public List<Ticket> DisplayAllProjectIncomingTickets(string id);
        public List<User> DisplayAllProjectWorkers(string id);
        
    }
}
