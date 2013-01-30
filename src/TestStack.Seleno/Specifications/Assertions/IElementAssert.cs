using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace TestStack.Seleno.Specifications.Assertions
{
    public interface IElementAssert
    {
        IElementAssert DoNotExist(string message = null);
        IElementAssert Exist(string message = null);
        IElementAssert ConformTo(Action<IEnumerable<IWebElement>> assertion);
    }
}