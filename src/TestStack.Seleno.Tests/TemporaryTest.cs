using Xunit;

namespace TestStack.Seleno.Tests
{
    public class TemporaryFixture
    {
        [Fact]
        public void ToVerifyThatTheBuildRunsTests()
        {
            Assert.Equal(1, 1); 
        }
    }
}
