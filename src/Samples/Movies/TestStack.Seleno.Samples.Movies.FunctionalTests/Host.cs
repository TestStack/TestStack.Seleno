using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.Samples.Movies.FunctionalTests
{
    public static class Host
    {
        static Host()
        {
            Instance.Run("TestStack.Seleno.Samples.Movies", 19456);
        }

        public static readonly SelenoHost Instance = new SelenoHost();
    }
}