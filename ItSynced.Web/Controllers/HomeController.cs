using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ItSynced.Web.DAL.Entities.Commands;
using ItSynced.Web.DAL.Entities.Queries;
using ItSynced.Web.DAL.EntityFramework;
using ItSynced.Web.DAL.LocalsystemCrawler;
using ItSynced.Web.DAL.MemoryCache;
using ItSynced.Web.Models;
using Microsoft.AspNet.Mvc;
using Directory = ItSynced.Web.DAL.Entities.Directory;

namespace ItSynced.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceProvider _service;
       
        public HomeController(IServiceProvider service)
        {
            _service = service;
        }
      
        public async Task<IActionResult> Index(string directory)
        {
            ViewBag.Title = "Home Page";
            if (directory == null)
            {
                directory = "." + Path.DirectorySeparatorChar;
            }
         
            List<Directory> directories = new List<Directory>();
           var command = (CreateDirectories) _service.GetService(typeof (CreateDirectories));
           await command.Create(new DirectoryCrawler().GetDirectories(directory));

          
          //  directories = await query.GetAsync();
            return View(directories);
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}