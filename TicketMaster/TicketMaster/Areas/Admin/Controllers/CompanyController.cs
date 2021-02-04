using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketMaster.Areas.Admin.Services.Interfaces;
using TicketMaster.Areas.Admin.ViewModels.Company;
using TicketMaster.Models;

namespace TicketMaster.Areas.Admin.Controllers
{
    [Route("[Area]/[Controller]/[Action]")]
    [Authorize(Roles = "Admin")]
    [Area("Admin")]

    public class CompanyController : Controller
    {
        private readonly IAdminCompanyService service;
        public CompanyController(IAdminCompanyService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpGet]
        public IActionResult DisplayAllCompanies()
        {
            var allCompanies = new DisplayAllCompaniesViewModel()
            {
                Companies = service.DisplayAllCompanies()
            };
            return View(allCompanies);
        }
        
        [HttpGet]
        public async Task<IActionResult> FindCompany(int id)
        {
            var findcompany = await service.FindCompany(id);
            var foundcompany = new FindCompanyViewModel();
            foundcompany.Id = findcompany.Id;
            foundcompany.Name = findcompany.Name;
            foundcompany.IsDeleted = findcompany.IsDeleted;
            foundcompany.Projects = findcompany.Projects;
            foundcompany.Workers = findcompany.Workers;
            return View(foundcompany);
        }

        
        [HttpGet]
        public async Task<IActionResult> CreateCompany()
        {
            var newCompany = new CreateCompanyBindingModel(); 
            return View(newCompany); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCompany(CreateCompanyBindingModel model)
        {
            if (ModelState.IsValid)
            {
                await service.CreateCompany(model);
                return Redirect("DisplayAllCompanies");
            }
            else return View(); 
        }

        [HttpGet]
        public async Task<IActionResult> EditCompany( int id)
        {
            var foundCompany = await service.FindCompany(id);
            if (foundCompany == null)
            {
                return NotFound(); 
            }
            var companyToEdit = new EditCompanyBindingModel();
            companyToEdit.Id = foundCompany.Id;
            companyToEdit.Name = foundCompany.Name;
            companyToEdit.IsDeleted = foundCompany.IsDeleted;
            return View(companyToEdit); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCompany(EditCompanyBindingModel model)
        {
            if (ModelState.IsValid)
            {
                await service.EditCompany(model);
                return RedirectToAction("DisplayAllCompanies");
            }
            else return View(); 
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var foundCompany = await service.FindCompany(id);
            if (foundCompany == null)
            {
                return NotFound(); 
            }
            else if (foundCompany.IsDeleted)
            {
                return RedirectToAction("DisplayAllCompanies");
            }
            var companyToDelete = new DeleteCompanyViewModel();
            companyToDelete.Id = foundCompany.Id;
            companyToDelete.IsDeleted = foundCompany.IsDeleted;
            return View(companyToDelete);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCompany(DeleteCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                await service.DeleteCompany(model);
                return RedirectToAction("DisplayAllCompanies");
            }
            else return View();
        }




        [HttpGet]
        public IActionResult DisplayAllCompanyWorkers(int id)
        {
            var workerS = new DisplayAllWorkersInCompanyViewModel()
            {
                Workers = service.DisplayAllCompanyWorkers(id)
            };
            return View(workerS); 

        }
       
        [HttpGet]
        public IActionResult DisplayAllCompanyProjects(int id)
        {
            var projectS = new DisplayAllCompanyProjectsViewModel()
            {
                Projects = service.DisplayAllCompanyProjects(id)
            };
            return View(projectS); 
        }


    }
}
