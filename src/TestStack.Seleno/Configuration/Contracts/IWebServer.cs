namespace TestStack.Seleno.Configuration.Contracts
{
    public interface IWebServer : ILifecycleTask
    {
        string BaseUrl { get; }
    }
}
