using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.Seleno.AcceptanceTests.PageObjects;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Locators;

namespace TestStack.Seleno.AcceptanceTests.Specifications
{
    public abstract class WaitTests
    {
        protected Page Page = new Page();

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
                Page.AssertThatElements.Exist(By.jQuery("#target"));
                Page.WaitFor.AjaxCallsToComplete();
            }

            public void ThenTheApiReturnsWhenTheCallFinishes()
            {
                Page.AssertThatElements.DoNotExist(By.jQuery("#target"));
            }
        }
    }
}
