using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TicketMaster.Areas.Admin.Controllers
{
    [Route("[Area]/[Controller]/[Action]")]
    [Authorize(Roles ="Admin")]
    [Area("Admin")]

    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult AdminGuide()
        {
            string path = "F:/Projects/repos/TicketMaster/TicketMaster/wwwroot/guides/adminGuide.txt";
            string allContent;
            string[] content;
            string txt;
            try
            {
                
                using (StreamReader src = new StreamReader(path))
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
                        ViewBag.userGuide = userGuide;
                        ViewBag.companyGuide = companyGuide;
                        ViewBag.projectGuide = projectGuide;
                        ViewBag.ticketGuide = ticketGuide; 
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.guideError = "The file could not be loaded.";
            }


            return View(); 
        }
    }
}
