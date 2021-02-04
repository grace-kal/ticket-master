using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TicketMaster.Areas.Admin.Services.Interfaces;
using TicketMaster.Areas.Admin.ViewModels.Ticket;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Controllers
{

    [Route("[Area]/[Controller]/[Action]")]
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class TicketController : Controller
    {
        private readonly IAdminTicketService service;
        public TicketController(IAdminTicketService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
         public IActionResult DisplayAllSendTickets()
         {
            var allTickets = new DisplayAllTicketsViewModel
            {
                AllTickets=service.DisplayAllSendTickets()
            };

            return View(allTickets);
         }
        [HttpGet]
        public IActionResult DisplayAllAnswerTickets()
        {
            var allTickets = new DisplayAllTicketsViewModel
            {
                AllTickets = service.DisplayAllAnswerTickets()
            };
            return View(allTickets); 
        }

        [HttpGet]
        public async Task< IActionResult> FindTicket(int id)
        {
            var findTicket = new FindTicketViewModel(); 
            var foundTicket = await service.FindTicket(id);
            findTicket.Id = foundTicket.Id;
            findTicket.Title = foundTicket.Title;
            findTicket.Descripton = foundTicket.Descripton;
            findTicket.SendOn = foundTicket.SendOn;
            findTicket.ProjectId = foundTicket.ProjectId;
            findTicket.TicketId = foundTicket.TicketId;
            findTicket.Priority = foundTicket.Priority;
            findTicket.IsDeleted = foundTicket.IsDeleted;
            findTicket.IsComplete = foundTicket.IsComplete;
            findTicket.IsAnswered = foundTicket.IsAnswered;
            findTicket.AgentId = foundTicket.AgentId;
            findTicket.AuthorId = foundTicket.AuthorId;

            return View(findTicket); 

        }

        [HttpGet]
        public async Task<IActionResult> CreateTicket(string id)
        {
            //---here the string I am receiving is named {id} but the value that is contained is User.Identity.Name which gives me the username of the curently logged user
            //to find the id of the user that I need because a {Ticket}{AuthorId} has to be asigned Id
            //---here {User.Identity.Name} is named id because when I'm routing data from the View for it to be successful i have to use
            //the same name in {asp-route-ID=""} and this ID is because of the routing in the startup {area=""}{controller="Home"}{action="Index"}{id=?}(and respectfuly for all defined areas)
            // ViewBag.User = id;//if i want display the value i got but here is not needed because i asign the value to the AuthorId below and if i want i will display that in the View
            
            var getId = await service.FindUserIdByName(id);

            var ticket = new Admin.ViewModels.Ticket.CreateTicketBindingModel
            {
                AuthorId = getId.Id
            };

            IEnumerable<Models.Project> projectIdToSelect = service.ProjectIdToSelect();
            ViewData["ProjectId"] = new SelectList(projectIdToSelect,"Id","Id",ticket.ProjectId);
            
            return View(ticket); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTicket(Admin.ViewModels.Ticket.CreateTicketBindingModel model)
        {
            
            if (ModelState.IsValid)
            {
                await service.CreateTicket(model);
                return RedirectToAction("DisplayAllSendTickets");
            }
            IEnumerable<Models.Project> projectIdToSelect = service.ProjectIdToSelect();
            ViewData["ProjectId"] = new SelectList(projectIdToSelect, "Id", "Id", model.ProjectId);
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
                return RedirectToAction("DisplayAllSendTickets"); 
            }
            
            var answer = new AnswerTicketBindingModel();
            answer.IdSendTicket = ticket.Id;
            answer.Title = ticket.Title;
            if (ticket.AgentId !=null)
            {
                var AgentUsername = await service.FindUserNameById(ticket.AgentId);
                answer.AgentId = AgentUsername.UserName;
            }
            else
            {
                return RedirectToAction("DisplayAllSendTickets");
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
                return RedirectToAction("DisplayAllSendTickets");
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
            ViewBag.AgentUsername = new SelectList(userUsernameToSelect,"UserName","UserName",ticketToEdit.AgentId);
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
                return RedirectToAction("DisplayAllSendTickets");
            }
            IEnumerable<Models.User> userUsernameToSelect = service.UserUsernameToSelect();
            IEnumerable<Models.Project> projectIdToSelect = service.ProjectIdToSelect();

            //here i will get username as a value and in the post action i will find by the username the id of the user
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
                return RedirectToAction("DisplayAllSendTickets");
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
                return RedirectToAction("DisplayAllSendTickets");
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
                return RedirectToAction("DisplayAllSendTickets");
            }
            return View();
        }


        [HttpGet]
        public IActionResult DisplayAllTicketFiles(int id)
        {
            var ticket = service.FindTicket(id);
            if (ticket == null)
            {
                return NotFound(); 
            }
            var allFiles = new DisplayAllTicketFilesViewModel
            {
                Files = service.DisplayAllTicketFiles(id)
            };
            return View(allFiles); 
        }
        [HttpGet]
        public IActionResult DisplayAllTicketWorkingTimes(int id)
        {
            var ticket = service.FindTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            var allWorkingTimes = new DisplayAllTicketWorkingTimesViewModel
            {
                WorkingTimes = service.DisplayAllWorkingTimes(id) 
            };
            return View(allWorkingTimes); 
        }

        [HttpGet]
        public async Task<IActionResult> DisplayTicketAnswerTicket(int id)
        {
            var ticket = await service.FindTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            if (ticket.ToReplyTicket == null)
            {
                string message = "The requested ticket is not yet answered!";
                ViewBag.NotAnswered = message;
                return View();
            }
            var answer = await service.DisplayTicketAnswerTicket(id);

            var dispAnswer = new FindTicketViewModel();
            dispAnswer.Id = answer.Id;
            dispAnswer.TicketId = answer.TicketId; 
            dispAnswer.SendOn = answer.SendOn;
            dispAnswer.Descripton = answer.Descripton;
            dispAnswer.AgentId = answer.Agent.UserName;//here to dislay the UserName and not he Id I just give the value UserName to the prop Id 
            dispAnswer.AuthorId = answer.Author.UserName;
            return View(answer); 

        }


    }
}
