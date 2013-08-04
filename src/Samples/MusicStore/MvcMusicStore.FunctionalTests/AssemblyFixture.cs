using NUnit.Framework;
using TestStack.Seleno.Configuration;

namespace MvcMusicStore.FunctionalTests
{
    public class Host
    {
        public static readonly SelenoHost Instance = new SelenoHost();
    }

    [SetUpFixture]
    public class AssemblyFixture
    {
        [SetUp]
        public void SetUp()
        {
            Host.Instance.Run("MvcMusicStore", 12345);
        }
    }
}