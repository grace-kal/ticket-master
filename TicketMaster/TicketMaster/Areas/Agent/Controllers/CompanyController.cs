using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketMaster.Areas.Agent.Services.Interfaces;
using TicketMaster.Areas.Agent.ViewModels.Company;
using TicketMaster.Models;

namespace TicketMaster.Areas.Agent.Controllers
{
    [Route("[Area]/[Controller]/[Action]")]
    [Authorize(Roles ="Admin,Agent")]
    [Area("Agent")]
    public class CompanyController : Controller
    {
        private readonly IAgentCompanyService service;
        public CompanyController(IAgentCompanyService service)
        {
            this.service = service; 
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyCompany(string id)
        {
            //here the value i get is named id but it contains User.Identity.Name 
            var myCompany = await service.MyCompany(id);
            if (myCompany == null)
            {
                return NotFound(); 
            }

            return View(myCompany);

        }
        public IActionResult CompanyProjects(int id)
        {
            var list = new CompanyProjectsViewModel
            {
                CompanyProjects = service.CompanyProjects(id)
            };
            return View(list); 
        }
        public IActionResult CompanyWorkers(int id)
        {
            var list = new CompanyWorkersViewModel
            {
                CompanyWorkers=service.CompanyWorkers(id)
            };
            return View(list); 
        }


    }
}
