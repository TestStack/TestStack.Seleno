using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace TestStack.Seleno.Specifications.Assertions
{
    public interface IElementAssert
    {
        ElementAssert DoNotExist(string message = null);
        ElementAssert Exist(string message = null);
        ElementAssert ConformTo(Action<IEnumerable<IWebElement>> assertion);
    }
}