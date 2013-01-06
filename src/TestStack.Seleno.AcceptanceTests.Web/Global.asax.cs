using System.Web.Routing;
using TestStack.Seleno.AcceptanceTests.Web.App_Start;

namespace TestStack.Seleno.AcceptanceTests.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}