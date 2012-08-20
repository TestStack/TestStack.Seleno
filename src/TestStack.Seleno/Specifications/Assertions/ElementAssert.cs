using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Specifications.Assertions
{
    public class ElementAssert
    {
        private readonly By _selector;
        private readonly ICamera _camera;

        public ElementAssert(By selector, ICamera camera)
        {
            _selector = selector;
            _camera = camera;
        }

        IWebDriver Browser
        {
            get
            {
                return SelenoApplicationRunner.Host.Browser;
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

            _camera.TakeScreenshot();
            throw new SelenoException(message);
        }

        public ElementAssert Exist(string message = null)
        {
            try
            {
                Browser.FindElement(_selector);
            }
            catch (NoSuchElementException ex)
            {
                _camera.TakeScreenshot();
                throw new SelenoException(message, ex);
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
                _camera.TakeScreenshot();
                throw;
            }

            return this;
        }
    }
}