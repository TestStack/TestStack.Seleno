using System;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;

namespace TestStack.Seleno.PageObjects
{
    public interface IComponentFactory
    {
        PageReader<T> CreatePageReader<T>() where T : class, new();
        PageWriter<T> CreatePageWriter<T>() where T : class, new();

        ElementAssert CreateElementAssert(By selector);
        TPage CreatePage<TPage>() where TPage : UiComponent, new();
    }

    //public interface IPageFactory
    //{
    //    TPage CreatePage<TPage>() where TPage : UiComponent, new();
    //}

    //public class PageFactory : IPageFactory
    //{
    //    public TPage CreatePage<TPage>() where TPage : UiComponent, new()
    //    {
    //        return (TPage)new UiComponent(_browser, _pageNavigator, _elementFinder, _scriptExecutor, _camera, this);
    //    }
    //}
}
