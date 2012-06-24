using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.Screenshots
{
    public class NullCamera : ICamera
    {
        public void TakeScreenshot(string fileName = null)
        {
            // do nothing
        }
    }
}