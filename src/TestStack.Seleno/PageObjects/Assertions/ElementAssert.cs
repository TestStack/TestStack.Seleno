using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Interceptors;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.PageObjects.Assertions
{
    public class ElementAssert : IElementAssert
    {
        public ElementAssert(IElementFinder find)
        {
            Find = find;
        }

        public IElementFinder Find { get; }

        private IElementAssert DoNotExist(Action action, string selector, string message)
        {
            try
            {
                action();
            }
            catch (SelenoReceivedException e)
            {
                if (e.InnerException is NoSuchElementException)
                    return this;
                throw;
            }
            catch (NoSuchElementException)
            {
                return this;
            }

            if (string.IsNullOrEmpty(message))
                message = $"'{selector}' was in fact found!";

            throw new SelenoException(message);
        }

        public IElementAssert DoNotExist(By findExpression, string message = null, TimeSpan maxWait = default(TimeSpan))
        {
            return DoNotExist(() => Find.Element(findExpression, maxWait), findExpression.ToString(), message);
        }

        public IElementAssert DoNotExist(Locators.By.jQueryBy findExpression, string message = null, TimeSpan maxWait = default(TimeSpan))
        {
            return DoNotExist(() => Find.Element(findExpression, maxWait), findExpression.ToString(), message);
        }

        IElementAssert Exist(Action action, string message)
        {
            try
            {
                action();
            }
            catch (SelenoReceivedException ex)
            {
                if (ex.InnerException is NoSuchElementException)
                    throw new SelenoException(message, ex);
                throw;
            }
            catch (NoSuchElementException ex)
            {
                throw new SelenoException(message, ex);
            }

            return this;
        }

        public IElementAssert Exist(By findExpression, string message = null, TimeSpan maxWait = default(TimeSpan))
        {
            return Exist(() => Find.Element(findExpression, maxWait), message);
        }

        public IElementAssert Exist(Locators.By.jQueryBy findExpression, string message = null, TimeSpan maxWait = default(TimeSpan))
        {
            return Exist(() => Find.Element(findExpression, maxWait), message);
        }

        IElementAssert ConformTo(Action action)
        {
            action();
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

        public IElementAssert ConformTo(Locators.By.jQueryBy findExpression, Action<IEnumerable<IWebElement>> assertion, TimeSpan maxWait = default(TimeSpan))
        {
            return ConformTo(() =>
                {
                    var elements = Find.Elements(findExpression, maxWait);
                    assertion(elements);
                });
        }
    }
}