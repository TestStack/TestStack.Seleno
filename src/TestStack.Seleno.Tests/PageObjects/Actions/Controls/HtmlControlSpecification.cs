using System;
using System.Linq.Expressions;
using TestStack.Seleno.Configuration.ControlIdGenerators;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    abstract class HtmlControlSpecificationFor<THtmlControl, TPropertyType> : SpecificationFor<THtmlControl>
        where THtmlControl : HTMLControl, new()
    {
        protected IPageNavigator PageNavigator { get; private set; }
        protected IElementFinder ElementFinder { get; private  set; }
        protected IExecutor Executor { get; private set; }
        protected IComponentFactory ComponentFactory { get; private set; }

        protected HtmlControlSpecificationFor(Expression<Func<TestViewModel, TPropertyType>> htmlControlPropertySelector)
        {
            SUT.ViewModelPropertySelector = htmlControlPropertySelector;
        }

        public override void InitialiseSystemUnderTest()
        {
            SUT = new THtmlControl
                      {
                          PageNavigator = PageNavigator = SubstituteFor<IPageNavigator>(),
                          ElementFinder = ElementFinder = SubstituteFor<IElementFinder>(),
                          Executor = Executor = SubstituteFor<IExecutor>(),
                          ComponentFactory = ComponentFactory = SubstituteFor<IComponentFactory>(),
                          ControlIdGenerator = new DefaultControlIdGenerator()
                      };

        }
    }
}