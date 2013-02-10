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

        public TSubstitute SubstituteFor<TSubstitute>() where TSubstitute : class
        {
            return _autoSubstitute.ResolveAndSubstituteFor<TSubstitute>();
        }

        public override Type Story
        {
            get { return typeof(T); }
        }

        private AutoSubstitute CreateContainer()
        {
            Action<ContainerBuilder> autofacCustomisation = c => c
                .RegisterType<T>()
                .FindConstructorsWith(t => t.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                .PropertiesAutowired();
            return new AutoSubstitute(autofacCustomisation);
        }
    }
}
