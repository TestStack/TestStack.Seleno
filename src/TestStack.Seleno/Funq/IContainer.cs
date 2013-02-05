namespace Funq
{
    public interface IContainer
    {
        TService Resolve<TService>();
    }
}