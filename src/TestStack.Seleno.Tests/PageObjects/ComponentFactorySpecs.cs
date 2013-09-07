using Autofac;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects
{
    abstract class ComponentFactorySpecification : SpecificationFor<ComponentFactory>
    {
        protected object Result;
    }

    class creating_a_PageReader : ComponentFactorySpecification
    {
        public void When_asked_to_create_a_PageReader_for_a_view_model()
        {
            Result = SUT.CreatePageReader<TestViewModel>();
        }

        public void Then_ComponentFactory_should_create_a_PageReader_for_that_view_model()
        {
            Result.Should().BeOfType<PageReader<TestViewModel>>();
        }
    }

    class creating_a_PageWriter : ComponentFactorySpecification
    {
        public void When_asked_to_create_a_PageWriter_for_a_view_model()
        {
            Result = SUT.CreatePageWriter<TestViewModel>();
        }

        public void Then_ComponentFactory_should_create_a_PageWriter_for_that_view_model()
        {
            Result.Should().BeOfType<PageWriter<TestViewModel>>();
        }
    }

    class creating_an_ElementAssert : ComponentFactorySpecification
    {
        public void When_asked_to_create_an_ElementAssert()
        {
            Result = SUT.CreateElementAssert(Arg.Any<IElementFinder>());
        }

        public void Then_ComponentFactory_should_create_an_ElementAssert()
        {
            Result.Should().BeOfType<ElementAssert>();
        }
    }

    class creating_a_Page : ComponentFactorySpecification
    {
        private readonly TestPage _testPage;

        public creating_a_Page()
        {
            _testPage = new TestPage();
            AutoSubstitute.Provide(_testPage);
        }

        public void When_asked_to_create_a_Page()
        {
            Result = SUT.CreatePage<TestPage>();
        }

        public void AndThen_ComponentFactory_should_create_the_Page_from_the_container()
        {
            Result.Should().Be(_testPage);
        }
    }
}
