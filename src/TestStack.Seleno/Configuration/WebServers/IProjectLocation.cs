namespace TestStack.Seleno.Configuration.WebServers
{
    /// <summary>
    /// The location of the Visual Studio web project under test.
    /// </summary>
    public interface IProjectLocation
    {
        /// <summary>
        /// The absolute path to the web project.
        /// </summary>
        /// <value>
        /// The full path.
        /// </value>
        string FullPath { get; }
    }
}
