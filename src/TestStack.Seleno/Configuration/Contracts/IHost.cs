using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration.Contracts
{
    public interface IHost
    {
        void Initialize();
        void ShutDown();

        IWebDriver Browser { get; }
        ICamera Camera { get; }
    }
}
