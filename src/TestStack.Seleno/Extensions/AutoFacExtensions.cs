using System.Linq;
using Autofac;
using Autofac.Core;

namespace TestStack.Seleno.Extensions
{
    public static class AutofacExtensions
    {
        public static Parameter[] ToNamedParameters(this object anonymousParameters)
        {
            var namedParameters = Enumerable.Empty<Parameter>().ToArray();
            if (anonymousParameters != null)
            {
                namedParameters =
                    anonymousParameters
                        .GetType()
                        .GetProperties()
                        .Select(p => new NamedParameter(p.Name, p.GetValue(anonymousParameters, null)))
                        .ToArray();
            }

            return namedParameters;
        }
    }
}
