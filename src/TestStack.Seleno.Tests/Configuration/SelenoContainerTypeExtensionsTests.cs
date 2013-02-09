using Funq;
using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.Tests.Configuration
{
    class SelenoContainerTypeExtensionsTests : Specification
    {
        private Container _container;
        private ExamplePageObject _pageObject;
        private IWebDriver _webDriver;
        private ICamera _camera;
        private IElementFinder _elementFinder;
        private IPageNavigator _pageNavigator;
        private IScriptExecutor _scriptExecutor;
        private IComponentFactory _componentFactory;

        public void Given_a_container()
        {
            _container = new Container();
        }

        public void AndGiven_there_is_a_registered_web_driver()
        {
            _webDriver = Substitute.For<IWebDriver>();
            _container.Register(c => _webDriver);
        }

        public void AndGiven_there_is_a_registered_camera()
        {
            _camera = Substitute.For<ICamera>();
            _container.Register(c => _camera);
        }

        public void AndGiven_there_is_a_registered_element_finder()
        {
            _elementFinder = Substitute.For<IElementFinder>();
            _container.Register(c => _elementFinder);
        }

        public void AndGiven_there_is_a_registered_page_navigator()
        {
            _pageNavigator = Substitute.For<IPageNavigator>();
            _container.Register(c => _pageNavigator);
        }

        public void AndGiven_there_is_a_registered_script_executor()
        {
            _scriptExecutor = Substitute.For<IScriptExecutor>();
            _container.Register(c => _scriptExecutor);
        }

        public void AndGiven_there_is_a_registered_component_factory()
        {
            _componentFactory = Substitute.For<IComponentFactory>();
            _container.Register(c => _componentFactory);
        }

        public void AndGiven_a_page_object_is_registered()
        {
            _container.RegisterPageObjects(new [] {typeof(ExamplePageObject)});
        }

        public void When_resolving_the_page_object()
        {
            _pageObject = _container.Resolve<ExamplePageObject>();
        }

        public void Then_the_page_is_instantiated()
        {
            Assert.That(_pageObject, Is.Not.Null);
        }

        public void And_the_browser_is_populated()
        {
            Assert.That(_pageObject.Browser, Is.EqualTo(_webDriver));
        }

        public void And_the_camera_is_populated()
        {
            Assert.That(_pageObject.Camera, Is.EqualTo(_camera));
        }

        public void And_the_element_finder_is_populated()
        {
            Assert.That(_pageObject.ElementFinder, Is.EqualTo(_elementFinder));
        }

        public void And_the_page_navigator_is_populated()
        {
            Assert.That(_pageObject.PageNavigator, Is.EqualTo(_pageNavigator));
        }

        public void And_the_script_executor_is_populated()
        {
            Assert.That(_pageObject.ScriptExecutor, Is.EqualTo(_scriptExecutor));
        }

        public void And_the_component_factory_is_populated()
        {
            Assert.That(_pageObject.ComponentFactory, Is.EqualTo(_componentFactory));
        }
    }

    class ExamplePageObject : Page {}
}
