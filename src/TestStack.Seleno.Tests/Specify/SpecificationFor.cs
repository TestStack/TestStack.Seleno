using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using AutofacContrib.NSubstitute;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.Specify
{
    public abstract class SpecificationFor<T> : Specification
    {
        public T SUT { get; set; }
        protected AutoSubstitute AutoSubstitute;

        protected SpecificationFor()
        {
            AutoSubstitute = CreateContainer();
            InitialiseSystemUnderTest();
        }

        public virtual void InitialiseSystemUnderTest()
        {
            SUT = AutoSubstitute.Resolve<T>();
        }

        public TSubstitute SubstituteFor<TSubstitute>(object anonynousParameterObject) where TSubstitute : class
        {
            return SubstituteFor<TSubstitute>(anonynousParameterObject.ToNamedParameters());
        }

        public TSubstitute SubstituteFor<TSubstitute>(params Parameter[] parameters) where TSubstitute : class
        {
            return AutoSubstitute.ResolveAndSubstituteFor<TSubstitute>(parameters);
        }

        public override Type Story
        {
            get { return typeof(T); }
        }

        private static AutoSubstitute CreateContainer()
        {
            Action<ContainerBuilder> autofacCustomisation = c => c
                .RegisterType<T>()
                .FindConstructorsWith(t => t.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                .PropertiesAutowired();
            return new AutoSubstitute(autofacCustomisation);
        }
    }
}
