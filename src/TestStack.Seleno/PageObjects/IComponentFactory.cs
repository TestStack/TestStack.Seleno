using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;

namespace TestStack.Seleno.PageObjects
{
    public interface IComponentFactory
    {
        IPageReader<T> CreatePageReader<T>() where T : class, new();
        IPageWriter<T> CreatePageWriter<T>() where T : class, new();
        IElementAssert CreateElementAssert(By selector);
        TPage CreatePage<TPage>() where TPage : UiComponent, new();
    }
}
