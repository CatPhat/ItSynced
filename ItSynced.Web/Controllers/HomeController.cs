using ItSynced.Web.Models;
using Microsoft.AspNet.Mvc;

namespace ItSynced.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var model = CacheRepository.GetItem(@"A:\DEVOPS");

            return View(model);
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}