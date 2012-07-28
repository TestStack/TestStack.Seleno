using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IElementFinder
    {
        IWebElement ElementWithWait(By findElement, int waitInSeconds = 20);
        IWebElement TryFindElement(By by);
        IWebElement TryFindElementByjQuery(Locators.By.jQueryBy by);
    }
}