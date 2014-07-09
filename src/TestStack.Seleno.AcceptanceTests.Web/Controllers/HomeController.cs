using System;
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

        public ActionResult TestingEnvVariable()
        {
            var envVar = Environment.GetEnvironmentVariable("FunctionalTest");
            if (envVar != "SomeVal")
                throw new Exception("Environment Variable was not injected!!");

            return RedirectToAction("Index");
        }
    }
}