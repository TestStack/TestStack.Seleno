using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.PageObjects.Actions.ScriptExecutor
{
    public class when_executing_script_with_formatting_arguments : ScriptExecutorSpecification
    {
        private const string ScriptToBeFormated = "$('#{0}').hide()";
       
        public when_executing_script_with_formatting_arguments()
        {
            SUT.ExecuteScript(ScriptToBeFormated, "id");
        }

        public void Then_it_should_execute_with_javascriptExecutor_with_passed_arguments()
        {
            Fake<IJavaScriptExecutor>().Received().ExecuteScript(ScriptToBeFormated, "id");
        }
    }
}