using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Configuration
{
    class when_creating_application_and_resolving_page_object : SpecificationFor<AppConfigurator>
    {
        private ExamplePageObject _pageObject;
        private IWebDriver _webDriver;
        private ICamera _camera;
        private ISelenoApplication _application;
        private string _baseUrl;

        public void Given_there_is_a_registered_web_driver()
        {
            _webDriver = Substitute.For<IWebDriver>();
            SUT.WithWebDriver(() => _webDriver);
            SUT.WithJavaScriptExecutor(() => Substitute.For<IJavaScriptExecutor>());
        }

        public void AndGiven_there_is_a_registered_camera()
        {
            _camera = Substitute.For<ICamera>();
            SUT.UsingCamera(_camera);
        }

        public void AndGiven_a_web_server_is_registered_with_a_base_url()
        {
            var webserver = Substitute.For<IWebServer>();
            _baseUrl = "http://localhost:3000";
            webserver.BaseUrl.Returns(_baseUrl);
            SUT.WithWebServer(webserver);
        }

        public void AndGiven_the_application_is_created()
        {
            _application = SUT.CreateApplication();
        }

        public void When_navigating_to_a_page_and_returning_a_page_object()
        {
            _pageObject = _application.NavigateToInitialPage<ExamplePageObject>();
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
            Assert.That(_pageObject.ElementFinder, Is.TypeOf<ElementFinder>());
        }

        public void And_the_page_navigator_is_populated()
        {
            Assert.That(_pageObject.PageNavigator, Is.TypeOf<PageNavigator>());
        }

        public void And_the_script_executor_is_populated()
        {
            Assert.That(_pageObject.ScriptExecutor, Is.TypeOf<ScriptExecutor>());
        }

        public void And_the_component_factory_is_populated()
        {
            Assert.That(_pageObject.ComponentFactory, Is.TypeOf<ComponentFactory>());
        }

        public void And_the_browser_was_navigated_to_the_correct_url()
        {
            _webDriver.Navigate().Received().GoToUrl(_baseUrl);
        }
    }

    public class ExamplePageObject : Page {}
}
