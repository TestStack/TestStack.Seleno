using NUnit.Framework;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests
{
    [SetUpFixture]
    public class AssemblyFixture
    {
        [SetUp]
        public void SetUp()
        {
            SelenoApplicationRunner.Run("TestStack.Seleno.AcceptanceTests.Web", 12345);
        }
    }
}
