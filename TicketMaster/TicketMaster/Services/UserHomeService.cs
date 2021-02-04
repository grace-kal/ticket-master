using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TicketMaster.Data;
using TicketMaster.Models;
using TicketMaster.Services.Interfaces;

namespace TicketMaster.Services
{
    public class UserHomeService : IUserHomeService
    {
        private readonly TicketMasterDbContext _dbContext;
        public UserHomeService(TicketMasterDbContext dbContext)
        {
            _dbContext = dbContext; 
        }
        public async Task<List<Ticket>> MySendTickets(string username)
        {
            List<Ticket>list = await _dbContext.Tickets
                .Include(t=>t.Agent)
                .Include(t=>t.Author)
                .Include(t=>t.WorkingTimes)
                .Include(t=>t.FilesToUpload)
                .Include(t=>t.ToReplyTicket)
                .Where(t => t.Author.UserName == username && t.Priority!=0)
                .ToListAsync();
            return list; 
        }


        public IEnumerable<Project> ProjectIdToSelect()
        {
            IEnumerable < Project >list= _dbContext.Projects.ToList();
            return list; 
        }
        public async Task CreateTicket(CreateTicketBindingModel model)
        {
            var newTicket = new Ticket();
            //Here i give default values on the Priority and Agent because these are props only accessed by Admin
            
            string username = "Admin"; 
            var agent = await FindUserIdByUserName(username);
            newTicket.AgentId = agent.Id;

            newTicket.Title = model.Title;
            newTicket.Priority = model.Priority;
            newTicket.Descripton = model.Description;
            newTicket.ProjectId = model.ProjectId;
            newTicket.AuthorId = model.AuthorId;
            newTicket.SendOn = model.SendOn;

            await _dbContext.Tickets.AddAsync(newTicket);
            await _dbContext.SaveChangesAsync(); 

        }
        public async Task EditTicket(EditTicketBindingModel model)
        {

            var ticket = await FindTicket(model.Id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No Ticket with id:{model.Id} exist.");
            }
            ticket.Descripton = model.Description;
            ticket.ProjectId = model.ProjectId;

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

                            await _dbContext.Files.AddAsync(fileToUpload);
                            await _dbContext.SaveChangesAsync();

                        }
                    }
                }
            }

            _dbContext.Tickets.Update(ticket);
            await _dbContext.SaveChangesAsync();


        }
        public async Task DeleteTicket(Ticket model)
        {
            var ticket = await FindTicket(model.Id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No ticket with id:{model.Id}.");
            }
            ticket.IsDeleted = true;
            _dbContext.Tickets.Update(ticket);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<Ticket> FindTicket(int id)
        {
            var ticket = await _dbContext.Tickets
                .Include(t => t.Agent)
                .Include(t => t.Author)
                .Include(t => t.WorkingTimes)
                .Include(t => t.FilesToUpload)
                .Include(t => t.ToReplyTicket)
                .Include(t=>t.Project)
                .FirstOrDefaultAsync(t=>t.Id==id);
            if (ticket == null)
            {
                throw new NullReferenceException($"No ticket with id:{id} exists."); 
            }
            return ticket; 
        }

        public async Task<User> FindUserIdByUserName(string username)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new NullReferenceException($"No user with username:{username}");
            }
            return user; 
        }

        public async Task<User> FindUserNameByUserId(string id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NullReferenceException($"No user with id:{id}");
            }
            return user;
        }


        public async Task<Ticket> TicketAnswerTicket(int id)
        {
            var ticket = await FindTicket(id); 
            if(ticket.ToReplyTicket==null)
            {
                throw new NullReferenceException($"Ticket with id:{id} is not yet answered."); 
            }
            var answerTicket = await FindTicket(ticket.ToReplyTicket.Id);
            return answerTicket; 

        }

        public async Task<List<Models.File>> TicketFiles(int id)
        {
            var ticket = await FindTicket(id);
            List<Models.File> list = await _dbContext.Files.Where(f => f.TicketId == id).ToListAsync();
            return list; 
        }

        public async Task<List<WorkingTime>> TicketWorkingTimes(int id)
        {
            var ticket = await FindTicket(id);
            List<WorkingTime> list = await _dbContext.WorkingTimes.Where(f => f.TicketId == id).ToListAsync();
            return list;
        }

    }
}
