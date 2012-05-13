using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace SeleniumExtensions
{
    public class ElementAssert
    {
        private readonly UiComponent _component;
        private readonly By _selector;

        public ElementAssert(UiComponent component, By selector)
        {
            _component = component;
            _selector = selector;
        }

        RemoteWebDriver Browser
        {
            get
            {
                return _component.Browser;
            }
        }

        public ElementAssert DoNotExist(string message = null)
        {
            try
            {
                Browser.FindElement(_selector);
            }
            catch (NoSuchElementException)
            {
                return this;
            }

            if (string.IsNullOrEmpty(message))
                message = string.Format("'{0}' was in fact found!", _selector);

            throw new SeleniumExtensionsException(message);
        }

        public ElementAssert Exist(string message = null)
        {
            try
            {
                Browser.FindElement(_selector);
            }
            catch (NoSuchElementException ex)
            {
                throw new SeleniumExtensionsException(message, ex);
            }

            return this;
        }

        public ElementAssert ConformTo(Action<IEnumerable<IWebElement>> assertion)
        {
            try
            {
                var elements = Browser.FindElements(_selector);
                assertion(elements);
            }
            catch (Exception)
            {
                _component.TakeScreenshot();
                throw;
            }

            return this;
        }
    }
}