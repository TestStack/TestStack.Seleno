namespace Seleno.Specifications
{
    public interface IContainer
    {
        T Resolve<T>();
    }
}