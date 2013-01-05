using TestStack.Samples.ModelIoTesting.Fixtures;
using TestStack.Samples.ModelIoTesting.ViewModels;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace TestStack.Samples.ModelIoTesting.PageObjects
{
    public class Form1Page : Page<Form1ViewModel>
    {
        public AssertionResultPage InputFixtureA()
        {
            Input().Model(Form1Fixtures.A);
            return Navigate().To<AssertionResultPage>(By.CssSelector("input[type=submit]"));
        }

        public Form1ViewModel ReadModel()
        {
            return Read().ModelFromPage();
        }
    }
}