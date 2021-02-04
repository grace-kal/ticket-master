using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TicketMaster.Models;
using TicketMaster.Services.Interfaces;

namespace TicketMaster.Controllers
{
    //[Route("[Controller]/[Action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUserHomeService _service;
        public HomeController(ILogger<HomeController> logger, IUserHomeService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task< IActionResult> Index()
        {
            string username; 

            if (!User.Identity.IsAuthenticated)
            {
                List<Ticket> empty = new List<Ticket>();
                var emptyList = new UserTicketsViewModel();
                emptyList.UserTickets = empty;
                return View(emptyList);
            }
            else
            {
                username = User.Identity.Name;
            }
            var user =await _service.FindUserIdByUserName(username); //Here i get a user. just checking if this user is valid, or anything else that needs a check. 
            if (user == null)
            {
                return RedirectToPage("Register");
            }
            var list = new UserTicketsViewModel
            {
                UserTickets = await _service.MySendTickets(username) 
            };
            return View(list); 

        }

        [HttpGet]
        public async Task<IActionResult> CreateTicket()//here the val i get User.Identity.Name
        {
            var user = await _service.FindUserIdByUserName(User.Identity.Name);
            var newTicket = new CreateTicketBindingModel
            {
                AuthorId = user.Id,
                Priority = Priority.Low
                
            };

            IEnumerable<Project> projects = _service.ProjectIdToSelect();
            ViewBag.ProjectId = new SelectList(projects,"Id","Id",newTicket.ProjectId);
            return View(newTicket); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTicket(CreateTicketBindingModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateTicket(model);
                return RedirectToAction("Index","Home"); 
            }
            IEnumerable<Project> projects = _service.ProjectIdToSelect();
            ViewBag.ProjectId = new SelectList(projects, "Id", "Id", model.ProjectId);
            return View(); 
        }

        [HttpGet]
        public async Task<IActionResult>EditTicket(int id)
        {
            var ticket = await _service.FindTicket(id);
            var toEdit = new EditTicketBindingModel();

            toEdit.Description = ticket.Descripton;
            toEdit.Id = ticket.Id;
            toEdit.ProjectId = ticket.ProjectId;
            toEdit.SendOn = ticket.SendOn;
            toEdit.Title = ticket.Title;

            IEnumerable<Project> projectId = _service.ProjectIdToSelect();
            ViewBag.ProjectId = new SelectList(projectId,"Id","Id",ticket.ProjectId); 
            return View(toEdit); 

        }
        [HttpPost]
        public async Task<IActionResult> EditTicket(EditTicketBindingModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.EditTicket(model);
                return RedirectToAction("Index","Home"); 
            }
            IEnumerable<Project> projectId = _service.ProjectIdToSelect();
            ViewBag.ProjectId = new SelectList(projectId, "Id", "Id", model.ProjectId);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _service.FindTicket(id);
            if (ticket.IsDeleted)
            {
                return RedirectToAction("Index", "Home"); 
            }
            var toDelete=new Ticket();
            toDelete.Title = ticket.Title;
            toDelete.Descripton = ticket.Descripton;
            toDelete.IsDeleted = ticket.IsDeleted;
            toDelete.SendOn = ticket.SendOn;
            toDelete.ProjectId = ticket.ProjectId; 
            return View(toDelete); 
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTicket(Ticket model)
        {
            if (ModelState.IsValid)
            {
                await _service.DeleteTicket(model);
                return RedirectToAction("Index","Home"); 
            }
            return View(); 
        }

        //remaining TicketFiles, TicketWorkingTimes


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        








    }
}
