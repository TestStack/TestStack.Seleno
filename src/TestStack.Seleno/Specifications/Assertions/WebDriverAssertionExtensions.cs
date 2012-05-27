using OpenQA.Selenium;

namespace TestStack.Seleno.Specifications.Assertions
{
    public static class WebDriverAssertionExtensions
    {
        public static void ShouldHavePageTitleOf(this IWebDriver driver, string title)
        {
            //driver.Title.Should().Be(title);
        }

        public static void ShouldHavePageIdOf(this IWebDriver driver, string pageId)
        {
            // TODO: a configurable func to specify the name of the PageId in the web app?
            //IWebElement hiddenId = (driver as RemoteWebDriver).FindElement(By.jQuery("#pageId"));
            //hiddenId.GetAttribute("Value").Should().Be(pageId);
        }
    }
}
