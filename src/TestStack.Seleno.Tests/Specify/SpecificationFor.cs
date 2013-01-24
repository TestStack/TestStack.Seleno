using System;
using Autofac;
using AutofacContrib.NSubstitute;

namespace TestStack.Seleno.Tests.Specify
{
    public abstract class SpecificationFor<T> : Specification
    {
        public T SUT { get; set; }
        protected AutoSubstitute _autoSubstitute;
        private Action<ContainerBuilder> _emptyBuilderAction = builder => { };

        protected SpecificationFor(Action<ContainerBuilder> containerBuilderAction = null)
        {
            _autoSubstitute = new AutoSubstitute(containerBuilderAction ?? _emptyBuilderAction);
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
    }
}
