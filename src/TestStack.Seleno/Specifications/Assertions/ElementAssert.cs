﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Specifications.Assertions
{
    public class ElementAssert : IElementAssert
    {
        private readonly By _selector;
        private readonly ICamera _camera;
        readonly IWebDriver _browser;

        public ElementAssert(By selector, ICamera camera, IWebDriver browser)
        {
            _selector = selector;
            _camera = camera;
            _browser = browser;
        }

        public IElementAssert DoNotExist(string message = null)
        {
            try
            {
                _browser.FindElement(_selector);
            }
            catch (NoSuchElementException)
            {
                return this;
            }

            if (string.IsNullOrEmpty(message))
                message = string.Format("'{0}' was in fact found!", _selector);

            // todo: Can we move the screenshots to a central place that catches any
            //  uncaught exceptions rather than having them in a number of places just
            //  before throwing an exception
            _camera.TakeScreenshot();
            throw new SelenoException(message);
        }

        public IElementAssert Exist(string message = null)
        {
            try
            {
                _browser.FindElement(_selector);
            }
            catch (NoSuchElementException ex)
            {
                _camera.TakeScreenshot();
                throw new SelenoException(message, ex);
            }

            return this;
        }

        public IElementAssert ConformTo(Action<IEnumerable<IWebElement>> assertion)
        {
            try
            {
                var elements = _browser.FindElements(_selector);
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