using System;
using System.Threading;
using System.Web.Mvc;
using NUnit.Framework;
using TestStack.Seleno.AcceptanceTests.Web.Fixtures;
using TestStack.Seleno.AcceptanceTests.Web.ViewModels;

namespace TestStack.Seleno.AcceptanceTests.Web.Controllers
{
    public class Form1Controller : Controller
    {
        public ActionResult ExpectFixtureA()
        {
            return View("Form", new Form1ViewModel());
        }

        [HttpPost]
        public ActionResult ExpectFixtureA(Form1ViewModel vm)
        {
            try
            {
                Assert.That(vm, IsSame.ViewModelAs(Form1Fixtures.A));
            }
            catch (Exception e)
            {
                return View("ExpectedFixtureAResult", new ExceptionViewModel
                {
                    Exception = e
                });
            }

            return new EmptyResult();
        }

        public ActionResult FixtureA()
        {
            return View("Form", Form1Fixtures.A);
        }

        public ActionResult FormWithAjax()
        {
            return View();
        }

        public ActionResult AjaxElements()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return Content("AJAX result");
        }

        public ActionResult Details()
        {
            return View();
        }
    }
}