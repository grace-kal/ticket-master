using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Areas.Admin.ViewModels.User;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Services.Interfaces
{
    public interface IAdminUserService
    {
        public List<User> DisplayAllUsers();
        public Task<Models.User> FindUser(string id);
        //public Task<Models.User> FindUserNameById(string id);
        //public Task<Models.User> FindIdByUserName(string id); 
        public Task EditUser(EditUserBindingModel model);
        public IEnumerable<Company> CompaniesIdToSelect(); 
        public Task DeleteUser( DeleteUserViewModel model);

        public Task AddUserToProject(AddUserToProjectViewModel model);
        public Task RemoveUserToProject(AddUserToProjectViewModel model);
        public IEnumerable<Project> ProjectIdToSelect(); 

        public Task<List<Ticket>> DisplayAllUserAnsweredTickets(string id);
        public Task< List<Ticket>> DisplayAllUserSendTickets(string id);
        public List<Project> DisplayAllUserProjects(string id); 
    }
}
