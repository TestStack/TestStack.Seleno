using System;
using System.Reflection;
using Autofac;
using Autofac.Core.Activators.Reflection;
using AutofacContrib.NSubstitute;

namespace TestStack.Seleno.Tests.Specify
{
    public abstract class SpecificationFor<T> : Specification
    {
        public T SUT { get; set; }
        protected AutoSubstitute _autoSubstitute;

        protected SpecificationFor()
        {
            _autoSubstitute = CreateContainer();
            InitialiseSystemUnderTest();
        }

        public virtual void InitialiseSystemUnderTest()
        {
            SUT = _autoSubstitute.Resolve<T>();
        }

        public TFake Fake<TFake>() where TFake : class
        {
            return _autoSubstitute.ResolveAndSubstituteFor<TFake>();
        }

        public override Type Story
        {
            get { return typeof(T); }
        }

        private AutoSubstitute CreateContainer()
        {
            Action<ContainerBuilder> autofacCustomisation = c => c
                .RegisterType<T>()
                .FindConstructorsWith(new BindingFlagsConstructorFinder(BindingFlags.Public | BindingFlags.NonPublic));
            return new AutoSubstitute(autofacCustomisation);
        }
    }
}
