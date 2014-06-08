using System.Web.Mvc;

namespace Solaise.Weather.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
