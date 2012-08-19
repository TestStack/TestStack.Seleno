using NUnit.Framework;
using TestStack.Seleno.Configuration;

namespace MvcMusicStore.FunctionalTests
{
    [SetUpFixture]
    public class AssemblyFixture
    {
        [SetUp]
        public void SetUp()
        {
            SelenoApplicationRunner.Run("MvcMusicStore", 12345);
        }

        [TearDown]
        public void TearDown()
        {
            SelenoApplicationRunner.Host.Browser.Close();
        }
    }
}