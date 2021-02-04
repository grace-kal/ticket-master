using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Areas.Admin.ViewModels.Ticket;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Services.Interfaces
{
    public interface IAdminTicketService
    {
        public List<Models.Ticket> DisplayAllSendTickets();
        public List<Models.Ticket> DisplayAllAnswerTickets(); 
        public Task<Models.Ticket> FindTicket(int id);

        public IEnumerable<Models.User> UserUsernameToSelect(); 
        public IEnumerable<Models.Project> ProjectIdToSelect();
        public Task<Models.User> FindUserIdByName(string username);
        public Task<Models.User> FindUserNameById(string id);

        public Task CreateTicket(Admin.ViewModels.Ticket.CreateTicketBindingModel model);
        public Task AnswerTicket(AnswerTicketBindingModel model);
        public Task EditTicket(Admin.ViewModels.Ticket.EditTicketBindingModel model); 
        public Task DeleteTicket(DeleteTicketViewModel model);

        public List<File> DisplayAllTicketFiles(int id);
        public List<WorkingTime> DisplayAllWorkingTimes(int id);
        public  Task<Ticket> DisplayTicketAnswerTicket(int id); 





    }
}
