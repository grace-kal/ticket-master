using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TicketMaster.Areas.Admin.ViewModels.Ticket;
using TicketMaster.Areas.Agent.Services.Interfaces;
using TicketMaster.Data;
using TicketMaster.Models;

namespace TicketMaster.Areas.Agent.Services
{
    public class AgentTicketService : IAgentTicketService
    {
        private readonly TicketMasterDbContext dbContext;
        public AgentTicketService(TicketMasterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Ticket> FindTicket(int id)
        {
            var ticket = await dbContext.Tickets
                .Include(t => t.WorkingTimes)
                .Include(t => t.FilesToUpload)
                .Include(t => t.Author)
                .Include(t => t.Agent)
                .Include(t => t.ToReplyTicket)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No ticket with id:{id} exists."); 
            }
            return ticket; 

        }
        public async Task<User> FindUserIdByName(string username)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new NullReferenceException($"No user with username: {username} exists.");
            }
            return user; 
        }
        public async Task<User> FindUserNameById(string id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id== id);
            if (user == null)
            {
                throw new NullReferenceException($"No user with id: {id} exists.");
            }
            return user;
        }
        public IEnumerable<Models.Project> ProjectIdToSelect()
        {
            IEnumerable<Models.Project> list = dbContext.Projects.ToList();
            return list;
        }
        public IEnumerable<Models.User> UserUsernameToSelect()
        {
            IEnumerable<Models.User> list = dbContext.Users.ToList();
            return list;
        }
        public async Task CreateTicket(Admin.ViewModels.Ticket.CreateTicketBindingModel model)
        {
            var ticket = new Ticket();
            if (model.AgentId !=null)
            {
                ticket.AgentId = model.AgentId;
            }
            else
            {
                string username = "Admin";
                var defaultAgent = await FindUserIdByName(username);
                ticket.AgentId = defaultAgent.Id;
            }
            
            
            ticket.AuthorId = model.AuthorId;
            ticket.Title = model.Title;
            ticket.Descripton = model.Descripton;
            ticket.SendOn = model.SendOn;
            ticket.Priority = model.Priority;
            ticket.ProjectId = model.ProjectId;

            await dbContext.Tickets.AddAsync(ticket);
            await dbContext.SaveChangesAsync();
        }
        public async Task AnswerTicket(AnswerTicketBindingModel model)
        {
            var sendTicked = await FindTicket(model.IdSendTicket);
            var answerTicket = new Ticket();
            answerTicket.Title = model.Title;
            answerTicket.AgentId = model.AgentId;
            answerTicket.AuthorId = model.AuthorId;
            answerTicket.Descripton = model.Description;
            answerTicket.SendOn = model.SendOn;
            answerTicket.TicketId = model.IdSendTicket;

            sendTicked.IsAnswered = true;
            sendTicked.ToReplyTicket = answerTicket;

            dbContext.Tickets.Update(sendTicked);
            await dbContext.Tickets.AddAsync(answerTicket);
            await dbContext.SaveChangesAsync();
        }
        public async Task EditTicket(Admin.ViewModels.Ticket.EditTicketBindingModel model)
        {
            var ticket = await FindTicket(model.Id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No Ticket with id:{model.Id} exist.");
            }
            ticket.AgentId = model.AgentId;
            ticket.IsComplete = model.IsComplete;
            ticket.IsDeleted = model.IsDeleted;
            ticket.Descripton = model.Description; 
            ticket.Priority = model.Priority;
            ticket.ProjectId = model.ProjectId;

            if (model.WorkingTime != null)
            {
                var wt = new WorkingTime();
                wt.WorkingSpan = model.WorkingTime;
                wt.TicketId = model.Id;

                await dbContext.WorkingTimes.AddAsync(wt);
                await dbContext.SaveChangesAsync();
            }

            if (model.FilesToUpload != null)
            {
                foreach (var file in model.FilesToUpload)
                {
                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var fileExtension = Path.GetExtension(fileName);
                            var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                            var fileToUpload = new Models.File
                            {
                                TicketId = model.Id
                            };
                            using (var target = new MemoryStream())
                            {
                                file.CopyTo(target);
                                fileToUpload.FileUpload = target.ToArray();
                            }

                            await dbContext.Files.AddAsync(fileToUpload);
                            await dbContext.SaveChangesAsync();

                        }
                    }
                }
            }

            dbContext.Tickets.Update(ticket);
            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteTicket(DeleteTicketViewModel model)
        {
            var ticketToDelete = await FindTicket(model.Id);
            if (ticketToDelete == null)
            {
                throw new NullReferenceException($"No ticket with id:{model.Id} exists.");
            }
            ticketToDelete.IsDeleted = true;

            dbContext.Update(ticketToDelete);
            await dbContext.SaveChangesAsync();
        }





        public async Task<List<Ticket>> MySendTickets(string username)
        {
            var user = await FindUserIdByName(username);
            List<Ticket> list = await dbContext.Tickets
                .Where(t => t.AuthorId == user.Id && t.Priority!=0)
                .Include(t=>t.Agent)
                .Include(t=>t.FilesToUpload)
                .Include(t=>t.WorkingTimes)
                .Include(t=>t.ToReplyTicket)
                .ToListAsync();
            return list; 
        }
        public async Task<List<Ticket>> MyAnswerTickets(string username)
        {
            var user = await FindUserIdByName(username);
            List<Ticket> list = await dbContext.Tickets
                .Where(t => t.AgentId == user.Id)
                .Include(t => t.Author)
                .Include(t => t.FilesToUpload)
                .Include(t => t.WorkingTimes)
                .Include(t => t.ToReplyTicket)
                .ToListAsync();
            return list;
        }

        public async Task< List<WorkingTime>> WorkingTimes(int id)
        {
            var ticket = await FindTicket(id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No ticket with id:{id} exists.");
            }
            List<Models.WorkingTime> ticketWorkingTimes = await dbContext.WorkingTimes.Where(w => w.TicketId == id).ToListAsync();
            return ticketWorkingTimes;

        }
        public async Task<List<Models.File>> TicketFiles(int id)
        {
            var ticket = await FindTicket(id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No ticket with id:{id} exists.");
            }
            List<Models.File> ticketFiles = await dbContext.Files.Where(f => f.TicketId == id).ToListAsync();
            return ticketFiles;

        }

        public async Task<Ticket> TicketAnswerTicket(int id)
        {
            var ticket = await FindTicket(id);

            if (ticket.ToReplyTicket == null)
            {
                throw new NullReferenceException("This ticket is not yet answered.");
            }
            var answer = await dbContext.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticket.Id);

            return answer;
        }

    }
}
