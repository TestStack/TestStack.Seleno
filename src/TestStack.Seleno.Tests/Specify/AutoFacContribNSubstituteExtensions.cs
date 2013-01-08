using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutofacContrib.NSubstitute;
using NSubstitute;

namespace TestStack.Seleno.Tests.Specify
{
    public static class AutoFacContribNSubstituteExtensions
    {
        public static TFirstService SubstituteFor<TFirstService, TSecondService>(this AutoSubstitute autoSubstitute,
                                                                                     params object[] parameters)
            where TFirstService : class
            where TSecondService : class
        {
            return autoSubstitute.Provide(Substitute.For<TFirstService, TSecondService>(parameters));
        }

        public static TFirstService SubstituteFor<TFirstService, TSecondService, TThirdService>(this AutoSubstitute autoSubstitute,
                                                                                                      params object[] parameters)
            where TFirstService : class
            where TSecondService : class
            where TThirdService : class
        {
            return autoSubstitute.Provide(Substitute.For<TFirstService, TSecondService, TThirdService>(parameters));
        }

        public static object SubstituteFor(this AutoSubstitute autoSubstitute, IEnumerable<Type> types, params object[] parameters)
        {
            return autoSubstitute.Provide(Substitute.For(types.ToArray(), parameters));
        }

    }
}
