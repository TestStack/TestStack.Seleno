using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.PageObjects
{
    public class AssertionResultPage : Page
    {
        public IEnumerable<IWebElement> ExceptionElement => Find.Elements(By.Id("exception"));

        public void AssertResult()
        {
            if (ExceptionElement.Any())
            {
                throw new Exception(ExceptionElement.First().Text);
            }
        }
    }
}