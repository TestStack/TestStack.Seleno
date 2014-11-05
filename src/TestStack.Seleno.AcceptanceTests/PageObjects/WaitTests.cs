using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace TestStack.Seleno.AcceptanceTests.PageObjects
{
    public abstract class WaitTests
    {
        protected FormWithAJAXPage Page;

        public WaitTests()
        {
            Page = Host.Instance.NavigateToInitialPage<HomePage>()
                .GoToFormWithAjax();
        }

        [Test]
        public void Verify()
        {
            this.BDDfy();
        }

        public class AJAX_completion : WaitTests
        {
            public void GivenThereIsAPendingAjaxCall()
            {
            }

            public void WhenWaitingForAjaxCompletion()
            {
                Page.AssertThatTargetElementExists()
                    .WaitForAjaxCallsToComplete();
            }

            public void ThenTheApiReturnsWhenTheCallFinishes()
            {
                Page.AssertThatTargetElementDoesNotExist();
            }
        }
    }
    public class FormWithAJAXPage : Page
    {
        public FormWithAJAXPage AssertThatTargetElementExists()
        {
            AssertThatElements.Exist(By.jQuery("#target"));
            return this;
        }
        public FormWithAJAXPage WaitForAjaxCallsToComplete()
        {
            WaitFor.AjaxCallsToComplete();
            return this;
        }
        public FormWithAJAXPage AssertThatTargetElementDoesNotExist()
        {
            AssertThatElements.DoNotExist(By.jQuery("#target"));
            return this;
        }
    }

}
