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
            SelenoApplicationRunner.Run("TestStack.Samples.ModelIoTesting", 12345);
        }
    }
}
