using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
        private readonly ItSyncedContext _context;
       
        public HomeController(ItSyncedContext context)
        {
            _context = context;
        }
      
        public async Task<IActionResult> Index(string directory)
        {
            ViewBag.Title = "Home Page";
            if (directory == null)
            {
                directory = "." + Path.DirectorySeparatorChar;
            }
            using (var db = _context)
            {
                await db.Database.EnsureCreatedAsync();
            }

            List<Directory> directories = new List<Directory>();
            using (var db = _context)
            {
                var directoriesToAdd = new DirectoryCrawler().GetDirectories(directory);
                db.AddRange(directoriesToAdd);
                await db.SaveChangesAsync();
                directories.AddRange(db.Directories);
            }
            
            return View(directories);
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}