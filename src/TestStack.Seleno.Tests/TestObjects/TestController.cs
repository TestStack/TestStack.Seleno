using System.Web.Mvc;

namespace TestStack.Seleno.Tests.TestObjects
{
    public abstract class TestController : Controller
    {
        [HttpGet]
        public abstract ActionResult Index();

        public abstract ActionResult ActionWithParameters(string parameter);
    }
}