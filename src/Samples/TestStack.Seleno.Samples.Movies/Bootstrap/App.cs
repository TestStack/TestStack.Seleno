namespace TestStack.Seleno.Samples.Movies.Bootstrap
{
    public static class App
    {
        public static void Start()
        {
            AutofacBootstrapper.Configure();
            MvcBootstrapper.Configure();
            AutoMapperBootstrapper.Configure();
        }
    }
}