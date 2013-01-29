using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;

namespace TestStack.Seleno.PageObjects
{
    public interface IComponentFactory
    {
        // todo PageReader -> IPageReader to make it's usage more testable
        PageReader<T> CreatePageReader<T>() where T : class, new();
        // todo PageWriter -> IPageWriter to make it's usage more testable
        PageWriter<T> CreatePageWriter<T>() where T : class, new();
        // todo ElementAssert -> IElementAssert to make it's usage more testable
        ElementAssert CreateElementAssert(By selector);
        TPage CreatePage<TPage>() where TPage : UiComponent, new();
    }
}
