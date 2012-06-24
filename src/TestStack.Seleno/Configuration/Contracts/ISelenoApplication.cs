using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration.Contracts
{
    public interface ISelenoApplication
    {
        void Initialize();
        void ShutDown();

        IWebDriver Browser { get; }
        ICamera Camera { get; }
    }
}
