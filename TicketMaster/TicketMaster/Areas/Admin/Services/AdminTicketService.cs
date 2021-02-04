using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TicketMaster.Areas.Admin.Services.Interfaces;
using TicketMaster.Areas.Admin.ViewModels.Ticket;
using TicketMaster.Data;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Services
{
    public class AdminTicketService : IAdminTicketService
    {
        private readonly TicketMasterDbContext dbContext;
        public AdminTicketService(TicketMasterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Ticket> DisplayAllSendTickets()
        {
            List<Ticket> AllTickets = dbContext.Tickets.Where(t => t.Priority != 0)
                .Include(t => t.Project)
                .Include(t => t.WorkingTimes)
                .Include(t => t.FilesToUpload)
                .Include(t => t.Author)
                .Include(t => t.Agent)
                .ToList();
            return AllTickets; 
        }
        public List<Ticket> DisplayAllAnswerTickets()
        {
            List<Ticket> AllTickets=dbContext.Tickets.Where(t=>t.Priority==0)
                .Include(t => t.Project)
                .Include(t => t.WorkingTimes)
                .Include(t => t.FilesToUpload)
                .Include(t => t.Author)
                .Include(t => t.Agent)
                .Include(t => t.ToReplyTicket)
                .ToList();
            return AllTickets; 
        }
        public async Task<Ticket> FindTicket(int id)
        {
            var ticket = await dbContext.Tickets.Include(t => t.Project)
                .Include(t => t.WorkingTimes)
                .Include(t => t.FilesToUpload)
                .Include(t => t.Author)
                .Include(t => t.Agent)
                .Include(t => t.ToReplyTicket).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No ticket with id:{id}");
            }
            return ticket; 
        }
        public async Task<Models.User> FindUserIdByName(string username)
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
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
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
            var defaultAgent = await FindUserIdByName("Admin");
            var ticket = new Ticket();
            ticket.AuthorId = model.AuthorId;
            ticket.Title = model.Title;
            ticket.Descripton = model.Descripton;
            ticket.SendOn = model.SendOn;
            ticket.Priority = model.Priority;
            ticket.ProjectId = model.ProjectId;
            ticket.AgentId = defaultAgent.Id; 

            await dbContext.Tickets.AddAsync(ticket);
            await dbContext.SaveChangesAsync(); 

        }

        public async Task AnswerTicket(AnswerTicketBindingModel model)
        {
            var sendTicked =await FindTicket(model.IdSendTicket);
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
            var ticket =await FindTicket(model.Id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No Ticket with id:{model.Id} exist.");
            }
            ticket.AgentId = model.AgentId;
            ticket.IsComplete = model.IsComplete;
            ticket.IsDeleted = model.IsDeleted;
            ticket.Priority = model.Priority;
            ticket.Descripton = model.Description;
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

        
        public List<Models.File> DisplayAllTicketFiles(int id)
        {
            var ticket = FindTicket(id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No ticket with id:{id} exists.");
            }
            List<Models.File> ticketFiles = dbContext.Files.Where(f => f.TicketId == id).ToList();
            return ticketFiles; 

        }
      
        public List<WorkingTime> DisplayAllWorkingTimes(int id)
        {
            var ticket = FindTicket(id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No ticket with id:{id} exists.");
            }
            List<Models.WorkingTime> ticketWorkingTimes = dbContext.WorkingTimes.Where(w => w.TicketId == id).ToList();
            return ticketWorkingTimes; 

        }

        public async Task<Ticket> DisplayTicketAnswerTicket(int id)
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
