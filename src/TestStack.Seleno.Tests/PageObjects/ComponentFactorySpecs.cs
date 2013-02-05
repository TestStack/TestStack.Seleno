using System;
using FluentAssertions;
using Funq;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.ViewModels;

namespace TestStack.Seleno.Tests.PageObjects
{
    public abstract class ComponentFactorySpecification : SpecificationFor<ComponentFactory>
    {
        protected object Result;
    }

    public class creating_a_PageReader : ComponentFactorySpecification
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

    public class creating_a_PageWriter : ComponentFactorySpecification
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

    public class creating_an_ElementAssert : ComponentFactorySpecification
    {
        public void When_asked_to_create_an_ElementAssert()
        {
            Result = SUT.CreateElementAssert(Arg.Any<By>());
        }

        public void Then_ComponentFactory_should_create_an_ElementAssert()
        {
            Result.Should().BeOfType<ElementAssert>();
        }
    }

    public class creating_a_Page : ComponentFactorySpecification
    {
        public creating_a_Page()
        {
            SubstituteFor<IContainer>().Resolve<TestPage>().Returns(new TestPage());
        }

        public void When_asked_to_create_a_Page()
        {
            Result = SUT.CreatePage<TestPage>();
        }

        public void Then_it_should_get_the_page_from_the_Container()
        {
            SubstituteFor<IContainer>().Received().Resolve<TestPage>();
        }

        public void AndThen_ComponentFactory_should_create_the_Page()
        {
            Result.Should().BeOfType<TestPage>();
        }
    }

}
