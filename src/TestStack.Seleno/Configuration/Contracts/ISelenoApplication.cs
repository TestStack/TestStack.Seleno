using Funq;
using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration.Contracts
{
    public interface ISelenoApplication
    {
        void Initialize();
        void ShutDown();

        Container Container { get; }
        IWebDriver Browser { get; }
        IWebServer WebServer { get; }
        ICamera Camera { get; }
    }
}
