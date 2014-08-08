using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.DomCaptures
{
    /// <summary>
    /// DOM Capturer that doesn't actually do anything
    /// </summary>
    public class NullDomCapture : IDomCapture
    {
        public IWebDriver Browser { get; set; }

        public void CaptureDom(string fileName = null)
        {
            //do nothing
        }
    }
}
