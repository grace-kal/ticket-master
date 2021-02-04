using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketMaster.Areas.Agent.Services.Interfaces;
using TicketMaster.Areas.Agent.ViewModels.Project;

namespace TicketMaster.Areas.Agent.Controllers
{
    [Route("[Area]/[Controller]/[Action]")]
    [Authorize(Roles ="Admin,Agent")]
    [Area("Agent")]
    public class ProjectController : Controller
    {
        private readonly IAgentProjectService service;
        public ProjectController(IAgentProjectService service)
        {
            this.service = service; 
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> MyProjects(string id)//here i get User.Identity.Name
        {
            var user = service.FindUser(id);
            if (user == null)
            {
                return NotFound();
            }
            var list = new UserProjectsViewModel
            {
                UserProjects = await service.UserProjects(id) 
            };
            return View(list); 
        }
        public IActionResult ProjectWorkers(string id)//here i get the project id
        {
            var project = service.FindProject(id);
            if (project == null)
            {
                return NotFound();
            }
            var list = new ProjectWorkersViewModel
            {
                Workers = service.ProjectWorkers(id)
            };
            return View(list); 
        }
        public IActionResult  ProjectIncomingTickets(string id)
        {
            var project = service.FindProject(id);
            if (project == null)
            {
                return NotFound();
            }
            var list = new ProjectIncomingTicketsViewModel
            {
                IncomingTickets= service.ProjectIncomingTickets(id)
            };
            return View(list); 
        }

    }
}
