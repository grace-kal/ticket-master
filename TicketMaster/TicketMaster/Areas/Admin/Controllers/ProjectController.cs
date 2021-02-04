using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TicketMaster.Areas.Admin.Services.Interfaces;
using TicketMaster.Areas.Admin.ViewModels.Project;
using TicketMaster.Data;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Controllers
{
    [Route("[Area]/[Controller]/[Action]")]
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly Random rand = new Random();
        private readonly IAdminProjectService service;
        public ProjectController(IAdminProjectService service,TicketMasterDbContext dbContext)
        {
            this.service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DisplayAllProjects()
        {
            var projects = new DisplayAllProjectsViewModel
            {
                Projects = service.DisplayAllProjects()
            };
            return View(projects); 
        }
        
        [HttpPost]
        public async Task<IActionResult> FindProject(string id)
        {
            if (id == null)
            {
                return NotFound(); 
            }
            var project = new FindProjectViewModel();
            var findproject = await service.FindProject(id);
            project.Id = findproject.Id;
            project.Title = findproject.Title;
            project.WorkTime = findproject.WorkTime;
            project.Description = findproject.Description;
            project.CompanyId = findproject.CompanyId;
            project.Workers = findproject.Workers;
            project.IncomingTickets = findproject.IncomingTickets;

            return View(project); 
        }

        public int RandomNumber(int min, int max)
        {
            return rand.Next(min ,max); 
        }

        [HttpGet]
        public async Task<IActionResult> CreateProject()
        {
            var newProject = new CreateProjectBindingModel();

            var idBuilder = new StringBuilder();
            idBuilder.Append(RandomNumber(1000,100000));
            
            newProject.Id = idBuilder.ToString(); 

            IEnumerable<Company> listOfCompanyId = service.CompaniesIdToSelect();
            ViewData["CompanyId"] = new SelectList (listOfCompanyId,"Id", "Id",newProject.CompanyId);

            return View(newProject);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(CreateProjectBindingModel model)
        {
            
            if (ModelState.IsValid)
            {
                await service.CreateProject(model);
                return RedirectToAction("DisplayAllProjects");
            }
            else
            { 
                //----for Error
               // ModelState.AddModelError(string.Empty, "The model you're passing is invalid. Please try again.");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProject(string id)
        {
            var foundProject = await service.FindProject(id);
            if (foundProject == null)
            {
                return NotFound(); 
            }
            var projectToEdit = new EditProjectBindingModel
            {
                Id = id,
                Title = foundProject.Title,
                Description = foundProject.Description,
                CompanyId = foundProject.CompanyId,
                IsDelete = foundProject.IsDeleted,
                WorkTime = foundProject.WorkTime
            };

            IEnumerable<Company> listOfCompanyId = service.CompaniesIdToSelect();
            ViewData["CompanyId"] = new SelectList(listOfCompanyId, "Id", "Id", projectToEdit.CompanyId);

            return View(projectToEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(EditProjectBindingModel model)
        {
            if (ModelState.IsValid)
            {
                await service.EditProject(model);
                return RedirectToAction("DisplayAllProjects");
            }
            return View(); 
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProject(string id)
        {
            var findToDelete = await service.FindProject(id);
            var projectToDelete = new DeleteProjectViewModel(); 
            if (findToDelete == null)
            {
                return NotFound(); 
            }
            else if (findToDelete.IsDeleted)
            {
                return RedirectToAction("DisplayAllProjects");
            }
            projectToDelete.Id = findToDelete.Id;
            projectToDelete.Title = findToDelete.Title;
            projectToDelete.Description = findToDelete.Description;
            projectToDelete.CompanyId = findToDelete.CompanyId;
            projectToDelete.WorkTime = findToDelete.WorkTime;
            projectToDelete.IsDelete = findToDelete.IsDeleted;
            return View(projectToDelete);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(DeleteProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await service.DeleteProject(model);
                return RedirectToAction("DisplayAllProjects");
            }
            return View(); 
        }

        [HttpGet]
        public async Task<IActionResult> AddProjectToUser(string id)
        {
            var findProject = await service.FindProject(id);
            if (findProject == null)
            {
                return NotFound();
            }
            var newCombo = new AddProjectToUserViewModel
            {
                ProjectId=id
            };

            IEnumerable<User> userIdToSelect = service.UsersToSelect();
            ViewBag.UserId = new SelectList(userIdToSelect, "UserName", "UserName", newCombo.UserId);

            return View(newCombo); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProjectToUser(AddProjectToUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = await service.FindUserIdByName(model.UserId);
                model.UserId = userId.Id; 
                await service.AddProjectToUser(model);
                return RedirectToAction("DisplayAllProjects");
            }
            return View(); 
        }
        [HttpGet]
        public async Task<IActionResult> RemoveProjectToUser(string id)
        {
            var findProject = await service.FindProject(id);
            if (findProject == null)
            {
                return NotFound();
            }
            var newCombo = new AddProjectToUserViewModel
            {
                ProjectId = id
            };

            IEnumerable<User> userIdToSelect = service.UsersToSelect();
            ViewBag.UserId = new SelectList(userIdToSelect, "UserName", "UserName", newCombo.UserId);

            return View(newCombo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProjectToUser(AddProjectToUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = await service.FindUserIdByName(model.UserId);
                model.UserId = userId.Id;
                await service.RemoveProjectToUser(model);
                return RedirectToAction("DisplayAllProjects");
            }
            return View();
        }


        [HttpGet]
        public IActionResult DisplayAllProjectIncomingTickets(string id)
        {
            var projectTickets = new DisplayAllProjectIncomingTicketsViewModel
            {
                IncomingTickets = service.DisplayAllProjectIncomingTickets(id)
            };

            return View(projectTickets); 
        }

        [HttpGet]
        public IActionResult DisplayAllProjectWorkers(string id)
        {
            var projectWorkers = new DisplayAllProjectWorkersViewModel
            {
                Workers = service.DisplayAllProjectWorkers(id)
            };
            return View(projectWorkers);
        }


    }
}
