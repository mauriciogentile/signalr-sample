using System.Web.Mvc;

namespace Solaise.Weather.Web.Controllers
{
    public class TemplateController : Controller
    {
        public ActionResult Get(string id)
        {
            return PartialView(id);
        }
    }
}
