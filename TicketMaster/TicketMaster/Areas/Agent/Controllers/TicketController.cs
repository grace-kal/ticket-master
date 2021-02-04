using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketMaster.Areas.Admin.ViewModels.Ticket;
using TicketMaster.Areas.Agent.Services.Interfaces;
using TicketMaster.Areas.Agent.ViewModels.Ticket;
using TicketMaster.Models;

namespace TicketMaster.Areas.Agent.Controllers
{
    [Route("[Area]/[Controller]/[Action]")]
    [Authorize(Roles = "Admin,Agent")]
    [Area("Agent")]
    public class TicketController : Controller
    {
        private readonly IAgentTicketService service;
        public TicketController(IAgentTicketService service)
        {
            this.service = service; 
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MySendTickets(string id)//here i get User.Identity.Name
        {
            var list = new TicketMaster.Areas.Agent.ViewModels.Ticket.UserTicketsViewModel
            {
                Tickets = await service.MySendTickets(id)
            };
            return View(list); 
        }
        [HttpGet]
        public async Task<IActionResult> MyAnswerTickets(string id)//here i get User.Identity.Name
        {
            var list = new TicketMaster.Areas.Agent.ViewModels.Ticket.UserTicketsViewModel
            {
                Tickets = await service.MyAnswerTickets(id)
            };
            
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTicket(string id)//here i get User.Identity.Name
        {
            var user = await service.FindUserIdByName(id);
            var ticket = new Admin.ViewModels.Ticket.CreateTicketBindingModel
            {
                AuthorId = user.Id
            };
            IEnumerable<Models.Project> list = service.ProjectIdToSelect();
            ViewBag.ProjectId = new SelectList(list, "Id", "Id", ticket.ProjectId);
            return View(ticket); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTicket(Admin.ViewModels.Ticket.CreateTicketBindingModel model)
        {
            if (ModelState.IsValid)
            {
                await service.CreateTicket(model);
                //---code below works to send a message if operation succeded if that is the wanted effect
                //ViewBag.Success = "You created a new ticket!"; 
                //return View(); 
                return RedirectToAction("MySendTickets", "Ticket", new { id = User.Identity.Name, area = "Agent" });

            }
            IEnumerable<Models.Project> list = service.ProjectIdToSelect();
            ViewBag.ProjectId = new SelectList(list, "Id", "Id", model.ProjectId);
            return View(); 

        }

        [HttpGet]
        public async Task<IActionResult> AnswerTicket(int id)
        {
            var ticket = await service.FindTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (ticket.IsAnswered)
            {
                return RedirectToAction("MyAnswerTickets", "Ticket", new { id = User.Identity.Name, area = "Agent" });
            }

            var answer = new AnswerTicketBindingModel();
            answer.IdSendTicket = ticket.Id;
            answer.Title = ticket.Title;
            if (ticket.AgentId != null)
            {
                var AgentUsername = await service.FindUserNameById(ticket.AgentId);
                answer.AgentId = AgentUsername.UserName;
            }
            else
            {
                return RedirectToAction("MyAnswerTickets", "Ticket", new { id=User.Identity.Name, area = "Agent" });
            }
            var AuthorUsername = await service.FindUserNameById(ticket.AuthorId);
            answer.AuthorId = AuthorUsername.UserName;
            return View(answer);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerTicket(AnswerTicketBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var AgentId = await service.FindUserIdByName(model.AgentId);
                model.AgentId = AgentId.Id;
                var AuthorId = await service.FindUserIdByName(model.AuthorId);
                model.AuthorId = AuthorId.Id;

                await service.AnswerTicket(model);
                return RedirectToAction("MyAnswerTickets", "Ticket", new { id = User.Identity.Name, area = "Agent" });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditTicket(int id)
        {
            var ticket = await service.FindTicket(id);
            var ticketToEdit = new Admin.ViewModels.Ticket.EditTicketBindingModel();
            if (ticket == null)
            {
                return NotFound();
            }
            //---the only fields that can be edited are AgentId,Priority,TicketId,IsAnswered,IsComplete,IsDeleted,ProjectId,UploadFiles,Descripton
            ticketToEdit.Id = ticket.Id;
            ticketToEdit.Title = ticket.Title;
            ticketToEdit.Description = ticket.Descripton;
            ticketToEdit.ProjectId = ticket.ProjectId;
            var authorUsername = await service.FindUserNameById(ticket.AuthorId);
            ticketToEdit.AuthorId = authorUsername.UserName;
            ticketToEdit.AgentId = ticket.AgentId;
            if (ticketToEdit.Priority == 0)
            {
                ticketToEdit.Priority = 0; 
            }
            else
            {
                ticketToEdit.Priority = ticket.Priority;
            }
            ticketToEdit.SendOn = ticket.SendOn;
            //ticketToEdit.TicketId = ticket.TicketId;
            //ticketToEdit.IsAnswered = ticket.IsAnswered;
            ticketToEdit.IsComplete = ticket.IsComplete;
            ticketToEdit.IsDeleted = ticket.IsDeleted;

            IEnumerable<Models.User> userUsernameToSelect = service.UserUsernameToSelect();
            IEnumerable<Models.Project> projectIdToSelect = service.ProjectIdToSelect();

            //here i will get username as a value and in the post action i will find by the username the id of the user
            //----
            ViewBag.AgentUsername = new SelectList(userUsernameToSelect, "UserName", "UserName", ticketToEdit.AgentId);
            ViewBag.ProjectId = new SelectList(projectIdToSelect, "Id", "Id", ticketToEdit.ProjectId);

            return View(ticketToEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTicket(Admin.ViewModels.Ticket.EditTicketBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var ticket = await service.FindTicket(model.Id);
                if (ticket.Priority == 0)
                {
                    model.Priority = 0;
                }
                var AgentId = await service.FindUserIdByName(model.AgentId);
                model.AgentId = AgentId.Id;
                var AuthorId = await service.FindUserIdByName(model.AuthorId);
                model.AuthorId = AuthorId.Id;
                await service.EditTicket(model);
                return RedirectToAction("MySendTickets","Ticket", new {  id=User.Identity.Name, area="Agent"});
            }
            IEnumerable<Models.User> userUsernameToSelect = service.UserUsernameToSelect();
            IEnumerable<Models.Project> projectIdToSelect = service.ProjectIdToSelect();

            //here i will get username as a value and in the post action i will find by the username the id of the user
            //----
            ViewBag.AgentUsername = new SelectList(userUsernameToSelect, "UserName", "UserName", model.AgentId);
            ViewBag.ProjectId = new SelectList(projectIdToSelect, "Id", "Id", model.ProjectId);
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await service.FindTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (ticket.IsDeleted)
            {
                return RedirectToAction("MySendTickets", "Ticket", new { id = User.Identity.Name, area = "Agent" });
            }
            var ticketToDelete = new DeleteTicketViewModel();
            ticketToDelete.Id = ticket.Id;
            ticketToDelete.Title = ticket.Title;
            ticketToDelete.Descripton = ticket.Descripton;
            ticketToDelete.Priority = ticket.Priority;
            if (ticket.AgentId != null)
            {
                var AgentUsername = await service.FindUserNameById(ticket.AgentId);
                ticketToDelete.AgentId = AgentUsername.UserName;
            }
            else
            {
                return RedirectToAction("MySendTickets", "Ticket", new { id = User.Identity.Name, area = "Agent" });
            }
            var AuthorUsername = await service.FindUserNameById(ticket.AuthorId);
            ticketToDelete.AuthorId = AuthorUsername.UserName;
            ticketToDelete.ProjectId = ticket.ProjectId;
            ticketToDelete.SendOn = ticket.SendOn;
            ticketToDelete.IsAnswered = ticket.IsAnswered;
            ticketToDelete.IsComplete = ticket.IsComplete;
            ticketToDelete.IsDeleted = ticket.IsDeleted;

            return View(ticketToDelete);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTicket(DeleteTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var AuthorId = await service.FindUserIdByName(model.AuthorId);
                model.AuthorId = AuthorId.Id;
                var AgentId = await service.FindUserIdByName(model.AgentId);
                model.AgentId = AgentId.Id;

                await service.DeleteTicket(model);
                return RedirectToAction("MySendTickets", "Ticket", new { id = User.Identity.Name, area = "Agent" });
            }
            return View();
        }


        [HttpGet]
        public async Task< IActionResult> TicketWorkingTimes(int id)
        {
            var ticket =await service.FindTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            var allWorkingTimes = new DisplayAllTicketWorkingTimesViewModel
            {
                WorkingTimes =await service.WorkingTimes(id)
            };
            return View(allWorkingTimes);
        }
        [HttpGet]
        public async Task< IActionResult> TicketFiles(int id)
        {
            var ticket =await service.FindTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            var allFiles = new DisplayAllTicketFilesViewModel
            {
                Files =await service.TicketFiles(id)
            };
            return View(allFiles);
        }

        [HttpGet]
        public async Task<IActionResult> TicketAnswerTicket(int id)
        {
            var ticket = await service.FindTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (ticket.ToReplyTicket == null)
            {
                string message= "The requested ticket is not yet answered!";
                ViewBag.NotAnswered = message;
                return View(); 
                //return RedirectToAction("MySendTickets","Ticket",new{id=User.Identity.Name , area="Agent" });
            }
            var answer = await service.TicketAnswerTicket(id);

            var dispAnswer = new FindTicketViewModel();
            dispAnswer.Id = answer.Id;
            dispAnswer.TicketId = answer.TicketId;
            dispAnswer.SendOn = answer.SendOn;
            dispAnswer.Descripton = answer.Descripton;
            dispAnswer.AgentId = answer.Agent.UserName;//here to display the UserName and not he Id I just give the value UserName to the prop Id 
            dispAnswer.AuthorId = answer.Author.UserName;
            return View(answer);

        }

    }
}
