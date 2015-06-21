using System.Threading.Tasks;
using ItSynced.Web.Models;
using Microsoft.AspNet.Mvc;

namespace ItSynced.Web.Controllers
{
    public class HomeController : Controller
    {
      
        public async Task<IActionResult> Index(string directory)
        {
            ViewBag.Title = "Home Page";
            var model = new CacheRepository().GetItem(@directory);

            return View(await Task.FromResult(model));
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}