using System.Reflection;
using Autofac;
using Autofac.Core.Activators.Reflection;

namespace TestStack.Seleno.Tests.Specify
{
    public class InternalActionSpecificationFor<T> : SpecificationFor<T>
    {
        public InternalActionSpecificationFor()
            : base(b => b.RegisterType<T>()
                         .FindConstructorsWith(new BindingFlagsConstructorFinder(BindingFlags.NonPublic))) 
        { }

    }
}