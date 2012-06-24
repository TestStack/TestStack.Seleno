using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;

using NUnit.Framework;

namespace MvcMusicStore.SampleFunctionalTests
{
    [SetUpFixture]
    public class AssemblySetupFixture
    {
        private IHost _host;

        [SetUp]
        public void SetUp()
        {
            _host = HostFactory
                .New(x =>
                         x.ProjectToTest(
                             WebApplication.Create(app =>
                             {
                                 app.Location = ProjectLocation.FromFolder("MvcMusicStore");
                                 app.PortNumber = 18763;
                             })));
            _host.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _host.ShutDown();
        }
    }


    //[SetUpFixture]
    //public class AssemblySetupFixture
    //{
    //    [SetUp]
    //    public void SetUp()
    //    {
    //        SelenoApplicationRunner.Run(configurator =>
    //        {
    //            //cfg.WithWebProject(
    //            //    new WebApplication2(ProjectLocation.FromFolder("MvcMusicStore"), 12345));
    //            configurator
    //                .WithWebDriver(() => GetFireFoxDriver());
    //            configurator.WithWebProject(WebApplication.Create(app =>
    //            {
    //                app.Location = ProjectLocation.FromFolder("MvcMusicStore");
    //                app.PortNumber = 12345;
    //            }));
    //        });
    //    }

    //    private static IWebDriver GetFireFoxDriver()
    //    {
    //        var browser = new FirefoxDriver();
    //        browser.SetImplicitTimeout(10);
    //        return browser;
    //    }

    //}
}
