using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TicketMaster.Data;
using TicketMaster.Models;

namespace TicketMaster.Areas.Agent.Services.Interfaces
{
    public class AgentProjectService:IAgentProjectService
    {
        private readonly TicketMasterDbContext dbContext;
        public AgentProjectService(TicketMasterDbContext dbContext)
        {
            this.dbContext = dbContext; 
        }

        public async Task<Models.User> FindUser(string username)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new NullReferenceException($"No user with username:{username}");
            }
            return user; 
        }

        public async Task<Models.Project> FindProject(string id)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                throw new NullReferenceException($"No project with id:{id}");
            }
            return project; 
        }
        public async Task<List<Models.Project>> UserProjects(string id) //here the id belongs to the logged user ITS ACTUALLY User.Identity.NAME
        {
            //---Here i am calling awaitable method in an UNawaitable one so like shown below i can make the awaitable method work and get its result. This way works but
            //i decided to find the projects of the user through their username that i get as parameter not trough id that i can get by the .GetAwaiter and .GetResult---
            //var finduser = FindUser(id).GetAwaiter();
            //var user=finduser.GetResult();
            List<Project> allProjects = new List<Project>();
            allProjects = dbContext.Projects.Where(p => p.Workers.Any(w => w.Worker.UserName == id)).Include(p => p.IncomingTickets).Include(t => t.Workers).ToList();
            return allProjects;
        }
        public  List<Models.User> ProjectWorkers(string id)//here the id belongs to the project
        {
            var proj = FindProject(id);
            if (proj == null)
            {
               throw new NullReferenceException($"No project with id:{id}");
            }
            List<Models.User> list = dbContext.Users
                .Where(u => u.Projects.Any(p => p.ProjectId == id))
                .Include(u=>u.AnsweredTickets)
                .Include(u=>u.SendTickets)
                .Include(u=>u.Projects)
                .ToList();
            return list; 

        }
        public List<Models.Ticket> ProjectIncomingTickets(string id)
        {
            var proj = FindProject(id);
            if (proj == null)
            {
                throw new NullReferenceException($"No project with id:{id}");
            }
            List<Models.Ticket> list = dbContext.Tickets
                .Where(t => t.ProjectId == id)
                .Include(t=>t.Agent)
                .Include(t=>t.Author)
                .Include(t=>t.FilesToUpload)
                .Include(t=>t.WorkingTimes)
                .Include(t=>t.ToReplyTicket)
                .Include(t=>t.Project)
                .ToList();
            return list; 
        }



    }
}
