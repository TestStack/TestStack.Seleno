using System.Linq;
using NSubstitute;

namespace TestStack.Seleno.Tests.Specify
{
    public static class NSubstituteExtensions
    {
        public static void Returns<T>(this T value, params T[] returnThese)
        {
            if (returnThese != null && returnThese.Any())
            {
                value.Returns(returnThese.First(), returnThese.Skip(1).ToArray());
            }
        }
    }
}
