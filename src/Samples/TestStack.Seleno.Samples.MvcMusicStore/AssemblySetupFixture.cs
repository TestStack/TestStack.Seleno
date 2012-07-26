using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;
using NUnit.Framework;

namespace TestStack.Seleno.Samples.MvcMusicStore
{
    [SetUpFixture]
    public class AssemblySetupFixture
    {
        private ISelenoApplication _selenoApplication;

        [SetUp]
        public void SetUp()
        {
            _selenoApplication = SelenoApplicationRunner
                .New(x =>
                         x.ProjectToTest(
                             WebApplication.Create(app =>
                             {
                                 app.Location = ProjectLocation.FromFolder("MvcMusicStore");
                                 app.PortNumber = 18763;
                             })));
            _selenoApplication.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _selenoApplication.ShutDown();
        }
    }
}
