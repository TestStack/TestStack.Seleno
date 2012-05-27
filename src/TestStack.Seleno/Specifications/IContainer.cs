namespace TestStack.Seleno.Specifications
{
    public interface IContainer
    {
        T Resolve<T>();
    }
}