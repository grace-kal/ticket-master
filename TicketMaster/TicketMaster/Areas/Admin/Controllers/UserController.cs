using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TicketMaster.Areas.Admin.Services.Interfaces;
using TicketMaster.Areas.Admin.ViewModels.User;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Controllers
{
    [Route("[Area]/[Controller]/[Action]")]
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IAdminUserService service;
        public UserController(IAdminUserService service)
        {
            this.service = service; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DisplayAllUsers()
        {
            var allUsers = new DisplayAllUsersViewModel
            {
                Users = service.DisplayAllUsers()
            };
            return View(allUsers);
        }
        public async Task< IActionResult> FindUser(string id)
        {
            if (id == null)
            {
                return NotFound(); 
            }
            var userToEdit = new FindUserViewModel(); 
            var findUser = await service.FindUser(id);
            userToEdit.Id = findUser.Id;
            userToEdit.Username = findUser.UserName;
            userToEdit.Email = findUser.Email;
            userToEdit.EmailConfirmed = findUser.EmailConfirmed;
            userToEdit.CompanyId = findUser.CompanyId;
            userToEdit.IsDeleted = findUser.IsDelete;
            userToEdit.IsGlobal = findUser.IsGlobal; 

            return View(findUser); 
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (id == null)
            {
                return NotFound(); 
            }
            var findUserToEdit =await service.FindUser(id); 
            var userToEdit = new EditUserBindingModel();
            //the only props of the User that can be edited will be the EmailConfirmed,IsGlobal,IsDeleted,CompanyId
            userToEdit.Id = findUserToEdit.Id;
            userToEdit.Username = findUserToEdit.UserName;
            userToEdit.Email = findUserToEdit.Email;
            userToEdit.EmailConfirmed = findUserToEdit.EmailConfirmed;
            userToEdit.CompanyId = findUserToEdit.CompanyId;
            userToEdit.IsDeleted = findUserToEdit.IsDelete;
            userToEdit.IsGlobal = findUserToEdit.IsGlobal;

            IEnumerable < Company > companiesIdToSelect= service.CompaniesIdToSelect();
            ViewData["CompanyId"] = new SelectList(companiesIdToSelect, "Id", "Id", userToEdit.CompanyId); 

            return View(userToEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserBindingModel model)
        {
            if (ModelState.IsValid)
            {
                await service.EditUser(model);
                return RedirectToAction("DisplayAllUsers");
            }
            else return View(); 
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var findUserToDelete = await service.FindUser(id);
            var deleteUser = new DeleteUserViewModel();
            if (findUserToDelete == null)
            {
                return NotFound(); 
            }
            else if (findUserToDelete.IsDelete)
            {
                return RedirectToAction("DisplayAllUsers");
            }
            deleteUser.Username = findUserToDelete.UserName;
            deleteUser.IsGlobal = findUserToDelete.IsGlobal;
            deleteUser.Email = findUserToDelete.Email;
            deleteUser.EmailConfirmed = findUserToDelete.EmailConfirmed;
            deleteUser.IsDeleted = findUserToDelete.IsDelete;
            deleteUser.CompanyId = findUserToDelete.CompanyId;

            return View(deleteUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(DeleteUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                await service.DeleteUser(model);
                return RedirectToAction("DisplayAllUsers");
            }
            else return View(); 
        }


        [HttpGet]
        public async Task<IActionResult> AddUserToProject(string id)
        {
            var findUser = await service.FindUser(id);
            if (findUser == null)
            {
                return NotFound(); 
            }
            ViewBag.UserName = findUser.UserName; 
            var newCombo = new AddUserToProjectViewModel
            {
                UserId = id
            };
            IEnumerable<Project> projectIdToSelect = service.ProjectIdToSelect();
            ViewData["ProjectId"] = new SelectList(projectIdToSelect, "Id", "Id", newCombo.ProjectId);


            return View(newCombo); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToProject(AddUserToProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await service.AddUserToProject(model);
                return RedirectToAction("DisplayAllUsers");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RemoveUserToProject(string id)
        {
            var findUser = await service.FindUser(id); 
            if (findUser == null)
            {
                return NotFound();
            }
            ViewBag.UserName = findUser.UserName;
            var newCombo = new AddUserToProjectViewModel
            {
                UserId = id
            };
            IEnumerable<Project> projectIdToSelect = service.ProjectIdToSelect();
            ViewData["ProjectId"] = new SelectList(projectIdToSelect, "Id", "Id", newCombo.ProjectId);
            return View(newCombo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserToProject(AddUserToProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                await service.RemoveUserToProject(model);
                return RedirectToAction("DisplayAllUsers");
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> DisplayAllUserAnsweredTickets(string id)
        {
            if (id == null)
            {
                return NotFound(); 
            }
            var user = await service.FindUser(id);
            ViewBag.AgentUserName = user.UserName;
            var answeredTickets = new DisplayAllUserAnsweredTicketsViewModel
            {
                AnsweredTickets = await service.DisplayAllUserAnsweredTickets(id)
            };
            return View(answeredTickets); 
        }
        [HttpGet]
        public async Task< IActionResult> DisplayAllUserSendTickets(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await service.FindUser(id);
            ViewBag.AuthorUserName = user.UserName; 

            var sendTickets = new DisplayAllUserSendTicketsViewModel
            {
                SendTickets= await service.DisplayAllUserSendTickets(id) 
            };
            return View(sendTickets); 
        }
        [HttpGet]
        public IActionResult DisplayAllUserProjects(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var projects = new DisplayAllUserProjectsViewModel
            {
                Projects = service.DisplayAllUserProjects(id)
            };
            return View(projects);
        }





    }
}
