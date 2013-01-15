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

        private readonly AutoSubstitute _autoSubstitute = new AutoSubstitute();

        protected SpecificationFor()
        {
            InitialiseSystemUnderTest();
        }


        public virtual TService Fake<TService>(params Type[] implementedTypes)
            where TService : class
        {
            if (implementedTypes != null && implementedTypes.Any())
            {
                var types = implementedTypes.ToList();
                types.Insert(0, typeof (TService));
                return (TService) _autoSubstitute.ResolveAndSubstituteFor(types);
            }
            
            return _autoSubstitute.ResolveAndSubstituteFor<TService>();
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
