using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Models;

namespace TicketMaster.Areas.Agent.Services.Interfaces
{
    public interface IAgentProjectService
    {
        public Task<Project> FindProject(string id);
        public Task<Models.User> FindUser(string id); 
        public Task<List<Models.Project>> UserProjects(string id);//here the id belongs to the logged user
        public List<Models.User> ProjectWorkers(string id);//here the id belongs to the project
        public List<Models.Ticket> ProjectIncomingTickets(string id);

    }
}
