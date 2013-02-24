using System;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    abstract class HtmlControlSpecificationFor<THtmlControl> : SpecificationFor<THtmlControl>
        where THtmlControl : HTMLControl, new()
    {

        protected IPageNavigator PageNavigator { get; private set; }
        protected IElementFinder ElementFinder { get; private  set; }
        protected IScriptExecutor ScriptExecutor { get; private set; }
        protected IComponentFactory ComponentFactory { get; private set; }

        protected HtmlControlSpecificationFor(Expression<Func<TestViewModel, object>> htmlControlPropertySelector)
        {
            PageNavigator = SubstituteFor<IPageNavigator>();
            ElementFinder = SubstituteFor<IElementFinder>();
            ComponentFactory = SubstituteFor<IComponentFactory>();
            ScriptExecutor = SubstituteFor<IScriptExecutor>();

            SUT = HtmlControl(htmlControlPropertySelector);
        }
        
        private THtmlControl HtmlControl(Expression<Func<TestViewModel, object>> htmlControlPropertySelector)
        {
            SUT = SUT ?? SubstituteFor<THtmlControl>();

            SUT.ViewModelPropertySelector = htmlControlPropertySelector;
            SUT.PageNavigator = PageNavigator;
            SUT.ElementFinder = ElementFinder;
            SUT.ScriptExecutor = ScriptExecutor;
            SUT.ComponentFactory = ComponentFactory;
            
            return SUT;

        }

        
    }
}