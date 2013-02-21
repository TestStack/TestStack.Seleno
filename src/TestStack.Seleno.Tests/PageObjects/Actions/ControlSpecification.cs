using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions
{
    abstract class ControlSpecification<T> : SpecificationFor<T>
        where T: class
    {
        protected IPageNavigator PageNavigator { get; private set; }
        protected IElementFinder ElementFinder { get; private set; }
        protected IScriptExecutor ScriptExecutor { get; private set; }
        protected IComponentFactory ComponentFactory { get; private set; }

        protected ControlSpecification()
        {
            PageNavigator = SubstituteFor<IPageNavigator>();
            ElementFinder = SubstituteFor<IElementFinder>();
            ComponentFactory = SubstituteFor<IComponentFactory>();
            ScriptExecutor = SubstituteFor<IScriptExecutor>();
        }

        protected THtmlControl HtmlControl<THtmlControl>(Expression<Func<TestViewModel, Object>> htmlControlPropertySelector)
            where THtmlControl : HTMLControl, new()
        {
            var htmlControl = SubstituteFor<THtmlControl>();

            htmlControl.ViewModelPropertySelector = htmlControlPropertySelector;
            htmlControl.PageNavigator = PageNavigator;
            htmlControl.ElementFinder = ElementFinder;
            htmlControl.ScriptExecutor = ScriptExecutor;
            htmlControl.ComponentFactory = ComponentFactory;

            ComponentFactory
              .HtmlControlFor<THtmlControl>(Arg.Any<LambdaExpression>(), Arg.Any<int>())
              .Returns(htmlControl);

            return htmlControl;

        }
    }
}
