using System;
using AutofacContrib.NSubstitute;

namespace TestStack.Seleno.Tests.Specify
{
    public abstract class SpecificationFor<T> : Specification
    {
        public T SUT { get; set; }
        protected AutoSubstitute _autoSubstitute;

        public SpecificationFor()
        {
            _autoSubstitute = new AutoSubstitute();
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
