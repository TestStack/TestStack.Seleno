using NUnit.Framework;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Screenshots;

namespace MvcMusicStore.FunctionalTests
{
    [SetUpFixture]
    public class AssemblyFixture
    {
        [SetUp]
        public void SetUp()
        {
            SelenoApplicationRunner.Run(
                "MvcMusicStore", 
                12345,
                c => c.UsingCamera(new FileCamera("d:\\temp\\AUIT")));
        }
    }
}