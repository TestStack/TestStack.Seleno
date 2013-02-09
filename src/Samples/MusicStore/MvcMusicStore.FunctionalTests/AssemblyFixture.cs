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
            SelenoHost.Run("MvcMusicStore", 12345);
        }
    }
}