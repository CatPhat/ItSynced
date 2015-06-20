using ItSynced.Web.Models;
using Microsoft.AspNet.Mvc;

namespace ItSynced.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string directory)
        {
            ViewBag.Title = "Home Page";
            var model = CacheRepository.GetItem(@directory);

            return View(model);
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}