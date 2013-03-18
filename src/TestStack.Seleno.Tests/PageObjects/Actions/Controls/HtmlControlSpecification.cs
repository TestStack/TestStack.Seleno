using System;
using System.Linq.Expressions;
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
            SUT.ViewModelPropertySelector = htmlControlPropertySelector;
        }

        public override void InitialiseSystemUnderTest()
        {
            SUT = new THtmlControl
                      {
                          PageNavigator = PageNavigator = SubstituteFor<IPageNavigator>(),
                          ElementFinder = ElementFinder = SubstituteFor<IElementFinder>(),
                          ScriptExecutor = ScriptExecutor = SubstituteFor<IScriptExecutor>(),
                          ComponentFactory = ComponentFactory = SubstituteFor<IComponentFactory>()
                      };

        }
    }
}