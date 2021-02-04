using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace TicketMaster.Areas.Agent.Controllers
{
    [Route("[Area]/[Controller]/[Action]")]
    [Authorize(Roles ="Agent,Admin")]
    [Area("Agent")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AgentGuide()
        {
            string path = "F:/Projects/repos/TicketMaster/TicketMaster/wwwroot/guides/agentGuide.txt";
            string[] content;
            string allContent;
            string txt;
            try
            {
                using(StreamReader src= new StreamReader(path))
                {
                    txt = src.ReadToEnd();
                    if (txt != null)
                    {
                        allContent = txt;
                        content = allContent.Split("----");

                        string navbarGuide = content[0];
                        string homeGuide = content[1];
                        string companyGuide = content[2];
                        string userGuide = content[3];
                        string projectGuide = content[4];
                        string ticketGuide = content[5];

                        ViewBag.navbarGuide = navbarGuide;
                        ViewBag.homeGuide = homeGuide;
                        ViewBag.companyGuide = companyGuide;
                        ViewBag.userGuide = userGuide;
                        ViewBag.projectGuide = projectGuide;
                        ViewBag.ticketGuide = ticketGuide;
                    }
                }
            }
            catch(Exception)
            {
                ViewBag.guideError = "The file could not be loaded."; 
            }

            return View();
        }
    }
}
