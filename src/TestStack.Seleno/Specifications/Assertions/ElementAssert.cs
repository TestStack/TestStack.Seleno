using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Specifications.Assertions
{
    public class ElementAssert : IElementAssert
    {
        private readonly ICamera _camera;

        public ElementAssert(ICamera camera, IElementFinder find)
        {
            _camera = camera;
            Find = find;
        }

        public IElementFinder Find { get; private set; }

        private IElementAssert DoNotExist(Action action, string selector, string message)
        {
            try
            {
                action();
            }
            catch (NoSuchElementException)
            {
                return this;
            }

            if (string.IsNullOrEmpty(message))
                message = string.Format("'{0}' was in fact found!", selector);

            // todo: Can we move the screenshots to a central place that catches any
            //  uncaught exceptions rather than having them in a number of places just
            //  before throwing an exception
            _camera.TakeScreenshot();
            throw new SelenoException(message);
        }

        public IElementAssert DoNotExist(By findExpression, string message = null, TimeSpan maxWait = default(TimeSpan))
        {
            return DoNotExist(() => Find.Element(findExpression, maxWait), findExpression.ToString(), message);
        }

        public IElementAssert DoNotExist(PageObjects.Locators.By.jQueryBy findExpression, string message = null, TimeSpan maxWait = default(TimeSpan))
        {
            return DoNotExist(() => Find.Element(findExpression, maxWait), findExpression.ToString(), message);
        }

        IElementAssert Exist(Action action, string message)
        {
            try
            {
                action();
            }
            catch (NoSuchElementException ex)
            {
                _camera.TakeScreenshot();
                throw new SelenoException(message, ex);
            }

            return this;
        }

        public IElementAssert Exist(By findExpression, string message = null, TimeSpan maxWait = default(TimeSpan))
        {
            return Exist(() => Find.Element(findExpression, maxWait), message);
        }

        public IElementAssert Exist(PageObjects.Locators.By.jQueryBy findExpression, string message = null, TimeSpan maxWait = default(TimeSpan))
        {
            return Exist(() => Find.Element(findExpression, maxWait), message);
        }

        IElementAssert ConformTo(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                _camera.TakeScreenshot();
                throw;
            }

            return this;
        }

        public IElementAssert ConformTo(By findExpression, Action<IEnumerable<IWebElement>> assertion, TimeSpan maxWait = default(TimeSpan))
        {
            return ConformTo(() =>
                {
                    var elements = Find.Elements(findExpression, maxWait);
                    assertion(elements);
                });
        }

        public IElementAssert ConformTo(PageObjects.Locators.By.jQueryBy findExpression, Action<IEnumerable<IWebElement>> assertion, TimeSpan maxWait = default(TimeSpan))
        {
            return ConformTo(() =>
                {
                    var elements = Find.Elements(findExpression, maxWait);
                    assertion(elements);
                });
        }
    }
}