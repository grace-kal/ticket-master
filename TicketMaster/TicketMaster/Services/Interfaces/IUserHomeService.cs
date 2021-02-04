using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Models;

namespace TicketMaster.Services.Interfaces
{
    public interface IUserHomeService
    {

        public Task<List<Models.Ticket>> MySendTickets(string username);
        public Task<Ticket> FindTicket(int id);
        public Task<Models.User> FindUserIdByUserName(string username);
        public Task<Models.User> FindUserNameByUserId(string id);

        IEnumerable<Models.Project> ProjectIdToSelect(); 
        public Task CreateTicket(Models.CreateTicketBindingModel model); 
        public Task EditTicket(Models.EditTicketBindingModel model);
        public Task DeleteTicket(Ticket model);

        public Task<List<Models.WorkingTime>> TicketWorkingTimes(int id);
        public Task<List<Models.File>> TicketFiles(int id);
        public Task<Ticket> TicketAnswerTicket(int id);




    }
}
