using System;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    abstract class PageReaderSpecification : SpecificationFor<PageReader<TestViewModel>>
    {
        protected IPageNavigator PageNavigator { get; private set; }
        protected IElementFinder ElementFinder { get; private set; }
        protected IComponentFactory ComponentFactory { get; private set; }

        public override Type Story
        {
            get { return typeof (PageReaderSpecification); }
        }

        protected PageReaderSpecification()
        {
            PageNavigator = SubstituteFor<IPageNavigator>();
            ElementFinder = SubstituteFor<IElementFinder>();
            ComponentFactory = SubstituteFor<IComponentFactory>();
        }

        protected THtmlControl HtmlControl<THtmlControl>(Expression<Func<TestViewModel,Object>> htmlControlPropertySelector)
            where THtmlControl : HTMLControl, new()
        {
           var htmlControl = SubstituteFor<THtmlControl>();

            htmlControl.ViewModelPropertySelector = htmlControlPropertySelector;
            htmlControl.PageNavigator = PageNavigator;
            htmlControl.ElementFinder = ElementFinder;
            htmlControl.ComponentFactory = ComponentFactory;

            ComponentFactory
              .HtmlControlFor<THtmlControl>(Arg.Any<LambdaExpression>(), Arg.Any<int>())
              .Returns(htmlControl);
            
            return htmlControl;

        }

        

       
    }
}
