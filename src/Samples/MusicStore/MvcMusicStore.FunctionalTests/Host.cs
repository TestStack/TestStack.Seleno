using TestStack.Seleno.Configuration;

namespace MvcMusicStore.FunctionalTests
{
    public static class Host
    {
        public static readonly SelenoHost Instance = new SelenoHost();

        static Host()
        {
            Instance.Run("MvcMusicStore", 12345);
        }
    }
}