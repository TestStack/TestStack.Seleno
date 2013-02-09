namespace TestStack.Seleno
{
    public class InaccessibleSelenoException : SelenoException
    {
        public InaccessibleSelenoException()
            : base("You cannot instantiate a Seleno Page Object. Please use SUT.NavigateToInitialPage()")
        {
            // log the exception...
        }
    }
}
