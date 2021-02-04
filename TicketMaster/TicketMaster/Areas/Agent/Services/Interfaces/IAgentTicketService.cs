using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Areas.Admin.ViewModels.Ticket;
using TicketMaster.Models;

namespace TicketMaster.Areas.Agent.Services.Interfaces
{
    public interface IAgentTicketService
    {
        public Task<Ticket> FindTicket(int id);
        public Task<User> FindUserIdByName(string username);
        public IEnumerable<Models.Project> ProjectIdToSelect();
        public IEnumerable<Models.User> UserUsernameToSelect();
        public Task<User> FindUserNameById(string id);

        public Task CreateTicket(Admin.ViewModels.Ticket.CreateTicketBindingModel model);
        public Task AnswerTicket(AnswerTicketBindingModel model); 
        public Task EditTicket(Admin.ViewModels.Ticket.EditTicketBindingModel model);
        public Task DeleteTicket(DeleteTicketViewModel model);
        public Task<List<Ticket>> MySendTickets(string username);
        public Task<List<Ticket>> MyAnswerTickets(string username);
        public Task<List<WorkingTime>> WorkingTimes(int id);
        public Task< List<Models.File>>TicketFiles(int id);
        public Task<Ticket> TicketAnswerTicket(int id); 




    }
}
