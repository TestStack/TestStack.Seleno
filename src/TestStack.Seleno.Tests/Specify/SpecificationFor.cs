using System;
using System.Collections.Generic;
using System.Linq;
using AutofacContrib.NSubstitute;
using NSubstitute;

namespace TestStack.Seleno.Tests.Specify
{
    public abstract class SpecificationFor<T> : Specification 
        where T : class
    {
        public  T SUT { get; protected set; }

        public abstract class Implementing<TImplemented> : SpecificationFor<T>
            where TImplemented : class
        {
            
            public TImplemented SUTAsFirstImplementation
            {
                get { return SUT as TImplemented; }
            }

            public override void InitialiseSystemUnderTest()
            {
                SUT = _autoSubstitute.SubstituteFor<T, TImplemented>();
            }
        }

        public abstract class Implementing<TFirstImplemented, TSecondImplemented> : Implementing<TFirstImplemented>
            where TFirstImplemented : class
            where TSecondImplemented : class
        {

            public TSecondImplemented SUTAsSecondImplementation
            {
                get { return SUT as TSecondImplemented; }
            }

            public override void InitialiseSystemUnderTest()
            {
                SUT = _autoSubstitute.SubstituteFor<T, TFirstImplemented, TSecondImplemented>(); 
            }
          
        }

        public abstract class Implementing<TFirstImplemented, TSecondImplemented, TThirdImplemented> : Implementing<TFirstImplemented, TSecondImplemented>
            where TFirstImplemented : class
            where TSecondImplemented : class
            where TThirdImplemented : class
        {

            public TThirdImplemented SUTAsThirdImplementation
            {
                get { return SUT as TThirdImplemented; }
            }

            public override void InitialiseSystemUnderTest()
            {
                SUT = Fake<T>(new[]
                                  {
                                      typeof (TFirstImplemented),
                                      typeof (TSecondImplemented),
                                      typeof (TThirdImplemented)
                                  });
            }

        }



        protected SpecificationFor()
        {
            InitialiseSystemUnderTest();
        }

        public virtual void InitialiseSystemUnderTest()
        {
            SUT = _autoSubstitute.Resolve<T>();
        }

        public override Type Story
        {
            get { return typeof(T); }
        }
    }
}
