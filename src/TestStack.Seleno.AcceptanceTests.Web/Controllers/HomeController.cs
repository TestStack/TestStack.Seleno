using System.Web.Mvc;

namespace TestStack.Seleno.AcceptanceTests.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }
    }
}